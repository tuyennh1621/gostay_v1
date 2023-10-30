using Microsoft.AspNetCore.Mvc;
using GoStay.Service;
using goStayCore.CommonCode;
using AutoMapper;
using GoStay.Service.Api.Hotels;
using GoStay.DataDto.Statistical;
using GoStay.DataDto.OrderDto;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminOrdersController : Controller
    {
        private IOrdersApiServices _ordersApiServices;
        private readonly IMyTypedClientServices _client;
        private readonly ISessionManager _SessionManag;
        private readonly IMapper _mapper;

        public AdminOrdersController(IOrdersApiServices _OrdersApiServices, IMyTypedClientServices client, IMapper mapper,
            ISessionManager sessionManag)
        {
            _ordersApiServices = _OrdersApiServices;
            _client = client;
            _SessionManag = sessionManag;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chart(int? year, int? month, int? status)
        {
            OrderChartDto _OrderChartDto = new OrderChartDto();
            var now = DateTime.Now;
            _OrderChartDto.GetOrderByMonth = _ordersApiServices.GetOrderByMonth(month?? now.Month, year??now.Year, status??3);
            _OrderChartDto.GetOrderTotalMoneyByMonth = _ordersApiServices.GetOrderTotalMoneyByMonth(month ?? now.Month, year ?? now.Year, status??3);
            _OrderChartDto.GetOrderRoomByMonth = _ordersApiServices.GetOrderRoomByMonth(month ?? now.Month, year ?? now.Year, status??3);
            //  var orderRoom = _ordersApiServices.GetOrderRoomByMonth(1, 2023, 1);
            return View(_OrderChartDto);
        }
        public IActionResult ChartPartial(int? year, int? month, int? status)
        {
            OrderChartDto _OrderChartDto = new OrderChartDto();
            var now = DateTime.Now;
            _OrderChartDto.GetOrderByMonth = _ordersApiServices.GetOrderByMonth(month ?? now.Month, year ?? now.Year, status ?? 3);
            _OrderChartDto.GetOrderTotalMoneyByMonth = _ordersApiServices.GetOrderTotalMoneyByMonth(month ?? now.Month, year ?? now.Year, status ?? 3);
            _OrderChartDto.GetOrderRoomByMonth = _ordersApiServices.GetOrderRoomByMonth(month ?? now.Month, year ?? now.Year, status ?? 3);
            //  var orderRoom = _ordersApiServices.GetOrderRoomByMonth(1, 2023, 1);
            return PartialView("~/Areas/Admin/Views/AdminOrders/ChartPartial.cshtml", _OrderChartDto);
        }

    }
}
