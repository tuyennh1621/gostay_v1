using DAL.Helpers.PageHelpers;
using GoStay.DataDto.OrderDto;
using GoStay.Service;
using GoStay.Service.Api.Hotels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class OrderHomeController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IOrdersApiServices _ordersApiServices;


        public OrderHomeController(IOrderServices OrderServices, IOrdersApiServices ordersApiServices)
        {
            _orderServices = OrderServices;
            _ordersApiServices = ordersApiServices;
        }

        [HttpGet]
        public IActionResult OrderList(string orderSearch)
        {
            OrderSearchParam orderSearchParam = new OrderSearchParam()
            {
                UserId = null,
                StartDate = null,
                EndDate = null,
                OrderCode = null,
                Email = null,
                Phone = null,
                Status = null,
                Style = null,
                PageSize = 25,
                PageIndex = 1
            };
            if (orderSearch != null)
            {
                var ordersearch = JsonConvert.DeserializeObject<OrderSearchDto>(orderSearch);

                if (ordersearch.UserId != "")
                {
                    orderSearchParam.UserId = int.Parse(ordersearch.UserId);
                }
                orderSearchParam.OrderCode = ordersearch.OrderCode;
                if (ordersearch.StartDate != "")
                {
                    ordersearch.StartDate = ordersearch.StartDate.Trim();
                    orderSearchParam.StartDate = DateTime.ParseExact(ordersearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                }
                if (ordersearch.EndDate != "")
                {
                    ordersearch.EndDate = ordersearch.EndDate.Trim();
                    orderSearchParam.EndDate = DateTime.ParseExact(ordersearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
            }
            try
            {

                var data = _ordersApiServices.GetListOrderSearch(orderSearchParam);
                var model = new ViewModel<OrderSearchOutDto>();
                model.Data = data;
                model.PageSize = orderSearchParam.PageSize;
                model.PageIndex = orderSearchParam.PageIndex;
                return View(model);
            }
            catch
            {
                return View(new ViewModel<OrderSearchOutDto>());

            }
        }
        [HttpGet]
        public IActionResult OrderListPartial(string orderSearch)
        {
            var ordersearch = JsonConvert.DeserializeObject<OrderSearchDto>(orderSearch);
            OrderSearchParam orderSearchParam = new OrderSearchParam();
            if (ordersearch.UserId != "")
            {
                orderSearchParam.UserId = int.Parse(ordersearch.UserId);
            }
            orderSearchParam.OrderCode = ordersearch.OrderCode;
            orderSearchParam.Email = ordersearch.Email;
            orderSearchParam.Phone = ordersearch.Phone;
            orderSearchParam.PageIndex = ordersearch.PageIndex;
            orderSearchParam.PageSize = ordersearch.PageSize;


            if (ordersearch.Status != "")
            {
                orderSearchParam.Status = int.Parse(ordersearch.Status);
            }
            if (ordersearch.Style != "")
            {
                orderSearchParam.Style = int.Parse(ordersearch.Style);
            }

            if (ordersearch.StartDate != "")
            {
                ordersearch.StartDate = ordersearch.StartDate.Trim();
                orderSearchParam.StartDate = DateTime.ParseExact(ordersearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            }
            if (ordersearch.EndDate != "")
            {
                ordersearch.EndDate = ordersearch.EndDate.Trim();
                orderSearchParam.EndDate = DateTime.ParseExact(ordersearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            var model = new ViewModel<OrderSearchOutDto>();

            try
            {

                var data = _ordersApiServices.GetListOrderSearch(orderSearchParam);
                model.Data = data;
                model.PageSize = orderSearchParam.PageSize;
                model.PageIndex = orderSearchParam.PageIndex;
                return PartialView("~/Areas/Admin/Views/OrderHome/OrderListPartial.cshtml", model);
            }
            catch
            {
                return PartialView("~/Areas/Admin/Views/OrderHome/OrderListPartial.cshtml", model);
            }
        }
        [HttpGet]
        public IActionResult GetUserInfo(int IdUser)
        {
            var model = _orderServices.GetUserInfo(IdUser);
            return PartialView("~/Areas/Admin/Views/OrderHome/PopupUser.cshtml", model);
        }
    }
}
