

using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Scheduler;

namespace GoStay.Service.Api.Scheduler
{
    public interface ISchedulerApiServices
    {
        public ResponseBase<int> Insert(SchedulerRoomPrice scheduler);
        public ResponseBase<int> Update(SchedulerRoomPrice scheduler);
        public ResponseBase<SchedulerRoomPrice> GetScheduler(int Id);
        public ResponseBase<List<SchedulerRoomPrice>> GetListScheduler( int RoomId);
        public ResponseBase<string> Destroy(int Id);
        public ResponseBase<List<SchedulerRoomPriceDto>> UpdateDailyPriceForAllRoom();
        public ResponseBase<Dictionary<string, double>> GetRoomPriceInFuture(string futuretime, int RoomId);

    }
}