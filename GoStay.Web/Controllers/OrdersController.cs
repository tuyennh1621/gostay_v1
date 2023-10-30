using Microsoft.AspNetCore.Mvc;
using GoStay.Service.Api.Hotels;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.OrderDto;
using System.Globalization;
using goStayCore.CommonCode;
using GoStay.Common;
using Newtonsoft.Json;
using GoStay.Common.Configuration;
using Microsoft.Graph.Models;
using GoStay.Service;
using GoStay.Web.Model;
using Hanssens.Net;
using GoStay.Service.Api.Payment;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using System.Security.Cryptography;
using ServiceStack.Web;
using Microsoft.Net.Http.Headers;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;
using Microsoft.AspNetCore.Http;
using GoStay.Common.Enums;
using GoStay.Common.Helper;
using Twilio.TwiML.Voice;
using Windows.UI.Xaml;
using Kendo.Mvc.UI;
using GoStay.DataDto.HotelDto;
using GoStay.Service.Api.Scheduler;

namespace GoStay.Web.Controllers
{
    public class OrdersController : Controller
    {
        private IOrdersApiServices _ordersApiServices;
        private IHotelApiServices _hotelApiServices;
        private ISchedulerApiServices _schedulerApiServices;

        private static OrderGetInfoDto result = new OrderGetInfoDto();

        private readonly ISessionManager _SessionManag;
        public string localSessionID = "";
        private readonly IPaymentApiServices _paymentApiServices;

        public OrdersController(IOrdersApiServices ordersApiServices, ISessionManager sessionManag, IHotelApiServices hotelApiServices, 
            IPaymentApiServices paymentApiServices, ISchedulerApiServices schedulerApiServices)

