using GoStay.Api.Providers;
using GoStay.DataDto.Scheduler;
using GoStay.Service.Api.Base;
using GoStay.Service.Api.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GoStay.Web.Helpers
{
    public class UpdateTimer : ApiServiceBase
    {
        private static ISchedulerApiServices _schedulerRoomPriceService;
        private static ApiServiceBase _apiServiceBase;
        public static void Init()
        {
            _apiServiceBase = new ApiServiceBase();
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnUpdateHotelRoomPrice);
            aTimer.Interval = 60 * 60 * 1000; // 1h
            aTimer.Enabled = true;
        }

        private static void OnUpdateHotelRoomPrice(object source, ElapsedEventArgs e)
        {
            if(DateTime.Now.Hour == 0)
            {
                _apiServiceBase.Get<List<SchedulerRoomPriceDto>>("Scheduler/update-daily-price");
            }
        }
    }
}
