using Google.Cloud.Firestore;
using Google.Protobuf.WellKnownTypes;
using GoStay.Common.Configuration;
using GoStay.Web.Areas.Admin.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Kiota.Abstractions;
using Newtonsoft.Json;
using ServiceStack;
using System.Web.Helpers;
using goStayCore.CommonCode;
using GoStay.Service.Api.Ticket;
using GoStay.DataDto.OrderDto;
using GoStay.Data.Ticket;
using ServiceStack.Html;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph.Models;
using ServiceStack.Host;
using System.Text.RegularExpressions;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FireBaseHomeController : Controller
    {
        private string directorio = AppConfigs.Directorio;
        private string projectId;
        private FirestoreDb _firestore;
        private readonly IOrdersTicketApiServices _orderTicketServices;
        private readonly ISessionManager _sessionManag;
        public FireBaseHomeController(IOrdersTicketApiServices orderTicketServices, ISessionManager sessionManag)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", directorio);
            projectId = "gostay-1ae07";
            _firestore = FirestoreDb.Create(projectId);
            _orderTicketServices = orderTicketServices;
            _sessionManag = sessionManag;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> ListCustomer()
        {
            Query query = _firestore.Collection("customer");
            QuerySnapshot snapshots = await query.GetSnapshotAsync();
            List<CustomerFirebase> listCustomer = new List<CustomerFirebase>();
            foreach (DocumentSnapshot documentSnapshot in snapshots.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> test = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(test);
                    CustomerFirebase customer = JsonConvert.DeserializeObject<CustomerFirebase>(json);
                    customer.CustomerId = documentSnapshot.Id;
                    listCustomer.Add(customer);
                }
            }
            return Json(listCustomer);
        }
        public class FormatMess {
            public string ErrorCode { get; set; }
            public string ErrorMess { get; set; }
        }
        [FirestoreData]
        public class CustomerFirebase
        {
            public string CustomerId { get; set; }

            /// <summary>
            /// khai bao ds cac truong tren firestore
            /// </summary>
            [FirestoreProperty]
            public string FullName { get; set; }
            [FirestoreProperty]
            public string Messenge { get; set; }
            [FirestoreProperty]
            public string Phone { get; set; }
            [FirestoreProperty]
            public string Email { get; set; }
            [FirestoreProperty]
            public string Time { get; set; }
            [FirestoreProperty]
            public string Status { get; set; }
            [FirestoreProperty]
            public string OrderId { get; set; }
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Create([FromBody] Customer input)
        {
            UpdateStatus param = new UpdateStatus();
            
            param.TicketId = input.OrderId;//"Id";
            param.UserId = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            var phoneNumber = formatPhoneNumber(input.Phone);
            FormatMess results = new FormatMess();
            results.ErrorCode = "-1";
            if (string.IsNullOrEmpty(phoneNumber))
            {
                results.ErrorCode = "-2";
                results.ErrorMess = "Số điện thoại không hợp lệ";
                return Json(results);
            }
            var update = _orderTicketServices.UpdateStatus(param);
            try
            {
                
                if (update.Data == "Success")
                {
                    var timeTmp = DateTime.Now;
                    input.Time = (string.IsNullOrEmpty(input.Time) ? ((DateTimeOffset)timeTmp).ToUnixTimeSeconds().ToString() : input.Time);
                    //CollectionReference colRf = _firestore.Collection("customer");
                    //Dictionary<string, object> value = new Dictionary<string, object>()
                    //    {
                    //        {"FullName", input.FullName },
                    //        {"Phone", input.Phone },
                    //        {"Email", input.Email },
                    //        {"Messenge", input.Messenge },
                    //        {"Time", input.Time },
                    //        {"Status", "2" },
                    //        {"OrderId", input.OrderId.ToString() },
                    //    };
                    //colRf.AddAsync(value);

                    DocumentReference colRf = _firestore.Collection("customer").Document(param.UserId.ToString() + "-" + input.Phone + "-" + input.OrderId.ToString());
                    Dictionary<string, object> value = new Dictionary<string, object>()
                        {
                            {"FullName", input.FullName },
                            {"Phone", phoneNumber },
                            {"Email", input.Email },
                            {"Messenge", input.Messenge },
                            {"Time", input.Time },
                            {"Status", "2" },
                            {"OrderId", input.OrderId.ToString() },
                        };
                    colRf.SetAsync(value);



                    results.ErrorCode = "0";
                    results.ErrorMess = "Success";
                }
                else
                {
                    results.ErrorMess = update.Data;
                }
            } 
            catch (Exception ex)
            {
                results.ErrorCode = "-1";
                results.ErrorMess = ex.Message;
            }
            return Json(results);
        }

        public string formatPhoneNumber(string phonenumber)
        {
            // Regex to check valid phonenumber
            if(string.IsNullOrEmpty(phonenumber)) return "";
            phonenumber = phonenumber.Trim().Replace(" ", "");
            Console.WriteLine("Hello World 0 {0}", phonenumber);
            // check for this in case they've entered 44 (0)xxxxxxxxx or similar
            if (phonenumber.StartsWith("03") || phonenumber.StartsWith("05") || phonenumber.StartsWith("07") || phonenumber.StartsWith("08") || phonenumber.StartsWith("09"))
            {
                phonenumber = phonenumber.Remove(0, 1);
                phonenumber = "+84" + phonenumber;
            }
            if (phonenumber.StartsWith("+84"))
            {
                var leng = phonenumber.Length;
                phonenumber = (leng == 12) ? phonenumber : "";
            }
            if (phonenumber.StartsWith("84"))
            {
                var leng = phonenumber.Length;
                phonenumber = (leng == 11) ? ("+" + phonenumber) : "";
            }
            return phonenumber;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<string> Update(CustomerFirebase input)
        {
            var time = DateTime.Now;
            input.Time = (string.IsNullOrEmpty(input.Time) ? ((DateTimeOffset)time).ToUnixTimeSeconds().ToString() : input.Time);
            FormatMess results = new FormatMess();
            results.ErrorCode = "-1";
            try
            {
                DocumentReference colRf = _firestore.Collection("customer").Document(input.CustomerId);
                await colRf.SetAsync(input, SetOptions.Overwrite);
                results.ErrorCode = "0";
                results.ErrorMess = "Success";
            }
            catch (Exception ex)
            {
                results.ErrorCode = "-1";
                results.ErrorMess = ex.Message;
            }
            return results.ErrorCode;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<string> Delete(CustomerFirebase input)
        {
            var time = DateTime.Now;
            input.Time = (string.IsNullOrEmpty(input.Time) ? ((DateTimeOffset)time).ToUnixTimeSeconds().ToString() : input.Time);
            FormatMess results = new FormatMess();
            results.ErrorCode = "-1";
            try
            {
                DocumentReference colRf = _firestore.Collection("customer").Document(input.CustomerId);
                await colRf.DeleteAsync();
                results.ErrorCode = "0";
                results.ErrorMess = "Success";
            }
            catch (Exception ex)
            {
                results.ErrorCode = "-1";
                results.ErrorMess = ex.Message;
            }
            return results.ErrorCode;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult SupriceEmail(string email)
        {
            
            FormatMess results = new FormatMess();
            results.ErrorCode = "-1";
            results.ErrorMess = "System error, please check email again!";
            try
            {
                    var timeTmp = DateTime.Now;
                    var time = ((DateTimeOffset)timeTmp).ToUnixTimeSeconds().ToString();
                    DocumentReference colRf = _firestore.Collection("EmailRegister").Document(email);
                    Dictionary<string, object> value = new Dictionary<string, object>()
                    {
                            {"Email", email },
                            {"Time", time },
                        };
                    colRf.SetAsync(value);
                    results.ErrorCode = "0";
                    results.ErrorMess = "Success";
              
            }
            catch (Exception ex)
            {
                results.ErrorCode = "-1";
                results.ErrorMess = "System error, please check email again!";
            }
            return Json(results);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult SavePersistent(string tableName, string uid, bool isLogin, string? deviceId, bool? IsFirstLogin)
        {
            FormatMess results = new FormatMess();
            results.ErrorCode = "-1";
            results.ErrorMess = "System error, please check email again!";

            var cookies = Request.Cookies[AppConfigs.CurrentUserCK];
            try
            {
                var timeTmp = DateTime.Now;
                var time = ((DateTimeOffset)timeTmp).ToUnixTimeSeconds().ToString();
                DocumentReference colRf = _firestore.Collection(tableName).Document(uid + "__" + deviceId);
                Dictionary<string, object> value = new Dictionary<string, object>()
                {
                    {"Id", uid },
                    {"Cookies", cookies },
                    {"Time", time },
                    {"IsLogin", true},
                    {"DeviceId", deviceId},
                    {"IsFirstLogin", IsFirstLogin},
                };
                colRf.SetAsync(value);

                if (!isLogin)
                {
                    CookieOptions remove = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Append("userId", string.Empty, remove);
                }
                else
                {
                    CookieOptions options = new CookieOptions
                    {
                        Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
                    };
                    Response.Cookies.Append("userId", uid, options);
                }

                results.ErrorCode = "0";
                results.ErrorMess = "Success";
            }
            catch (Exception ex)
            {
                results.ErrorCode = "-1";
                results.ErrorMess = "System error, please check email again!";
            }
            return Json(results);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult CreateOrder(string Id, string Link, string Code, string TotalPrice, string DateCreate, string UserId, 
            string Email, string Method, string PrePay,string Image, string DataRoomOrder, string HotelName, string HotelAddress)
        {

            FormatMess results = new FormatMess();
            results.ErrorCode = "-1";
            results.ErrorMess = "System error, please check email again!";
            try
            {
                Link = AppConfigs.Domain + "Orders/Order/" + Link;
                var timeTmp = DateTime.Now;
                var time = ((DateTimeOffset)timeTmp).ToUnixTimeSeconds().ToString();
                DocumentReference colRf = _firestore.Collection("Order").Document(Id.ToString());
                Dictionary<string, object> value = new Dictionary<string, object>()
                {
                    {"Id", Id },
                    {"Link", Link },
                    {"Time", time },
                    {"Code", Code},
                    {"TotalPrice", TotalPrice},
                    {"DateCreate", DateCreate},
                    {"UserId", UserId},
                    {"Email", Email},
                    {"Method", Method},
                    {"PrePay", PrePay},
                    {"Image", Image},
                    {"DataRoomOrder", DataRoomOrder},
                    {"HotelName", HotelName},
                    {"HotelAddress", HotelAddress}

                    //{"DeviceId", deviceId},
                    //{"IsFirstLogin", IsFirstLogin},
                };
                colRf.SetAsync(value);

                results.ErrorCode = "0";
                results.ErrorMess = "Success";
            }
            catch (Exception ex)
            {
                results.ErrorCode = "-1";
                results.ErrorMess = "System error, please check email again!";
            }
            return Json(results);
        }
    }
}