        {
            _ordersApiServices = ordersApiServices;
            _SessionManag = sessionManag;
            _hotelApiServices = hotelApiServices;
            _paymentApiServices = paymentApiServices;
            _schedulerApiServices = schedulerApiServices;
        }
        public string ConfirmCheckIn(int IdOrder)
        {
            try
            {
                var update = _ordersApiServices.UpdateStatusOrder(new UpdateStatusOrderParam
                {
                    IdOder = IdOrder,
                    Status = 4
                });
                var result = _ordersApiServices.GetOrderbyId(IdOrder);

                var model = new OrderSuccessInfor();

                model.RoomOrner = false;
                model.CheckIn = true;
                var userid = _SessionManag.GetUserGostay().UserId;
                string updatestatus = "";
                if(result.Style ==1)
                {
                    var data = _hotelApiServices.ChangeStatusRoom((int)result.Room.Id, 2);
                    updatestatus += $"{result.Room.Id} - {data.Message} / ";
                }    
                    
                
                if (result.Style ==1)
                {
                    model.OrderSuccessInforDto = result;

                    return "1";

                    //return PartialView("~/Views/Orders/Xacnhanhdatphong.cshtml", model);

                }
                else
                {
                    model.OrderSuccessInforDto = result;
                    return "2";
                    // return PartialView("~/Views/Orders/Xacnhanhdattour.cshtml", result);
                }

            }
            catch (Exception ex)
            {
                return "DAT KHONG THANH CONG" + ex.Message.ToString();
                // return PartialView(ex.Message);
            }
        }
        public IActionResult XacnhanPTThanhToan(int orderId)
        {
            var res = _ordersApiServices.GetOrderbyId(orderId);
            try
            {
                FileHelper.GeneratorFileByDay(FileStype.Log, $"Start payment:... id: {res.Id} code: {res.Ordercode} price. {res.TotalAmount}", "Order");
                if ( res.Status == 3)
                {
                    return RedirectToAction("Index", "Home");
                }

                UserGostay _user = _SessionManag.GetUserGostay();


                if (_user.isLogined)
                {
                    res.UserName = _user.UserName;
                    res.Address = _user.Address;
                    res.Mobile = _user.MobileNo;
                    res.Email = _user.Email;
                    res.IsLogined = _user.isLogined;

                    if (res.IdUser == 1)
                    {
                        UpdateUserIdBySessionParam param = new UpdateUserIdBySessionParam();
                        param.IdUser = _user.UserId;
                        param.Session = res.Session;
                        _ordersApiServices.UpdateUserIDbySession(param);
                    }
                }

                _ordersApiServices.UpdateStatusOrder(new UpdateStatusOrderParam
                {
                    IdOder = res.Id,
                    Status = 2
                });

                res.Status = 2;
                return View(res);
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(FileStype.Error, $"Detail exception;... {ex.ToString()}", "XacnhanPTThanhToan");
                return View("Đã xảy ra lỗi" + ex.Message.ToString());
            }
        }
        [HttpPost]
        public IActionResult PaymentMethodForHotel(PaymentMethodEnum flexRadio, TypePaymentEnum typePayment, string Email, int orderId)  //typePayment = 7 || 8 là thanh toán online <> 100% || 10% ; 4: 0% trả sau
        {
            var res = _ordersApiServices.GetOrderbyId(orderId);
            var resultChangePaymentmethod = _ordersApiServices.UpdatePTTTOrder(new UpdatePTTTOrderParam() { IdOder = orderId, IdPTThanhtoan = (byte)typePayment });
            FileHelper.GeneratorFileByDay(FileStype.Log, $"payment hotel {res.Id}. code {res.Ordercode}. title {res.Title} .TotalNewPrice {res.TotalAmount} ", "Order");
            //if (res != null && res.ListOrderDetails.Count <= 0)
            //{
            //    return RedirectToAction("Index", "Hotels");
            //}
            double total = 0;
            double percen = (typePayment == TypePaymentEnum.Postpaid) ? 0 : ((typePayment == TypePaymentEnum.Deposit) ? 0.1 : ((typePayment == TypePaymentEnum.PrePayment) ? 1 : 1));
            res.Email = Email;

            if (typePayment == TypePaymentEnum.Deposit || typePayment == TypePaymentEnum.PrePayment) // tính toán số tiền
            {
                total = Math.Round(Decimal.ToDouble((decimal)res.TotalAmount) * percen, 0);
            }
            else if (typePayment == TypePaymentEnum.Postpaid || flexRadio == PaymentMethodEnum.Postpaid)
            { // thanh toán sau
                ResultPayment result = new ResultPayment();
                result.errorCode = "0";
                return Redirect("/order/successful?errorCode=0&amount=0&idOrder=" + res.Id);
            }
            else// đặt cọc
            {
                return Redirect("/order/payments?orderId=" + res.Id);
            }
            if (flexRadio == PaymentMethodEnum.MoMo) // momo
            {
                var resultMomo = _paymentApiServices.MomoPay(total.ToString(), AppConfigs.Domain, "order/successful", res.Ordercode.ToString().Replace(",", "-"), res.Id.ToString());
                return Redirect(resultMomo.GetValue("payUrl").ToString() + "&idOrder=" + res.Id);
            }
            else if (flexRadio == PaymentMethodEnum.VNPay)
            {
                total = total * 100;
                var resultVN = _paymentApiServices.VNPay(total.ToString(), "sgo:09", AppConfigs.Domain, "order/successful", res.Ordercode.ToString().Replace(",", "-"), res.Id.ToString());
                return Redirect(resultVN);
            }
            else if (flexRadio == PaymentMethodEnum.Appotapay)
            {
                var resultApp = _paymentApiServices.Appota(total.ToString(), AppConfigs.Domain, "order/successful", res.Ordercode.ToString().Replace(",", "-"), res.Id.ToString());
                return Redirect(resultApp.GetValue("paymentUrl").ToString() + "&idOrder=" + res.Id);
            }
            else
            {
                //ResultPayment result = new ResultPayment();
                //result.errorCode = "0";
                //return Xacnhanhdatphong(result);
                return Redirect("/order/payments?orderId=" + res.Id);
            }
        }

