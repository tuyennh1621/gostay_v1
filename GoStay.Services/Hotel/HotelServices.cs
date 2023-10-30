
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.HotelDto;
using Microsoft.EntityFrameworkCore;

namespace GoStay.Service
{
    public class HotelServices : IHotelServices
    {
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<Album> _albumRepository;
        private readonly ICommonRepository<TinhThanh> _tinhRepository;
        private readonly ICommonRepository<Quan> _quanRepository;
        private readonly ICommonRepository<Phuong> _phuongRepository;



        private readonly ICommonUoW _commonUoW;
        public DateTime currentDate = DateTime.Now;
        public HotelServices(ICommonRepository<Hotel> hotelRepository, ICommonUoW commonUoW, ICommonRepository<Album> albumRepository,
            ICommonRepository<TinhThanh> tinhRepository, ICommonRepository<Quan> quanRepository, ICommonRepository<Phuong> phuongRepository)
        {
            _hotelRepository = hotelRepository;
            _commonUoW = commonUoW;
            _albumRepository = albumRepository;
            _tinhRepository = tinhRepository;
            _quanRepository = quanRepository;
            _phuongRepository = phuongRepository;
        }

        public HotelListAdmin GetHotelList(GetHotelAdminParam request)
        {
            var count = _hotelRepository.FindAll(x => x.Deleted != 1).Count();
            if (request.NameSearch == null)
            {
                request.NameSearch = "";
            }

            request.NameSearch = request.NameSearch.RemoveUnicode();
            request.NameSearch = request.NameSearch.Replace(" ", string.Empty).ToLower();
            List<Hotel> hotel = new List<Hotel>();
            int total = 0;
            if (request.IdProvince == null || request.IdProvince == 0)
            {
                total = _hotelRepository.FindAll(x => x.Deleted != 1 && x.SearchKey.Contains(request.NameSearch) == true).Count();
                hotel = _hotelRepository.FindAll(x => x.Deleted != 1 && x.SearchKey.Contains(request.NameSearch) == true)
                    .Include(x => x.IdPriceRangeNavigation)
                    .Include(x => x.TypeNavigation)
                    .OrderByDescending(x => x.Id)
                    .Skip(request.PageSize * (request.PageIndex - 1))
                    .Take(request.PageSize).ToList();
            }
            else
            {
                total = _hotelRepository.FindAll(x => x.IdTinhThanh == request.IdProvince && x.Deleted != 1 && x.SearchKey.Contains(request.NameSearch) == true).Count();
                hotel = _hotelRepository.FindAll(x => x.IdTinhThanh == request.IdProvince && x.Deleted != 1 && x.SearchKey.Contains(request.NameSearch) == true)
                    .Include(x => x.IdPriceRangeNavigation)
                    .Include(x => x.TypeNavigation)
                    .Include(x => x.HotelRooms.Where(z => z.Deleted != 1))
                    .OrderByDescending(x => x.Id)
                    .Skip(request.PageSize * (request.PageIndex - 1))
                    .Take(request.PageSize).ToList();
            }
            var result = new HotelListAdmin()
            {
                ListHotel = hotel,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Total = total,

            };
            return result;
        }

