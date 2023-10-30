using AutoMapper;
using GoStay.Data.Ticket;
using GoStay.Service.Api.Ticket;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Mvc;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TicketHomeController : Controller
    {
        private readonly IOrdersTicketApiServices _orderTicketServices;
        private readonly ISessionManager _SessionManag;
        private readonly IMapper _mapper;

        public TicketHomeController(IOrdersTicketApiServices orderTicketServices, IMapper mapper, ISessionManager sessionManag)
        {
            _orderTicketServices = orderTicketServices;
            _SessionManag = sessionManag;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult OrderTicketList()
        {

            try
            {
                var userid = _SessionManag.GetGostayUserFromSession().UserId;
                var result = _orderTicketServices.GetAllOrderTicket(null, 1, 25);
                DataTicketAdminDto data = new DataTicketAdminDto()
                {
                    Total = result.Data.FirstOrDefault().TotalCount,
                    TotalPage = result.Data.FirstOrDefault().TotalPage,
                    PageIndex = 1,
                    PageSize = 25,
                    Tickets = result.Data
                };
                //if (response.Data == null)
                //{
                //    response.Code = ErrorCodeMessage.ListNull.Key;
                //    response.Message = ErrorCodeMessage.ListNull.Value;
                //}
                return View(data);

            }
            catch
            {
                //response.Code = ErrorCodeMessage.InternalExeption.Key;
                //response.Message = ErrorCodeMessage.InternalExeption.Value;
                return View(null);
            }

        }

        public IActionResult TicketListPartial(int pageIndex, int pageSize)
        {
            try
            {
                var userid = _SessionManag.GetGostayUserFromSession().UserId;
                var result = _orderTicketServices.GetAllOrderTicket(null, pageIndex, pageSize);
                DataTicketAdminDto data = new DataTicketAdminDto()
                {
                    Total = result.Data.FirstOrDefault().TotalCount,
                    TotalPage = result.Data.FirstOrDefault().TotalPage,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Tickets = result.Data
                };
                //response.Data = result.Data;
                //if (response.Data == null)
                //{
                //    response.Code = ErrorCodeMessage.ListNull.Key;
                //    response.Message = ErrorCodeMessage.ListNull.Value;
                //}
                return View("~/Areas/Admin/Views/TicketHome/TicketListPartial.cshtml", data);

            }
            catch
            {
                //response.Code = ErrorCodeMessage.InternalExeption.Key;
                //response.Message = ErrorCodeMessage.InternalExeption.Value;
                return View("~/Areas/Admin/Views/TicketHome/TicketListPartial.cshtml", null);
            }

        }
    }
}