        public IActionResult Xacnhanhdatphong(ResultPayment results)
        {
            var res = _ordersApiServices.GetOrderbyId(results.idOrder);
            FileHelper.GeneratorFileByDay(FileStype.Log, $"Success hotel {res.Id}. code {res.Ordercode}. title {res.Title} .price: {res.TotalAmount} ", "Order");
            if(res.Status==3)
            {
                return RedirectToAction("Order", "Orders", new {Id = res.Id});
            }    
            //if (res.ListOrderDetails.Count <= 0)
            //{
            //    return RedirectToAction("Index", "Hotels");
            //}
            //else
            //{
                var model = new OrderSuccessInfor();
                if ((results.errorCode == "0" && results.resultCode == 0 && results.vnp_ResponseCode == 0)  // appota
                || (results.resultCode == 0 && results.errorCode == null && results.vnp_ResponseCode == 0)) // momo & vnpay
                {
                    
                    model.RoomOrner = false;
                    _ordersApiServices.UpdateStatusOrder(new UpdateStatusOrderParam
                    {
                        IdOder = res.Id,
                        Status = 3
                    });

                    res.Status = 3;
                    model.OrderSuccessInforDto = res;
                    model.amount = results.vnp_Amount != 0 ? (results.vnp_Amount/100).ToString() : results.amount;
                    var resultprepayment = _ordersApiServices.UpdatePrePayment(new UpdatePrePaymentOrderParam() { IdOder = results.idOrder, PrePayment = (decimal)Int64.Parse(model.amount) });

                    return View(model); 
                }
                else
                {
                    return Redirect("/order/payments?orderId=" + res.Id);
                }
                

            //}
        }
        [HttpPost]
        public IActionResult PaymentMethodForTour(PaymentMethodEnum flexRadio, TypePaymentEnum typePayment, string Email, int orderId)  //typePayment =7 || 8 là thanh toán online <> 100% || 10% ; 4: 0% trả sau
        {
            var res = _ordersApiServices.GetOrderbyId(orderId);
            var resultChangePaymentmethod = _ordersApiServices.UpdatePTTTOrder(new UpdatePTTTOrderParam() { IdOder = orderId, IdPTThanhtoan = (byte)typePayment });
            FileHelper.GeneratorFileByDay(FileStype.Log, $"payment tour {res.Id}. code {res.Ordercode}. title {res.Title} .TotalAmount{res.TotalAmount} ", "Order");

            //if (res != null && res.ListOrderDetails.Count <= 0)
            //{
            //    return RedirectToAction("Index", "Hotels");
            //}
            double total = 0;
            double percen = (typePayment == TypePaymentEnum.Postpaid) ? 0 : ((typePayment == TypePaymentEnum.Deposit) ? 0.1 : ((typePayment == TypePaymentEnum.PrePayment) ? 1 : 1));
            res.Email = Email;
            if (typePayment == TypePaymentEnum.Deposit || typePayment == TypePaymentEnum.PrePayment) // tính toán số tiền
            {
                total = Decimal.ToDouble((decimal)res.TotalAmount) * percen;
                total = Math.Round(total, 0);
                //var resultprepayment = _ordersApiServices.UpdatePrePayment(new UpdatePrePaymentOrderParam() { IdOder = orderId, PrePayment = (decimal)total });

            }
            else if (typePayment == TypePaymentEnum.Postpaid || flexRadio == PaymentMethodEnum.Postpaid)
            { // thanh toán sau
                ResultPayment result = new ResultPayment();
                result.errorCode = "0";
                return Redirect("/Orders/Xacnhanhdattour?errorCode=0&amount=0&idOrder=" + res.Id);
            }
            else// đặt cọc
            {
                return Redirect("/order/payments?orderId=" + res.Id);
            }

            if (flexRadio == PaymentMethodEnum.MoMo) // momo
            {
                var resultMomo = _paymentApiServices.MomoPay(total.ToString(), AppConfigs.Domain, "Orders/Xacnhanhdattour", res.Ordercode.ToString().Replace(",", "-"), res.Id.ToString());
                return Redirect(resultMomo.GetValue("payUrl").ToString() + "&idOrder=" + res.Id);
            }
            else if (flexRadio == PaymentMethodEnum.VNPay)
            {
                total = total * 100;
                var resultVN = _paymentApiServices.VNPay(total.ToString(), "sgo:09", AppConfigs.Domain, "Orders/Xacnhanhdattour", res.Ordercode.ToString().Replace(",", "-"), res.Id.ToString());
                return Redirect(resultVN);
            }
            else if (flexRadio == PaymentMethodEnum.Appotapay)
            {
                var resultApp = _paymentApiServices.Appota(total.ToString(), AppConfigs.Domain, "Orders/Xacnhanhdattour", res.Ordercode.ToString().Replace(",", "-"), res.Id.ToString());
                return Redirect(resultApp.GetValue("paymentUrl").ToString() + "&idOrder=" + res.Id);
            }
            else
            {
                return Redirect("/order/payments?orderId=" + res.Id);
            }
        }

