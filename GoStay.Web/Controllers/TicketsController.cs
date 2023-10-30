using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Helper;
using GoStay.Data.Ticket;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Air;
using GoStay.DataDto.Air.DOMGetBaggages;
using GoStay.DataDto.Air.ExtDOMSearchFlights;
using GoStay.DataDto.Air.ExtDOMSearchFlightsShort;
using GoStay.DataDto.OrderDto;
using GoStay.Service.Api.Air;
using GoStay.Service.Api.Ticket;
using GoStay.Service.Api.Users;
using goStayCore.CommonCode;
using Hanssens.Net;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Uno.Extensions;

namespace GoStay.Web.Controllers
{
    public class TicketsController : Controller
    {
        private IAirApiServices _airApiServices;
        private IUserApiServices _userServices;
        private IOrdersTicketApiServices _orderTicketApiServices;
        private string TitlePage = "Vé máy bay giá rẻ";
        public string localSessionID = "";
        private readonly ISessionManager _SessionManag;
        private static SearchAirParam _param = new SearchAirParam();
        private static List<Airline> Airlines = new List<Airline>()
    {
        new Airline { Value = "VN", Text = "Vietnam Airline" },
        new Airline { Value = "QH", Text = "Bamboo Airways" },
        new Airline { Value = "VJ", Text = "VietjetAir" },
        new Airline { Value = "VU", Text = "Vietravel Airline" },
        new Airline { Value = "BL", Text = "Jestar Pacifics" },

    };
        private static List<Class> Class = new List<Class>()
        {
            new Class { Value = "1", Text = "Phổ thông tiết kiệm", TextEng = "Universal Savings", TextChi = "普遍储蓄" },
            new Class { Value = "2", Text = "Phổ thông", TextEng = "Business", TextChi = "商业" },
            new Class { Value = "3", Text = "Phổ thông đặc biệt", TextEng = "Premium economy", TextChi = "豪华经济舱" },
            new Class { Value = "4", Text = "Thương gia", TextEng = "Economy", TextChi = "经济" },

        };
        private static List<StartTime> StartTimes = new List<StartTime>()
        {
            new StartTime { Value = "0", Text = "Sáng sớm ( 0:00 - 06:00)", TextEng = "Early morning", TextChi = "早晨" },
            new StartTime { Value = "1", Text = "Buổi sáng ( 06:00 - 12:00)", TextEng = "Morning", TextChi = "早晨" },
            new StartTime { Value = "2", Text = "Buổi chiều ( 12:00 - 18:00)", TextEng = "Afternoon", TextChi = "下午" },
            new StartTime { Value = "3", Text = "Buổi tối ( 18:00 - 24:00)", TextEng = "Evening", TextChi = "晚上" },
        };
        private static List<TypeFlight> TypeFlights = new List<TypeFlight>()
        {
            new TypeFlight { Value = "0", Text = "Bay thẳng", TextEng = "Direct", TextChi = "直达航班" },
            new TypeFlight { Value = "1", Text = "Nối chuyến", TextEng = "Connecting flights", TextChi = "中转航班" },
        };
        private static List<Direction> Directions = new List<Direction>()
        {
            new Direction { Value = "0", Text = "Chiều đi", TextEng = "Direction To", TextChi = "方向" },
            new Direction { Value = "1", Text = "Chiều về", TextEng = "Direction Back", TextChi = "返回方向" },
        };
        private static Dictionary<string, string> LogoAirline = new Dictionary<string, string>()
        {
            { "VN", "assets/images/flight/vna.jpg"},
            { "QH", "assets/images/flight/bamboo.jpg"},
            { "VJ", "assets/images/flight/vietjet.jpg"},
            { "VU", "assets/images/flight/vietravel.jpg"},
            { "BL", "assets/images/flight/jestar.jpg"},
        };
        private static Dictionary<string, string> startPoints = new Dictionary<string, string>()
        {
            { "HAN", "Hà Nội (HAN)"},
            { "SGN", "TPHCM (SGN)"},
            { "DAD", "Đà Nẵng (DAD)"},
            { "VCS", "Bà Rịa Vũng Tàu (VCS)"},
            { "UIH", "Bình Định (UIH)"},
            { "CAH", "Cà Mau (CAH)"},
            { "VCA", "Cần Thơ (VCA)"},
            { "BMV", "Đắk Lăk (BMV)"},
            { "DIN", "Điện Biên (DIN)"},
            { "PXU", "Gia Lai (PXU)"},
            { "HPH", "Hải Phòng (HPH)"},
            { "HUI", "Huế (HUI)"},
            { "CXR", "Khánh Hòa (CXR)"},
            { "PQC", "Kiên Giang (PQC)"},
            { "VKG", "Kiên Giang (VKG)"},
            { "DLI", "Lâm Đồng (DLI)"},
            { "VII", "Nghệ An (VII)"},
            { "TBB", "Phú Yên (TBB)"},
            { "VDH", "Quảng Bình (VDH)"},
            { "VCL", "Quảng Nam (VCL)"},
            { "VDO", "Quảng Ninh (VDO)"},
            { "THD", "Thanh Hóa (THD)"},

        };
        private static Dictionary<string, string> endPoints = new Dictionary<string, string>()
        {
            { "SGN", "TPHCM (SGN)"},
            { "HAN", "Hà Nội (HAN)"},
            { "DAD", "Đà Nẵng (DAD)"},
            { "VCS", "Bà Rịa Vũng Tàu (VCS)"},
            { "UIH", "Bình Định (UIH)"},
            { "CAH", "Cà Mau (CAH)"},
            { "VCA", "Cần Thơ (VCA)"},
            { "BMV", "Đắk Lăk (BMV)"},
            { "DIN", "Điện Biên (DIN)"},
            { "PXU", "Gia Lai (PXU)"},
            { "HPH", "Hải Phòng (HPH)"},
            { "HUI", "Huế (HUI)"},
            { "CXR", "Khánh Hòa (CXR)"},
            { "PQC", "Kiên Giang (PQC)"},
            { "VKG", "Kiên Giang (VKG)"},
            { "DLI", "Lâm Đồng (DLI)"},
            { "VII", "Nghệ An (VII)"},
            { "TBB", "Phú Yên (TBB)"},
            { "VDH", "Quảng Bình (VDH)"},
            { "VCL", "Quảng Nam (VCL)"},
            { "VDO", "Quảng Ninh (VDO)"},
            { "THD", "Thanh Hóa (THD)"},
        };
        private static ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData> result = new ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData>();
        private static OldRequest oldRequest = new OldRequest()
        {
            StartPoint = "HAN",
            EndPoint = "SGN",
            DepartureDate = DateTime.Today.ToString("dd/MM/yyyy"),
            ReturnDate = DateTime.Today.ToString("dd/MM/yyyy"),
            Adult = 0,
            Children = 0,
            Infant = 0,

        };
        public TicketsController(IAirApiServices airApiServices, ISessionManager SessionManag, IUserApiServices userServices, IOrdersTicketApiServices orderTicketApiServices)
        {
            _airApiServices = airApiServices;
            _SessionManag = SessionManag; 
            _userServices = userServices;
            _orderTicketApiServices = orderTicketApiServices;
            AppConfigs.acvivemenu = 3;
        }
        public bool CheckRequest(string StartPoint, string EndPoint, string DepartureDate,
            string ReturnDate, int Adult, int Children, int Infant)
        {
            if (StartPoint != oldRequest.StartPoint || EndPoint != oldRequest.EndPoint || DepartureDate != oldRequest.DepartureDate
                || ReturnDate != oldRequest.ReturnDate || Adult != oldRequest.Adult || Children != oldRequest.Children
                || Infant != oldRequest.Infant
                )
            {
                oldRequest.StartPoint = StartPoint;
                oldRequest.EndPoint = EndPoint;
                oldRequest.DepartureDate = DepartureDate;
                oldRequest.ReturnDate = ReturnDate;
                oldRequest.Adult = Adult;
                oldRequest.Children = Children;
                oldRequest.Infant = Infant;
                return false;
            }

            return true;
        }
        public IActionResult Index(string? StartPoint = "HAN", string? EndPoint = "SGN",
            string? DepartureDate = null, string? ReturnDate = null, int? Adult = 1, int? Children = 0, int? Infant = 0, string InputTicketSearch = "")
        {
            ViewData["TitlePage"] = TitlePage + " các hãng: Vietnam Airline, Bamboo Airways, VietjetAir, Vietravel Airline, Jestar Pacifics";
            ViewData["descriptionPage"] = "Vé máy bay giá rẻ, vé máy bay trực tuyến, thông tin cập nhật vé máy may các hãng liên tục 247";

            ViewBag.startPoints = startPoints;
            ViewBag.endPoints = endPoints;
            _param.PriceMin = 0;
            _param.PriceMax = 5000000;
            DateTime departureDate =  DateTime.Now.AddDays(2);
            DateTime returnDate = DateTime.Now.AddDays(3);


            if (DepartureDate != null)
            {
                departureDate = DateTime.ParseExact(DepartureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                AppConfigs.DepartureDate = departureDate.ToString("dd/MM/yyyy");
            }
            AppConfigs.DepartureDate = departureDate.ToString("dd/MM/yyyy");

            if (ReturnDate != null)
            {
                returnDate = DateTime.ParseExact(ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                AppConfigs.ReturnDate = returnDate.ToString("dd/MM/yyyy");
            }
            AppConfigs.ReturnDate = returnDate.ToString("dd/MM/yyyy");

            _param.Direction = Directions.Select(s => new System.Web.Mvc.SelectListItem
            {
                Value = s.Value,
                Text = s.TextLang,
                Selected = false
            }).ToList();
            _param.Airlines = Airlines.Select(s => new System.Web.Mvc.SelectListItem
            {
                Value = s.Value,
                Text = s.Text,
                Selected = false
            }).ToList();
            _param.StartTime = StartTimes.Select(s => new System.Web.Mvc.SelectListItem
            {
                Value = s.Value,
                Text = s.TextLang,
                Selected = false
            }).ToList();
            _param.TypeFlight = TypeFlights.Select(s => new System.Web.Mvc.SelectListItem
            {
                Value = s.Value,
                Text = s.TextLang,
                Selected = false
            }).ToList();
            _param.ClassTk = Class.Select(s => new System.Web.Mvc.SelectListItem
            {
                Value = s.Value,
                Text = s.TextLang,
                Selected = false
            }).ToList();
            var data = new AirSearchInOut() { };
            data.searchAirParam = _param;
            data.StartPoint = StartPoint;
            data.EndPoint = EndPoint;
            data.DepartureDate = departureDate.ToString("dd/MM/yyyy");
            data.ReturnDate = returnDate.ToString("dd/MM/yyyy");
            data.Adult = (int)Adult;
            data.Children = (int)Children;
            data.Infant = (int)Infant;
            AppConfigs.TextTicketSearchBar = InputTicketSearch;
            AppConfigs.Adult = (int)Adult;
            AppConfigs.Children = (int)Children;
            AppConfigs.Infant = (int)Infant;

            return View(data);
        }
        public IActionResult Detail(string dataSession, string departureFlightSession, string returnFlightSession)
        {
            RequestData<BaggageParams> request = new RequestData<BaggageParams>()
            {
                Username = "SGO",
                Password = "Sgo@#123@#API",
                Data = new BaggageParams()
                {
                    dataSession = dataSession,
                    departureFlightSession = departureFlightSession,
                    returnFlightSession = returnFlightSession,

                }
            };

            var result2 = _airApiServices.DOMGetBaggages(request);
            AppConfigs.acvivemenu = 3;
            return View();
        }
        public IActionResult GetTicketHomePage(int IsHomepage)
        {
            try
            {
                if (IsHomepage == 10)
                {
                    var DepartureDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
                    var ReturnDate = DateTime.Now.AddDays(8).ToString("dd/MM/yyyy");

                    var request1 = new RequestData<FlightParams>()
                    {
                        Username = "SGO",
                        Password = "Sgo@#123@#API",
                        Data = new FlightParams()
                        {
                            ItineraryType = 1,
                            StartPoint = "HAN",
                            EndPoint = "SGN",
                            DepartureDate = DepartureDate,
                            ReturnDate = ReturnDate,
                            Adult = 1,
                            Children = 0,
                            Infant = 0,
                            ClientVia = "WEB",
                            FlightFilter = new FlightFilter() { FilterType = 0 }
                        }
                    };

                    result = _airApiServices.ExtDOMSearchFlights(request1);

                    result.Data.Flights = new Dictionary<string, DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo>();
                    ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData> resulttemp = new ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData>
                    {
                        Code = result.Code,
                        Message = result.Message,
                        Data = result.Data.Clone()
                    };
                    resulttemp.Data.Flights = resulttemp.Data.DepartureFlights;

                    var data = resulttemp.Data.Flights.Values.OrderBy(x => x.Price).Take(8).ToList();
                    foreach (var item in data)
                    {
                        item.AirlineName = Airlines.Where(x => x.Value == item.AirlineCode).SingleOrDefault().Text;
                        item.LogoAirline = LogoAirline.Where(x => x.Key == item.AirlineCode).SingleOrDefault().Value;
                        item.StartPoint = startPoints.Where(x => x.Key == item.StartPoint).SingleOrDefault().Value;
                        item.EndPoint = startPoints.Where(x => x.Key == item.EndPoint).SingleOrDefault().Value;
                    }
                    TicketHomePage model = new TicketHomePage()
                    {
                        DataSession = result.Data.DataSession,
                        data = data,
                        UserId = _SessionManag.GetGostayUserFromSession().UserId
                    };
                    return View(model);
                }
                else
                {
                    return View(null);
                }    
            }
            catch(Exception ex)
            {
                return View(ex.ToString());
            }
        }


        public IActionResult TicketOrder(string DataSession, string FlySession, string Direction, string Classtk,
                                        string TkDate, int adult, int child, int infant)
        {
            try
            {
                DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo flight = new DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo();
                if (FlySession != null)
                {

                    flight = result.Data.DepartureFlights.Values.Where(x => x.FlightSession == FlySession).SingleOrDefault();
                    if (flight == null)
                    {
                        flight = result.Data.ReturnFlights.Values.Where(x => x.FlightSession == FlySession).SingleOrDefault();
                    }
                }
                var startpoint = startPoints.Where(x => x.Key == flight.StartPoint).SingleOrDefault().Value;
                var endpoint = startPoints.Where(x => x.Key == flight.EndPoint).SingleOrDefault().Value;
                var airlinename = Airlines.Where(x => x.Value == flight.AirlineCode).SingleOrDefault().Text;
                var logoAirline = LogoAirline.Where(x => x.Key == flight.AirlineCode).SingleOrDefault().Value;
                var fareoption = flight.FareOptions.Where(x => x.Class == Classtk).SingleOrDefault();

                var IdUser = _SessionManag.GetGostayUserFromSession().UserId;
                var user = _userServices.CheckUserByID(IdUser);
                TicketOrder data = new TicketOrder()
                {
                    UserInfo = new UserInfoDto()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        MobileNo = user.MobileNo,
                        Address = user.Address,
                        UserId = IdUser,
                    },
                    DataSession = DataSession,
                    Flights = flight,
                    Adult = adult,
                    Child = child,
                    Infant = infant,
                    StartPoint = startpoint,
                    EndPoint = endpoint,
                    FareOption = fareoption
                };
                data.Flights.AirlineName = airlinename;
                data.Flights.LogoAirline = logoAirline;
                return View(data);
            }
            catch(Exception e)
            {
                return View(" Bạn cần tìm kiếm lại vé, Lỗi: " + e.Message);

            }
        }
        public IActionResult CreateOrderTicket(string Adult, string Child, string Infant, string Baggare, string DataSession,
            string FlightSession, string ClassTk, string Phone, string Email, string Name, string Address)

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

            UserGostay user = _SessionManag.GetUserGostay();
            DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo flight = new DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo();
            if (FlightSession != null)
            {

                flight = result.Data.DepartureFlights.Values.Where(x => x.FlightSession == FlightSession).SingleOrDefault();
                if (flight == null)
                {
                    flight = result.Data.ReturnFlights.Values.Where(x => x.FlightSession == FlightSession).SingleOrDefault();
                }
            }
            flight.AirlineName = Airlines.Where(x => x.Value == flight.AirlineCode).SingleOrDefault().Text;
            flight.LogoAirline = LogoAirline.Where(x => x.Key == flight.AirlineCode).SingleOrDefault().Value;

            var startpoint = startPoints.Where(x => x.Key == flight.StartPoint).SingleOrDefault().Value;
            var endpoint = startPoints.Where(x => x.Key == flight.EndPoint).SingleOrDefault().Value;
            var airlinename = Airlines.Where(x => x.Value == flight.AirlineCode).SingleOrDefault().Text;
            var fareoption = flight.FareOptions.Where(x => x.Class == ClassTk).SingleOrDefault();

            var listAdult = Adult.Split(",");
            string[] listChild = null;
            string[] listInfant = null;

            List<TicketPassengerDto> passengers = new List<TicketPassengerDto>();

            for (int a = 0; a < listAdult.Count(); a++)
            {
                var passenger = new TicketPassengerDto();

                var infoAdult = listAdult[a].Split("-");
                var gen = infoAdult[1];
                bool gender = true;
                if (gen == "0")
                {
                    gender = false;
                }

                passenger = new TicketPassengerDto()
                {
                    FullName = infoAdult[0],
                    Gender = gender,
                    Birthday = infoAdult[2],
                    Type = "Adult",
                    IdTicket = 0,
                    Price = (decimal)(fareoption.PriceAdult + fareoption.FeeAdult + fareoption.TaxAdult),
                };
                passengers.Add(passenger);
            }

            if (!Child.IsNullOrEmpty())
            {
                listChild = Child.Split(",");

                foreach (var child in listChild)
                {
                    if (child != "-1-")
                    {
                        var infoChild = child.Split("-");
                        var gen = infoChild[1];
                        bool gender = true;
                        if (gen == "0")
                        {
                            gender = false;
                        }
                        var passenger = new TicketPassengerDto()
                        {
                            FullName = infoChild[0],
                            Gender = gender,
                            Birthday = infoChild[2],
                            Type = "Child",
                            IdTicket = 0,
                            Price = (decimal)(fareoption.PriceChild + fareoption.FeeChild + fareoption.TaxChild),
                        };
                        passengers.Add(passenger);
                    }
                }
            }


            if (!Infant.IsNullOrEmpty())
            {
                listInfant = Infant.Split(",");

                foreach (var infant in listInfant)
                {
                    if (infant != "-1-")
                    {
                        var infoInfant = infant.Split("-");
                        var gen = infoInfant[1];
                        bool gender = true;
                        if (gen == "0")
                        {
                            gender = false;
                        }
                        var passenger = new TicketPassengerDto()
                        {
                            FullName = infoInfant[0],
                            Gender = gender,
                            Birthday = infoInfant[2],
                            Type = "Infant",
                            IdTicket = 0,
                            Price = (decimal)(fareoption.PriceInfant + fareoption.FeeInfant + fareoption.TaxInfant),

                        };
                        passengers.Add(passenger);
                    }
                }
            }

            CreateOrderTicketParam param = new CreateOrderTicketParam()
            {
                order = new OrderTicketDto()
                {
                    DataFlightSession = DataSession,
                    FlightSession = flight.FlightSession,
                    IdPtthanhToan = 4,
                    Status = 1,
                    Title = $"Order Chuyến {flight.FlightNumber}: {flight.StartPoint} Lúc {flight.StartDate} Đến {flight.EndPoint} Lúc {flight.EndDate}",
                    IdUser = _SessionManag.GetGostayUserFromSession().UserId,
                    Session = cookie,
                    Ordercode = DateTime.UtcNow.TimeOfDay.TotalSeconds.ToString(),
                    ContactInfor = Name,
                    Phone = Phone,
                    Email = Email,
                    Address = Address,
                },
                orderDetail = new OrderTicketDetailDto()
                {
                    IdOrder = 0,
                    Price = 0,
                    Discount = 0,
                    StartPoint = flight.StartPoint,
                    EndPoint = flight.EndPoint,
                    DepartureDateText = flight.StartDate.ToString("dd/MM/yyy"),
                    StartDateText = flight.StartDate.ToString("dd/MM/yyy HH:mm"),
                    EndDateText = flight.EndDate.ToString("dd/MM/yyy HH:mm"),
                    AirlineCode = flight.AirlineCode,
                    AirlineName = flight.AirlineName,
                    FlightNumber = flight.FlightNumber,
                    Duration = flight.Duration,
                    Barrage = Baggare,
                    Class = ClassTk,
                    ServiceFee = flight.FareOptions.FirstOrDefault().ServiceFee,
                    IssueFee = flight.FareOptions.FirstOrDefault().IssueFee,
                    Passengers = passengers
                }
            };

            RequestData<DataDto.Air.DOMMakeReservation.ReservationParams> request = new RequestData<DataDto.Air.DOMMakeReservation.ReservationParams>()
            {
                Username = "SGO",
                Password = "Sgo@#123@#API",
                Data = new DataDto.Air.DOMMakeReservation.ReservationParams()
            };
            request.Data.DataSession = DataSession;
            request.Data.DepartureFlightSession = flight.FlightSession;
            request.Data.ReturnFlightSession = string.Empty;
            request.Data.ListPassengers = new List<DataDto.Air.DOMMakeReservation.PassengerInfo>();
            foreach (var passenger in passengers)
            {
                var typepassenger = "";
                if (passenger.Type == "Adult")
                {
                    typepassenger = "ADT";
                }
                if (passenger.Type == "Child")
                {
                    typepassenger = "CHD";
                }
                if (passenger.Type == "Infant")
                {
                    typepassenger = "INF";
                }
                byte gen = 1;
                if (passenger.Gender == false)
                    gen = 0;
                request.Data.ListPassengers.Add(new DataDto.Air.DOMMakeReservation.PassengerInfo()
                {
                    FirstName = passenger.FullName,
                    LastName = "",
                    Type = typepassenger,
                    Gender = gen,
                    Email = "",
                    Phone = "",
                    Birthday = passenger.Birthday,
                    PassportExpiryDate = "",
                    PassportIssueCountry = "",
                    PassportNumber = "",
                    BaggageDeparture = "",
                    BaggageReturn = "",
                });
            }

            request.Data.contactInfo = new DataDto.Air.DOMMakeReservation.ContactInfo()
            {
                Gender = 1,
                FirstName = Name,
                LastName = "",
                Phone = Phone,
                Email = Email,
                Address = Address,
                Company = "",
                Note = ""
            };
            request.Data.ClientVia = "WEB";
            ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> Result = new ResultData<DataDto.Air.DOMMakeReservation.ReservationCode>();
            Result = _airApiServices.DOMMakeReservation(request);
            if (Result != null)
            {
                param.order.ReservationFlightCode = Result.Data.DepartureCode;
                param.order.ReservationTransactionCode = Result.Data.TransactionCode;
            }
            var orderinfo = _orderTicketApiServices.CreateOrderTicket(param);
            orderinfo.Data.TicketDetail.Logo = LogoAirline.Where(x => x.Key == orderinfo.Data.TicketDetail.AirlineCode).SingleOrDefault().Value;

            OrderTicketShowDto data = orderinfo.Data;
            return View(data);
        }