        public PagingList<Hotel> GetHotelByTinhThanh(FilterBase filter)
        {
            if (filter.IdObj == 0)
            {
                if (filter.stringsearch == null)
                {
                    var listServices = _hotelRepository.FindAll(x => x.Deleted != 1).OrderByDescending(src => src.IntDate)
                                        .Include(x => x.TypeNavigation)
                                        .Include(x => x.IdPriceRangeNavigation);
                    return listServices.ConvertToPaging(filter.PageSize, filter.PageIndex);
                }
                else
                {
                    var listServices = _hotelRepository.FindAll(x => x.Deleted != 1).OrderByDescending(src => src.IntDate)
                                    .Include(x => x.TypeNavigation)
                                    .Include(x => x.IdPriceRangeNavigation)
                                    .Where(x => x.Name.Contains(filter.stringsearch) == true);
                    return listServices.ConvertToPaging(filter.PageSize, filter.PageIndex);
                }
            }
            else
            {
                if (filter.stringsearch == null)
                {
                    var listServices = _hotelRepository.FindAll(x => x.IdTinhThanh == filter.IdObj && x.Deleted != 1).OrderByDescending(src => src.IntDate)
                                    .Include(x => x.TypeNavigation)
                                    .Include(x => x.IdPriceRangeNavigation);
                    return listServices.ConvertToPaging(filter.PageSize, filter.PageIndex);
                }
                else
                {
                    var listServices = _hotelRepository.FindAll(x => x.IdTinhThanh == filter.IdObj && x.Deleted != 1).OrderByDescending(src => src.IntDate)
                                    .Include(x => x.TypeNavigation)
                                    .Include(x => x.IdPriceRangeNavigation)
                                    .Where(x => x.Name.Contains(filter.stringsearch) == true);
                    return listServices.ConvertToPaging(filter.PageSize, filter.PageIndex);
                }
            }

        }

        public Hotel GetHotelById(int Id)
        {
            var Hotel = _hotelRepository.GetById(Id);

            return Hotel;
        }
        public IQueryable<Hotel> GetAllHotel()
        {
            var listHotel = _hotelRepository.FindAll(x => x.Deleted != 1);

            return listHotel;
        }


        public string? AddNewHotel(Hotel data)
        {
            try
            {
                data.SearchKey = data.Name.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                _commonUoW.BeginTransaction();
                _hotelRepository.Insert(data);
                _tinhRepository.GetById(data.IdTinhThanh).Numrecord++;
                _quanRepository.GetById(data.IdQuan).Numrecord++;
                _phuongRepository.GetById(data.IdPhuong).Numrecord++;
                _commonUoW.Commit();

                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditHotel(Hotel data)
        {
            try
            {
                data.SearchKey = data.Name.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                data.IntDate = (long)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;
                _commonUoW.BeginTransaction();
                var temp = _hotelRepository.FindAll().Where(x => x.Id == data.Id).Take(1).AsNoTracking();
                var idTinhOld = temp.FirstOrDefault().IdTinhThanh;
                int idQuanOld = temp.FirstOrDefault().IdQuan;
                int idPhuongOld = temp.FirstOrDefault().IdPhuong;

                data.CreatedDate = temp.FirstOrDefault().CreatedDate;
                if (data.IdTinhThanh != idTinhOld)
                {
                    _tinhRepository.GetById(idTinhOld).Numrecord--;
                    _tinhRepository.GetById(data.IdTinhThanh).Numrecord++;

                }
                if (data.IdQuan != idQuanOld)
                {
                    _quanRepository.GetById(idQuanOld).Numrecord--;
                    _quanRepository.GetById(data.IdQuan).Numrecord++;
                }
                if (data.IdPhuong != idPhuongOld)
                {
                    _phuongRepository.GetById(idPhuongOld).Numrecord--;
                    _phuongRepository.GetById(data.IdPhuong).Numrecord++;

                }

                _hotelRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteHotel(int id)
        {
            try
            {
                var entity = _hotelRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _hotelRepository.SoftDelete(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
        public string SetMapHotel(SetMapHotelRequest param)
        {
            try
            {
                _commonUoW.BeginTransaction();

                var hotel = _hotelRepository.FindAll(x => x.Id == param.HotelId).SingleOrDefault();
                if (hotel == null)
                {
                    _commonUoW.Commit();
                    return "Not found hotel";
                }
                hotel.LonMap = param.LON;
                hotel.LatMap = param.LAT;
                _hotelRepository.Update(hotel);

                _commonUoW.Commit();
                return "success";

            }
            catch
            {
                _commonUoW.RollBack();
                return "fail";
            }

        }
    }


}

