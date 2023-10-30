using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesHomeController : Controller
    {

        private readonly IServicesServices _ServicesServices;
        private readonly IHotelMamenitiServices _hotelMamenitiServices;
        private readonly IRoomMamenitiServices _roomMamenitiServices;
        private readonly IHotelServices _hotelServices;
        private readonly IHotelRoomServices _roomServices;


        public ServicesHomeController( IServicesServices ServicesServices, IHotelServices hotelServices, IHotelRoomServices roomServices
            , IRoomMamenitiServices roomMamenitiServices, IHotelMamenitiServices hotelMamenitiServices)
        {
            _ServicesServices = ServicesServices;
            _roomMamenitiServices = roomMamenitiServices;
            _hotelMamenitiServices = hotelMamenitiServices;
            _hotelServices = hotelServices;
            _roomServices = roomServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ServicesList(FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            ResponseBase<PagingList<DataAccess.Entities.Service>> response = new ResponseBase<PagingList<DataAccess.Entities.Service>>();
            try
            {
                ViewBag.listMameniti = _hotelMamenitiServices.GetAllHotelMameniti();
                response.Data = _ServicesServices.GetServicesList(filter);
                if (response.Data == null)
                {
                    response.Code = ErrorCodeMessage.ListNull.Key;
                    response.Message = ErrorCodeMessage.ListNull.Value;
                }

            }
            catch
            {
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                response.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return View(response);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddServices(string Services)
        {
            ResponseBase result  = new ResponseBase();
            dynamic data = null;
            var service = JsonConvert.DeserializeObject<DataAccess.Entities.Service>(Services);
            if (service.Name == null)
            {
                return Redirect("AddServices");
            }
            else
            {
                if(_ServicesServices.CheckServiceName(service.Name)==true)
                {
                    return Redirect("AddServices");
                }    
            }    

            //--Data Insert Part starts here
            result.Message = _ServicesServices.AddNewServices(service);


            if (result.Message == ErrorCodeMessage.Success.Value )
            {
                TempData["SuccessMsg"] = result.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "thất Bại"
                };
                return Json(data);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AssignService(DataAccess.Entities.Service service, int Id)
        {
            ResponseBase result = new ResponseBase();
            FilterBase filter = new FilterBase();
            if (_roomServices.GetHotelRoomById(Id) == null
                && _hotelServices.GetHotelById(Id) == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("AssignService");
            }

            result.Message =  _ServicesServices.AssignService(service, Id);
            //--Data Insert Part starts here

            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect("ServicesList");
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect("ServicesList");
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult RemoveService(int IdService, int IdHotelorRoom)
        {
            var returnUrl = Request.Headers["Referer"].ToString();
            ResponseBase result = new ResponseBase();
            var mameniti = _hotelMamenitiServices.GetHotelMamenitiByIdService(IdService, IdHotelorRoom);
            result.Message = _hotelMamenitiServices.DeleteHotelMameniti(mameniti.Id);
            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect(returnUrl);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult RemoveServiceRoom(int IdService, int IdHotelorRoom)
        {
            var returnUrl = Request.Headers["Referer"].ToString();
            ResponseBase result = new ResponseBase();
            var mameniti = _roomMamenitiServices.GetRoomMamenitiByIdService(IdService, IdHotelorRoom);
            result.Message = _roomMamenitiServices.DeleteRoomMameniti(mameniti.Id);
            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect(returnUrl);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditServices(DataAccess.Entities.Service FormData)
        {
            ResponseBase model = new ResponseBase();

            if (FormData.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("EditServices");
            }

            //--Data update Part starts here
            model.Message = _ServicesServices.EditServices(FormData);
            if (model.Message == ErrorCodeMessage.Success.Value )
            {
                TempData["SuccessMsg"] = model.Message;
                return Redirect("ServicesList");
            }
            else
            {
                TempData["ErrorMsg"] = model.Message;
                return Redirect("ServicesList");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteServices(int Id)
        {
            ResponseBase FormData = new ResponseBase();


            //--getting calling page URL

            //--Data Delete Part starts here
            FormData.Message = _ServicesServices.DeleteServices(Id);

            if (FormData.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect("ServicesList");
            }
            else
            {
                TempData["ErrorMsg"] = FormData.Message;
                return Redirect("ServicesList");
            }

        }

    }
}