        public IActionResult Xacnhanhdattour(ResultPayment results)
        {
            var res = _ordersApiServices.GetOrderbyId(results.idOrder);
            FileHelper.GeneratorFileByDay(FileStype.Log, $"Success tour {res.Id}. code {res.Ordercode}. title {res.Title} .price: {res.TotalAmount} ", "Order");
            //if (res.ListOrderDetails.Count <= 0)
            //{
            //    return RedirectToAction("Index", "Tour");
            //}
            if ((results.errorCode == "0" && results.resultCode == 0 && results.vnp_ResponseCode == 0) 
                || (results.resultCode == 0 && results.errorCode == null && results.vnp_ResponseCode == 0))
            {
                var model = new OrderSuccessInfor();
                _ordersApiServices.UpdateStatusOrder(new UpdateStatusOrderParam
                {
                    IdOder = res.Id,
                    Status = 3
                });

                res.Status = 3;
                model.OrderSuccessInforDto = res;
                model.amount = results.vnp_Amount != 0 ? (results.vnp_Amount / 100).ToString() : results.amount;

                var resultprepayment = _ordersApiServices.UpdatePrePayment(new UpdatePrePaymentOrderParam() { IdOder = results.idOrder, PrePayment = (decimal)Int64.Parse(model.amount) });
                return View(model);
            }
            else
            {
                return Redirect("/order/payments?orderId=" + res.Id);
            }

            
        }

        public IActionResult Index(int IDHotel, int IDRoom,string SelectDate,
             byte numRoom = 1)
        {
            try
            {
                int yearstart = 0;
                int yearend = 0;
                int monthstart = 0;
                int monthend= 0;
                int daystart  = 0;
                int dayend = 0;

                var check1 = SelectDate.Contains("},");
                var check2 = SelectDate.Contains("],");

                if (!check1 && !check2)
                {
                    var select = SelectDate.Replace("{", string.Empty).Replace("}", string.Empty)
                                    .Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Split(":");

                    yearstart = int.Parse(select[0]);
                    yearend = int.Parse(select[0]);
                    monthstart = int.Parse(select[1]) +1;
                    monthend = int.Parse(select[1]) + 1;
                    daystart = int.Parse(select[2].Split(",").First());
                    dayend = int.Parse(select[2].Split(",").Reverse().First());
                }
                if (check1 && !check2)
                {
                    var select = SelectDate.Split("},");
                    var select0 = select[0].Replace("{", string.Empty).Replace("}", string.Empty)
                                    .Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Split(":");

                    var select1 = select[1].Replace("{", string.Empty).Replace("}", string.Empty)
                                    .Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Split(":");

                    yearstart = int.Parse(select0[0]);
                    yearend = int.Parse(select1[0]);
                    monthstart = int.Parse(select0[1]) + 1;
                    monthend = int.Parse(select1[1]) + 1;
                    daystart = int.Parse(select0[2].Split(",").First());
                    dayend = int.Parse(select1[2].Split(",").Reverse().First());

                }
                if (!check1 && check2)
                {
                    var select = SelectDate.Split("],");
                    var select0 = select[0].Replace("{", string.Empty).Replace("}", string.Empty)
                                    .Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Split(":");

                    var select1 = select[1].Replace("{", string.Empty).Replace("}", string.Empty)
                                    .Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Split(":");

                    yearstart = int.Parse(select0[0]);
                    yearend = int.Parse(select0[0]);
                    monthstart = int.Parse(select0[1]) + 1;
                    monthend = int.Parse(select1[0]) + 1;
                    daystart = int.Parse(select0[2].Split(",").First());
                    dayend = int.Parse(select1[1].Split(",").Reverse().First());
                }
                


                result = new OrderGetInfoDto();

                UserGostay user = _SessionManag.GetUserGostay();

                var cookieOptions = new CookieOptions();
                var cookie = Request.Cookies.FirstOrDefault(x => x.Key == "ordersession").Value;
                if (cookie == null)
                {
                    cookie = Guid.NewGuid().ToString();
                    cookieOptions.Expires = DateTime.Now.AddDays(365);
                    cookieOptions.Path = "/";
                    Response.Cookies.Append("ordersession", $"{cookie}", cookieOptions);
                }

                var order = new OrderDto()
                {
                    Title = "Order Khách sạn: " + $"{IDHotel}" + " Phòng: " + $"{IDRoom}",
                    IdHotel = IDHotel,
                    IdUser = user.UserId,
                    Ordercode = DateTime.UtcNow.TimeOfDay.TotalSeconds.ToString(),
                    Status = 1,
                    IdPtthanhToan = 4,
                    MoreInfo = "string",
                    Session = cookie,
                    IdRoom = IDRoom,
                    NumRoom = numRoom,
                    Style = 1,
                };
                order.CheckInDate = new DateTime(yearstart,monthstart,daystart);
                order.CheckOutDate = new DateTime(yearend, monthend, dayend);

                var temp1 = order.CheckOutDate - order.CheckInDate;
                order.NumNight = (byte)temp1.Value.Days;

                FileHelper.GeneratorFileByDay(FileStype.Log, $"IDHotel {IDHotel}. IDRoom {IDRoom}. " +
                    $"CheckinDate {order.CheckInDate}. CheckoutDate {order.CheckOutDate}. numRooms {numRoom}", "Order");
                if (IDHotel == 0 || IDRoom == 0 )
                {
                    return RedirectToAction("Index", "Hotels");
                }

                result = _ordersApiServices.CheckOrder(order);

                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    return View("result is null");
                }
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(FileStype.Error, $"IDHotel {IDHotel}. IDRoom {IDRoom}. " +
                $"SelectDate {SelectDate}. numRooms {numRoom}. Detail {ex.ToString()}", "Order");
                return View("Đã xảy ra lỗi" + ex.Message.ToString());
            }
        }
        public IActionResult OrderTour(int IdTour, int Adult, int Children, int Infant)
        {
            try
            {
                var cookieOptions = new CookieOptions();
                var cookie = Request.Cookies.FirstOrDefault(x => x.Key == "ordersession").Value;
                if (cookie == null)
                {
                    cookie = Guid.NewGuid().ToString();
                    cookieOptions.Expires = DateTime.Now.AddDays(365);
                    cookieOptions.Path = "/";
                    Response.Cookies.Append("ordersession", $"{cookie}", cookieOptions);
                }

                if (IdTour == 0)
                {
                    return RedirectToAction("Index", "Tour");
                }


                UserGostay user = _SessionManag.GetUserGostay();

                var order = new OrderDto()
                {
                    Title = "Order Tour : " + $"{IdTour}",
                    IdTour = IdTour,
                    IdUser = user.UserId,
                    Ordercode = DateTime.UtcNow.TimeOfDay.TotalSeconds.ToString(),
                    Status = 1,
                    IdPtthanhToan = 4,
                    MoreInfo = "string",
                    Session = cookie,
                    Style = 2,
                    Adult = Adult,
                    Children = Children,
                    Infant = Infant
                };


                result = _ordersApiServices.CheckOrder(order);

                if (result != null)
                {

                    result.Title = result.Tour.TourName;

                    //return PartialView("~/Views/Orders/OrderTour.cshtml", result);
                    return View(result);
                }
                else
                {
                    return View("result is null");
                }

                //return View(result);
            }
            catch (Exception ex)
            {
                return View("Đã xảy ra lỗi" + ex.Message.ToString());
            }

        }
        public IActionResult Order(int Id)
        {

            var result = _ordersApiServices.GetOrderbyId(Id);

            var model = new OrderSuccessInfor();

            model.RoomOrner = false;
            var userid = _SessionManag.GetUserGostay().UserId;
            
            //if (result.ListOrderDetails.Count <= 0) {
            //    return RedirectToAction("Index", "Hotels");
            //}
            if (result.Style==1)
            {

                if (result.Room.IdUser== userid)
                {
                    model.RoomOrner = true;
                }

                model.OrderSuccessInforDto = result;
                model.amount = result.Prepayment.ToString();
                return PartialView("~/Views/Orders/Xacnhanhdatphong.cshtml", model);

            }
            else
            {
                model.OrderSuccessInforDto = result;
                model.amount = result.Prepayment.ToString();

                return PartialView("~/Views/Orders/Xacnhanhdattour.cshtml", model);
            }
        }

