
using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;

using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace GoStay.Service
{
    public class HotelRoomServices : IHotelRoomServices
    {
        private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;

        private readonly ICommonRepository<TypeHotel> _typehotelRepository;
        private readonly ICommonRepository<Palletbed> _palletbedRepository;
        private readonly ICommonRepository<RoomView> _roomViewsRepository;
        private readonly ICommonRepository<ViewDirection> _viewRepository;
        private readonly ICommonRepository<RoomMameniti> _roomServicesRepository;
        private readonly ICommonRepository<DataAccess.Entities.Service> _serviceRepository;


        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;
        public HotelRoomServices(ICommonRepository<HotelRoom> hotelRoomRepository, ICommonUoW commonUoW, IMapper mapper,
            ICommonRepository<TypeHotel> typehotelRepository, ICommonRepository<Hotel> hotelRepository, 
            ICommonRepository<Palletbed> palletbedRepository,ICommonRepository<RoomView> roomViewsRepository,
        ICommonRepository<ViewDirection> viewRepository,
        ICommonRepository<RoomMameniti> roomServicesRepository,
        ICommonRepository<DataAccess.Entities.Service> serviceRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _typehotelRepository = typehotelRepository;
            _hotelRepository = hotelRepository;
            _palletbedRepository = palletbedRepository;
            _roomViewsRepository = roomViewsRepository;
            _roomServicesRepository = roomServicesRepository;
            _serviceRepository = serviceRepository;
            _viewRepository = viewRepository;
        }

        public IQueryable<HotelRoom> GetHotelRoomList(FilterBase filter)
        {
            if (filter.IdObj == 0)
            {
                var listRoom = _hotelRoomRepository.FindAll(x => x.Deleted != 1).OrderByDescending(x => x.IntDate)
                                .Include(x => x.IdhotelNavigation);

                return listRoom;
            }
            else
            {
                var listRoom = _hotelRoomRepository.FindAll(x => x.Idhotel == filter.IdObj && x.Deleted != 1)
                                                .OrderByDescending(x => x.IntDate);

                return listRoom;
            }
        }
        public IQueryable<Palletbed> GetListPalletbed()
        {
            return _palletbedRepository.FindAll().OrderBy(x=>x.Text);
        }
        public HotelRoom GetHotelRoomById(int Id)
        {
            var room = _hotelRoomRepository.GetById(Id);

            return room;
        }
        public IQueryable<HotelRoom> GetAllHotelRoom()
        {
            var listHotelRoom = _hotelRoomRepository.FindAll(x => x.Deleted != 1)
                .Include(x => x.IdhotelNavigation);
            return listHotelRoom;
        }
        public int[]? GetListRoomView(int IdRoom)
        {
            var array = _roomViewsRepository.FindAll(x=>x.IdRoom==IdRoom).Select(x=>x.IdView).ToArray();
            return array;
        }
        public int[]? GetListRoomService(int IdRoom)
        {
            var array = _roomServicesRepository.FindAll(x => x.Idroom == IdRoom).Select(x => x.Idservices).ToArray();
            return array;
        }
        public string? AddNewHotelRoom(HotelRoom data, int[] view, int[] service)
        {
            try
            {
                _commonUoW.BeginTransaction();
                data.SearchKey = data.Name.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                if (data.Iduser == 9)
                {
                    data.Status = 1;//đc duyệt
                }
                else
                {
                    data.Status = 0;//chờ duyệt
                }
                data.MinNight = 1;
                data.DeadLinePreOrder = 12;
                data.CurrentPrice = data.PriceValue;
                _hotelRoomRepository.Insert(data);
                _commonUoW.Commit();
                if(view != null)
                {
                    foreach(var item in view)
                    {
                        _commonUoW.BeginTransaction();
                        _roomViewsRepository.Insert(new RoomView() { IdRoom = data.Id, IdView = item });
                        _commonUoW.Commit();

                    }
                }
                if (service != null)
                {
                    foreach (var item in service)
                    {
                        _commonUoW.BeginTransaction();
                        _roomServicesRepository.Insert(new RoomMameniti() { Idroom = data.Id, Idservices = item });
                        _commonUoW.Commit();
                    }
                }
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditHotelRoom(HotelRoom data, int[] view, int[] service)
        {
            try
            {
                data.SearchKey = data.Name.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                if (data.Iduser == 9)
                {
                    data.Status = 1;//đc duyệt
                }
                else
                {
                    data.Status = 0;//chờ duyệt
                }
                data.IntDate = (long)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;
                _commonUoW.BeginTransaction();
                var listviewold = _roomViewsRepository.FindAll(x => x.IdRoom == data.Id).ToList();
                _roomViewsRepository.RemoveMultiple(listviewold);
                if (view != null)
                {
                    foreach (var item in view)
                    {
                        _roomViewsRepository.Insert(new RoomView() { IdRoom = data.Id, IdView = item });
                    }
                }
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();
                var listserviceold = _roomServicesRepository.FindAll(x => x.Idroom == data.Id).ToList();
                _roomServicesRepository.RemoveMultiple(listserviceold);
                if (service != null)
                {
                    foreach (var item in service)
                    {
                        _roomServicesRepository.Insert(new RoomMameniti() { Idroom = data.Id, Idservices = item });
                    }
                }
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();
                var hotel = _hotelRepository.GetById(data.Idhotel);
                hotel.IntDate = data.IntDate;
                var temp = _hotelRoomRepository.FindAll().Where(x=>x.Id==data.Id).Take(1).AsNoTracking();
                data.CreatedDate = temp.FirstOrDefault().CreatedDate;
                data.RemainNum = temp.FirstOrDefault().RemainNum;
                data.CurrentPrice = temp.FirstOrDefault().CurrentPrice;
                data.MinNight = temp.FirstOrDefault().MinNight;
                data.DeadLinePreOrder = temp.FirstOrDefault().DeadLinePreOrder;
                _hotelRoomRepository.Update(data);
                _hotelRepository.Update(hotel);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteHotelRoom(int id)
        {
            try
            {
                var entity = _hotelRoomRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _hotelRoomRepository.SoftDelete(entity);
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

