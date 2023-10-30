using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.News;
using GoStay.Service.Api.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using Newtonsoft.Json;
using GoStay.DataDto.Scheduler;

namespace GoStay.Service.Api.Scheduler
{
   
    public class SchedulerApiServices : ApiServiceBase, ISchedulerApiServices
    {
        public ResponseBase<int> Insert(SchedulerRoomPrice scheduler)
        {
            var response = Post<SchedulerRoomPrice, int>("Scheduler/insert", scheduler);
            return response;
        }
        public ResponseBase<int> Update(SchedulerRoomPrice scheduler)
        {
            var response = Post<SchedulerRoomPrice, int>("Scheduler/update", scheduler);
            return response;
        }
        public ResponseBase<SchedulerRoomPrice> GetScheduler(int Id)
        {
            var response = Get<SchedulerRoomPrice>("Scheduler/get", new KeyValuePair<string, object>("Id", Id));
            return response;
        }
        public ResponseBase<List<SchedulerRoomPrice>> GetListScheduler( int RoomId)
        {
            var response = Get<List<SchedulerRoomPrice>>("Scheduler/get-list", 
                new KeyValuePair<string, object>("RoomId", RoomId));
            return response;
        }

        public ResponseBase<string> Destroy(int Id)
        {
            var response = Delete<string>("Scheduler/destroy", new KeyValuePair<string, object>("Id", Id));
            return response;
        }

        public ResponseBase<List<SchedulerRoomPriceDto>> UpdateDailyPriceForAllRoom()
        {
            var response = Get< List<SchedulerRoomPriceDto>> ("Scheduler/update-daily-price");
            return response;
        }
        public ResponseBase<Dictionary<string, double>> GetRoomPriceInFuture(string futuretime, int RoomId)
        {
            var response = Get<Dictionary<string, double>>("Scheduler/room-price-future"
                , new KeyValuePair<string, object>("futuretime", futuretime)
                , new KeyValuePair<string, object>("RoomId", RoomId));
            return response;
        }
    }
}