        public IActionResult TransitMoreInfo(string param)
        {
            var moreinfo = JsonConvert.DeserializeObject<MoreInfoParam>(param);

            return PartialView("~/Views/Orders/AddMoreInfo.cshtml", moreinfo);

        }
        public IActionResult GetListRoomInOrder(int IdRoom)
        {
            try
            {
                var result = _ordersApiServices.GetRoomInOrder(IdRoom);

                return PartialView("~/Views/Orders/ListRoomOrderPartial.cshtml", result.Data);

            }
            catch
            {
                return null;
            }

        }

        public IActionResult PriceBoxInOrder(int IdRoom)
        {
            try
            {
                var result = _ordersApiServices.GetRoomInOrder(IdRoom);
                OrderPriceBoxDto model = new OrderPriceBoxDto();
                model.Room = result.Data;


                return PartialView("~/Views/Orders/PriceBoxPartial.cshtml", model);
            }
            catch
            {
                return null;
            }

        }
        public string GetHiddenDate(int IdRoom)
        {
            var result = _ordersApiServices.GetBookedDateRoom(IdRoom);
            var futureprice = _schedulerApiServices.GetRoomPriceInFuture("2023-12-31", IdRoom);

            return string.Join(",", result.Data)+"|"+ string.Join("-", futureprice.Data).Replace("[","").Replace("]", "").Trim();

            //return result.Data;
        }

    }
}
