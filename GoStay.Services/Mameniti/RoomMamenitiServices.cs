using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public class RoomMamenitiServices : IRoomMamenitiServices
    {
        private readonly ICommonRepository<RoomMameniti> _RoomMamenitiRepository;
        private readonly ICommonUoW _commonUoW;
        public RoomMamenitiServices(ICommonRepository<RoomMameniti> RoomMamenitiRepository, ICommonUoW commonUoW)
        {
            _RoomMamenitiRepository = RoomMamenitiRepository;
            _commonUoW = commonUoW;
        }

        public RoomMameniti GetRoomMamenitiByIdService(int IdService, int IdRoom)
        {
            try
            {
                return _RoomMamenitiRepository.FindSingle(x => x.Idservices == IdService && x.Idroom == IdRoom);
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<RoomMameniti> GetAllRoomMameniti()
        {
            try
            {
                return _RoomMamenitiRepository.FindAll();
            }
            catch
            {
                return null;
            }
        }
        public string? AddNewRoomMameniti(RoomMameniti data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _RoomMamenitiRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditRoomMameniti(RoomMameniti data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _RoomMamenitiRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteRoomMameniti(int Id)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _RoomMamenitiRepository.Remove(Id);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
    }
}
