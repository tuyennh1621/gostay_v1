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
    public class HotelMamenitiServices : IHotelMamenitiServices
    {
        private readonly ICommonRepository<HotelMameniti> _HotelMamenitiRepository;
        private readonly ICommonUoW _commonUoW;
        public HotelMamenitiServices(ICommonRepository<HotelMameniti> HotelMamenitiRepository, ICommonUoW commonUoW)
        {
            _HotelMamenitiRepository = HotelMamenitiRepository;
            _commonUoW = commonUoW;
        }

        public HotelMameniti GetHotelMamenitiByIdService(int IdService, int IdHotel)
        {
            try
            {
                return _HotelMamenitiRepository.FindSingle(x=>x.Idservices == IdService && x.Idhotel == IdHotel);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<HotelMameniti> GetAllHotelMameniti()
        {
            try
            {
                return _HotelMamenitiRepository.FindAll();
            }
            catch
            {
                return null;
            }
        }
        public string? AddNewHotelMameniti(HotelMameniti data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _HotelMamenitiRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditHotelMameniti(HotelMameniti data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _HotelMamenitiRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteHotelMameniti(int Id)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _HotelMamenitiRepository.Remove(Id);
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
