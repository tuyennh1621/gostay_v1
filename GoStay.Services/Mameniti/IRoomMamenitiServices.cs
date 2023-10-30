using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IRoomMamenitiServices
    {
        public RoomMameniti GetRoomMamenitiByIdService(int IdService, int IdRoom);
        public IQueryable<RoomMameniti> GetAllRoomMameniti();

        public string? AddNewRoomMameniti(RoomMameniti data);
        public string? EditRoomMameniti(RoomMameniti data);
        public string? DeleteRoomMameniti(int Id);

    }
}
