
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using System.Data;
using GoStay.Common;

namespace GoStay.Service
{
    public class ServicesServices : IServicesServices
    {
        private readonly ICommonRepository<DataAccess.Entities.Service> _serviceRepository;
        private readonly ICommonRepository<HotelMameniti> _hotelMamenitiRepository;
        private readonly ICommonRepository<RoomMameniti> _roomMamenitiRepository;
        private readonly ICommonUoW _commonUoW;
        public ServicesServices(ICommonRepository<DataAccess.Entities.Service> serviceRepository, ICommonUoW commonUoW,
            ICommonRepository<HotelMameniti> hotelRepository, ICommonRepository<RoomMameniti> roomRepository)
        {
            _serviceRepository = serviceRepository;
            _commonUoW = commonUoW;
            _hotelMamenitiRepository = hotelRepository;
            _roomMamenitiRepository = roomRepository;
        }

        public PagingList<DataAccess.Entities.Service> GetServicesList(FilterBase filter)
        {
            var listServices = _serviceRepository.FindAll(x => x.Deleted != 1).OrderBy(x => x.Name);

            return listServices.ConvertToPaging(500, filter.PageIndex);
        }
        public bool CheckServiceName(string name)
        {
            if (_serviceRepository.FindAll(x => x.Name == name && x.Deleted !=1).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }    
        }
        public PagingList<DataAccess.Entities.Service> GetAllServices(FilterBase filter, int type)
        {
            if (filter.IdObj == 0)
            {
                var listService = _serviceRepository.FindAll(x => x.Deleted != 1 ).OrderByDescending(x => x.Id);

                return listService.ConvertToPaging(filter.PageSize, filter.PageIndex);
            }
            else
            {
                var listService = _serviceRepository.FindAll(x => x.Id == filter.IdObj && x.Deleted != 1 && x.IdStyle == type)
                                                                .OrderByDescending(x => x.Id);

                return listService.ConvertToPaging(filter.PageSize, filter.PageIndex);
            }
        }

        public List<DataAccess.Entities.Service> GetServicesAssigned(int IdHotelorRoom, int type)
        {
            if (type == 1)
            {
                var listService = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == type)
                                                .Where(x => x.RoomMamenitis.Any(x => x.Idroom == IdHotelorRoom))
                                                .OrderBy(x => x.Name).ToList();

                return listService;
            }
            else
            {
                var listService = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == type)
                                                .Where(x => x.HotelMamenitis.Any(x => x.Idhotel == IdHotelorRoom))
                                                .OrderBy(x => x.Name).ToList();
  
                return listService;
            }
        }
        public List<DataAccess.Entities.Service> GetServicesNotAssigned(int IdHotelorRoom, int type)
        {
            if (type == 1)
            {

                var listService = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == type)
                                                .Where(x => !x.RoomMamenitis.Any(x => x.Idroom == IdHotelorRoom))
                                                .OrderBy(x => x.Name).ToList();

                return listService;
            }
            else
            {
                var listService = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == type)
                                                .Where(x => !x.HotelMamenitis.Any(x => x.Idhotel == IdHotelorRoom))
                                                .OrderBy(x => x.Name).ToList();

                return listService;
            }
        }


        public DataAccess.Entities.Service GetServiceById(int? Id)
        {
            if (Id == null)
                return null;
            var service = _serviceRepository.GetById(Id);
            return service;

        }

        public IQueryable<DataAccess.Entities.Service> GetServices(int type)
        {
            var listService = _serviceRepository.FindAll(x =>x.Deleted != 1 && x.IdStyle == type)
                                                                .OrderByDescending(x => x.Id);

            return listService;

        }
        public IQueryable<DataAccess.Entities.Service> GetServicesSearch(int type)
        {
            var listService = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == type)
                                                                .OrderBy(x => x.AdvantageLevel).Take(30);
            return listService;

        }

        public string AssignService(DataAccess.Entities.Service service, int Id)
        {
            try
            {
                _commonUoW.BeginTransaction();
                if(service.IdStyle == 0)
                {
                    HotelMameniti hotelMameniti = new HotelMameniti();
                    hotelMameniti.Idservices = service.Id;
                    hotelMameniti.Idhotel = Id;
                    _hotelMamenitiRepository.Insert(hotelMameniti);

                }
                if (service.IdStyle == 1)
                {
                    RoomMameniti roomMameniti = new RoomMameniti();
                    roomMameniti.Idservices = service.Id;
                    roomMameniti.Idroom = Id;
                    _roomMamenitiRepository.Insert(roomMameniti);
                }    
                _commonUoW.Commit();

                return ErrorCodeMessage.Success.Value;

            }
            catch
            {
                _commonUoW.RollBack();

                return ErrorCodeMessage.OperationFail.Value;
            }
        }

        public string? AddNewServices(DataAccess.Entities.Service data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _serviceRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditServices(DataAccess.Entities.Service data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _serviceRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteServices(int id)
        {
            try
            {
                var entity = _serviceRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _serviceRepository.SoftDelete(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}" ;
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
    }
}