        public IActionResult showTicket(int Id)
        {
            OrderTicketShowDto data = _orderTicketApiServices.GetOrderTicketbyId(Id).Data;
            var logoAirline = LogoAirline.Where(x => x.Key == data.TicketDetail.AirlineCode).SingleOrDefault().Value;
            data.TicketDetail.Logo = logoAirline;
            return PartialView("~/Views/Tickets/CreateOrderTicket.cshtml", data);
        }



        public IActionResult ketqua(string? StartPoint = "HAN", string? EndPoint = "SGN", string? DepartureDate = null,
            string? ReturnDate = null, int? Adult = 1, int? Children = 0, int? Infant = 0, string? ClientVia = "Web", int? FilterType = 0,
            int? PriceMin = 0, int? PriceMax = 0, string[]? Airline = null, string[]? StartTime = null, string[]? ClassTk = null,
            string[]? TypeFlight = null, string[]? Direction = null, int PageIndex = 0, int PageSize = 0, string InputTicketSearch = ""
            )
        {
            try
            {
                var checkrequest = CheckRequest(StartPoint, EndPoint, DepartureDate, ReturnDate, (int)Adult, (int)Children, (int)Infant);
                if (DepartureDate == null )
                {
                    DepartureDate = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
                }
                if (ReturnDate == null)
                {
                    ReturnDate = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");
                }
                if (Adult == 0) Adult = 1;
                AppConfigs.TextTicketSearchBar = InputTicketSearch;


                Direction = Direction.CheckParamUrl();
                Airline = Airline.CheckParamUrl();
                ClassTk = ClassTk.CheckParamUrl();
                StartTime = StartTime.CheckParamUrl();
                TypeFlight = TypeFlight.CheckParamUrl();
                bool isSearch = true;
                if (StartPoint == "HAN" && EndPoint == "SGN" && DepartureDate == DateTime.Today.AddDays(2).ToString("dd/MM/yyyy") 
                    && ReturnDate == DateTime.Today.AddDays(3).ToString("dd/MM/yyyy") && Adult == 1 && Children == 0 &&
                    Infant == 0 && ClientVia == "Web" && FilterType == 0 && PriceMin == 0 && PriceMax == 5000000 && Airline.IsNullOrEmpty() == true &&
                    StartTime.IsNullOrEmpty() == true && ClassTk.IsNullOrEmpty() == true && TypeFlight.IsNullOrEmpty() == true)
                {
                    isSearch = false;
                }
                if (isSearch == false)
                {

                    //DepartureDate = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
                    //ReturnDate = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");

                    _param.PriceMin = PriceMin;
                    _param.PriceMax = PriceMax;

                    _param.Direction = Directions.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();

                    _param.Airlines = Airlines.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.StartTime = StartTimes.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.TypeFlight = TypeFlights.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.ClassTk = Class.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();

                    var strDepartureDate = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
                    string strReturnDate = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");

                    var request = new RequestData<FlightParams>()
                    {
                        Username = "SGO",
                        Password = "Sgo@#123@#API",
                        Data = new FlightParams()
                        {
                            ItineraryType = 2,
                            StartPoint = StartPoint,
                            EndPoint = EndPoint,
                            DepartureDate = strDepartureDate,
                            ReturnDate = strReturnDate,
                            Adult = 1,
                            Children = 1,
                            Infant = 1,
                            ClientVia = ClientVia,
                            FlightFilter = new FlightFilter() { FilterType = (int)FilterType }
                        }
                    };
                    if (PageIndex == 0 && Direction == null && checkrequest != true)
                    {
                        result = _airApiServices.ExtDOMSearchFlights(request);

                        result.Data.Flights = new Dictionary<string, DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo>();

                    }
                    ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData> resulttemp = new ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData>
                    {
                        Code = result.Code,
                        Message = result.Message,
                        Data = result.Data.Clone()
                    };

                    if (Direction != null)
                    {
                        foreach (System.Web.Mvc.SelectListItem item in _param.Direction)
                        {
                            if (Direction.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        string direction = String.Join(",", Direction);
                        if (direction == "0")
                        {
                            resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.DepartureFlights);
                        }
                        else
                        {
                            if (direction == "1")
                            {
                                resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.ReturnFlights);
                            }
                            else
                            {
                                resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.DepartureFlights);
                                foreach (var item in resulttemp.Data.ReturnFlights)
                                {
                                    resulttemp.Data.Flights.Add(item.Key, item.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.DepartureFlights);
                        foreach (var item in resulttemp.Data.ReturnFlights)
                        {
                            resulttemp.Data.Flights.Add(item.Key, item.Value);
                        }
                    }

                    var data = new AirSearchInOut()
                    {
                        ItineraryType = request.Data.ItineraryType,
                        StartPoint = StartPoint,
                        EndPoint = EndPoint,
                        DepartureDate = request.Data.DepartureDate,
                        ReturnDate = request.Data.ReturnDate,
                        Adult = 1,
                        Children = 0,
                        Infant = 0,
                        FlightFilter = request.Data.FlightFilter.FilterType,
                        searchAirParam = _param,
                        PageIndex = PageIndex,
                        PageSize = PageSize,
                        Total = resulttemp.Data.Flights.Count(),
                        Present = (PageIndex + 1) * PageSize,
                        Data = resulttemp.Data,
                    };
                    foreach (var item in data.Data.Flights.Values)
                    {
                        item.AirlineName = Airlines.Where(x => x.Value == item.AirlineCode).SingleOrDefault().Text;
                        item.LogoAirline = LogoAirline.Where(x => x.Key == item.AirlineCode).SingleOrDefault().Value;
                        if (item.StartPoint == StartPoint)
                        {
                            item.Direction = 1;//đi
                        }
                        if (item.StartPoint == EndPoint)
                        {
                            item.Direction = 2;//về
                        }
                    }
                    if (data.Total < data.Present)
                    {
                        data.Present = data.Total;
                    }
                    data.UserId = _SessionManag.GetGostayUserFromSession().UserId;
                    AppConfigs.Adult = (int)Adult;
                    AppConfigs.Children = (int)Children;
                    AppConfigs.Infant = (int)Infant;
                    return View(data);
                }
                else
                {
                    AppConfigs.TextTicketSearchBar = InputTicketSearch;
                    var departureDate = DateTime.ParseExact(DepartureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var returnDate = DateTime.ParseExact(ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                    if (departureDate < DateTime.Today.AddDays(2) || DepartureDate == null)
                    {
                        DepartureDate = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
                    }
                    AppConfigs.DepartureDate = DepartureDate;

                    if (returnDate < DateTime.Today.AddDays(3) || ReturnDate == null)
                    {
                        ReturnDate = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");
                    }
                    AppConfigs.ReturnDate = ReturnDate;


                    _param.PriceMin = PriceMin;
                    _param.PriceMax = PriceMax;

                    Direction = Direction.CheckParamUrl();
                    Airline = Airline.CheckParamUrl();
                    ClassTk = ClassTk.CheckParamUrl();
                    StartTime = StartTime.CheckParamUrl();
                    TypeFlight = TypeFlight.CheckParamUrl();

                    _param.Direction = Directions.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.Airlines = Airlines.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.ClassTk = Class.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.StartTime = StartTimes.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();
                    _param.TypeFlight = TypeFlights.Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,
                        Selected = false
                    }).ToList();

                    var request1 = new RequestData<FlightParams>()
                    {
                        Username = "SGO",
                        Password = "Sgo@#123@#API",
                        Data = new FlightParams()
                        {
                            ItineraryType = 2,
                            StartPoint = StartPoint,
                            EndPoint = EndPoint,
                            DepartureDate = DepartureDate,
                            ReturnDate = ReturnDate,
                            Adult = (int)Adult,
                            Children = (int)Children,
                            Infant = (int)Infant,
                            ClientVia = ClientVia,
                            FlightFilter = new FlightFilter() { FilterType = (int)FilterType }
                        }
                    };
                    if (checkrequest != true)
                    {
                        result = _airApiServices.ExtDOMSearchFlights(request1);

                    }
                    ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData> resulttemp = new ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData>();

                    if (result.Data != null)
                    {
                        result.Data.Flights = new Dictionary<string, DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightInfo>();

                        resulttemp = new ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData>
                        {
                            Code = result.Code,
                            Message = result.Message,
                            Data = result.Data.Clone()
                        };
                    }
                    SearchAirRequest request2 = new SearchAirRequest();

                    if (Direction != null)
                    {
                        foreach (System.Web.Mvc.SelectListItem item in _param.Direction)
                        {
                            if (Direction.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        string direction = String.Join(",", Direction);
                        if (direction == "0")
                        {
                            if(resulttemp.Data!=null)
                            resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.DepartureFlights);

                        }
                        else
                        {
                            if (direction == "1")
                            {
                                if (resulttemp.Data != null)

                                    resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.ReturnFlights);
                            }
                            else
                            {
                                if (resulttemp.Data != null)
                                {
                                    resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.DepartureFlights);
                                    foreach (var item in resulttemp.Data.ReturnFlights)
                                    {
                                        resulttemp.Data.Flights.Add(item.Key, item.Value);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (resulttemp.Data != null)
                        {
                            resulttemp.Data.Flights = resulttemp.Data.CloneDic(resulttemp.Data.DepartureFlights);
                            foreach (var item in resulttemp.Data.ReturnFlights)
                            {
                                resulttemp.Data.Flights.Add(item.Key, item.Value);
                            }
                        }
                    }

                    if (PriceMin != 0)
                    {
                        request2.PriceMin = PriceMin;
                        foreach (var item in resulttemp.Data.Flights)
                        {
                            if (item.Value.Price < request2.PriceMin)
                                resulttemp.Data.Flights.Remove(item.Key);
                        }
                    }
                    if (PriceMax != 0)
                    {
                        request2.PriceMax = PriceMax;
                        if (resulttemp.Data != null)
                        {
                            foreach (var item in resulttemp.Data.Flights)
                            {
                                if (item.Value.Price > request2.PriceMax)
                                    resulttemp.Data.Flights.Remove(item.Key);
                            }
                        }
                    }

                    if (Airline != null)
                    {
                        foreach (System.Web.Mvc.SelectListItem item in _param.Airlines)
                        {
                            if (Airline.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        request2.Airlines = Airline;
                        string airlines = String.Join(",", request2.Airlines);
                        foreach (var item in resulttemp.Data.Flights)
                        {
                            if (airlines.Contains(item.Value.AirlineCode) == false)
                                resulttemp.Data.Flights.Remove(item.Key);
                        }
                    }


                    if (StartTime != null)
                    {
                        foreach (System.Web.Mvc.SelectListItem item in _param.StartTime)
                        {
                            if (StartTime.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        request2.StartTime = StartTime;
                        string starttime = String.Join(",", StartTime);

                        foreach (var item in resulttemp.Data.Flights)
                        {
                            if (starttime.Contains(item.Value.StartTime) == false)
                                resulttemp.Data.Flights.Remove(item.Key);
                        }
                    }

                    if (ClassTk != null)
                    {

                        foreach (System.Web.Mvc.SelectListItem item in _param.ClassTk)
                        {
                            if (ClassTk.Any(x => x.Trim().Equals(item.Value.Trim())))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        request2.ClassTk = ClassTk;
                    }

                    if (TypeFlight != null)
                    {
                        foreach (System.Web.Mvc.SelectListItem item in _param.TypeFlight)
                        {
                            if (TypeFlight.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        request2.TypeFlight = TypeFlight;
                    }

                    var data = new AirSearchInOut()
                    {
                        ItineraryType = request1.Data.ItineraryType,
                        StartPoint = request1.Data.StartPoint,
                        EndPoint = request1.Data.EndPoint,
                        DepartureDate = request1.Data.DepartureDate,
                        ReturnDate = request1.Data.ReturnDate,
                        Adult = request1.Data.Adult,
                        Children = request1.Data.Children,
                        Infant = request1.Data.Infant,
                        FlightFilter = request1.Data.FlightFilter.FilterType,
                        searchAirParam = _param,
                        PageIndex = PageIndex,
                        PageSize = PageSize,
                        Total = resulttemp.Data.Flights.Count(),
                        Present = (PageIndex + 1) * PageSize,
                        Data = resulttemp.Data,
                    };
                    AppConfigs.Adult = (int)Adult;
                    AppConfigs.Children = (int)Children;
                    AppConfigs.Infant = (int)Infant;

                    foreach (var item in data.Data.Flights.Values)
                    {
                        item.AirlineName = Airlines.Where(x => x.Value == item.AirlineCode).SingleOrDefault().Text;
                        item.LogoAirline = LogoAirline.Where(x => x.Key == item.AirlineCode).SingleOrDefault().Value;

                        if (item.StartPoint == StartPoint)
                        {
                            item.Direction = 1;//đi
                        }
                        if (item.StartPoint == EndPoint)
                        {
                            item.Direction = 2;//về
                        }
                    }
                    data.UserId = _SessionManag.GetGostayUserFromSession().UserId;

                    if (data.Total < data.Present)
                    {
                        data.Present = data.Total;
                    }
                    return View(data);
                }
            }
            catch(Exception ex)
            {
                return View(ex.ToString());
            }
        }

    }
}
