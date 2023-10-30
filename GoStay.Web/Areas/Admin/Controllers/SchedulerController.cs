
using AutoMapper;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Scheduler;
using GoStay.Service.Api.Scheduler;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Windows.Globalization.DateTimeFormatting;

namespace GoStay.Web.Areas.Admin.Controllers
{
   

    [Area("Admin")]
    public class SchedulerController : Controller
    {
        public const string SessionKeyID = "PriceId";
        //private List<SchedulerRoomPrice> activities = GetTasks() as List<SchedulerRoomPrice>;
        private readonly ISchedulerApiServices _schedulerApiServices;

        public SchedulerController(ISchedulerApiServices schedulerApiServices)
        {
            _schedulerApiServices = schedulerApiServices;
        }
        public IActionResult Index(int IdRoom = 100, int month=6, int year=2023)
        {
            var Model = new InputScheduler()
            {
                IdRoom = IdRoom,
                Year = year,
                Month = month
            };
            return View(Model);
        }


        public virtual JsonResult Create([DataSourceRequest] DataSourceRequest request, SchedulerRoomPriceDto task, int IdRoom)
        {
            SchedulerRoomPrice scheduler = new SchedulerRoomPrice();
            scheduler.Title = task.Title;
            scheduler.Start = task.DStart;
            scheduler.End = task.DEnd;
            scheduler.RoomId = IdRoom;
            scheduler.RecurrenceRule = task.RecurrenceRule;
            scheduler.IsAllDay = task.IsAllDay;
            scheduler.Description = task.Description;
            scheduler.RecurrenceException = task.RecurrenceException;

            scheduler.Attendees = task.Attendees;
            scheduler.RecurrenceId = task.RecurrenceId;
            scheduler.EndTimezone = task.EndTimezone;
            scheduler.StartTimezone = task.StartTimezone;


            _schedulerApiServices.Insert(scheduler);

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Update([DataSourceRequest] DataSourceRequest request, SchedulerRoomPriceDto task)
        {
            SchedulerRoomPrice scheduler = new SchedulerRoomPrice();
            scheduler.PriceId = task.PriceId;
            scheduler.Title = task.Title;
            scheduler.Start = task.DStart;
            scheduler.End = task.DEnd;
            scheduler.RoomId = task.RoomId;
            scheduler.RecurrenceRule = task.RecurrenceRule;
            scheduler.IsAllDay = task.IsAllDay;
            scheduler.Description = task.Description;
            scheduler.RecurrenceException = task.RecurrenceException;

            scheduler.Attendees = task.Attendees;
            scheduler.RecurrenceId = task.RecurrenceId;
            scheduler.EndTimezone = task.EndTimezone;
            scheduler.StartTimezone = task.StartTimezone;


            if (ModelState.IsValid)
            {
                _schedulerApiServices.Update(scheduler);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Overview_Read([DataSourceRequest] DataSourceRequest request, int idRoom)
        {
            //return Json(GetTasks().ToDataSourceResult(request));
            var list = _schedulerApiServices.GetListScheduler(idRoom).Data;
            var result = Json(list.ToDataSourceResult(request));
            return result;

        }

        public virtual JsonResult Overview_Destroy([DataSourceRequest] DataSourceRequest request, SchedulerRoomPrice task)
        {

            _schedulerApiServices.Destroy(task.PriceId);
            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        //private static IList<SchedulerRoomPrice> GetTasks()
        //{
        //    IList<SchedulerRoomPrice> schedulerTasks = new List<SchedulerRoomPrice>()
        //    {
        //        new SchedulerRoomPrice { PriceId = 1, Title = "5000000", Start = new DateTime(2023,5,28,8,00,00), End= new DateTime(2023,5,28,8,00,00), RecurrenceRule="FREQ=WEEKLY;COUNT=5;BYDAY=MO;WKST=SU", Attendees = 1 },
        //   };
        //    //IList<SchedulerRoomPrice> schedulerTasks = _schedulerApiServices.
        //    return schedulerTasks;
        //}
    }
}
