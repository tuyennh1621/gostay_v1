using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using GoStay.DataDto.Hành_Chính;
using AutoMapper;

using GoStay.DataDto;
using GoStay.Common;

namespace GoStay.Services.WebSupport
{

    public class ProvinceServices : IProvinceServices
    {
        private readonly ICommonRepository<TinhThanh> _TinhRepository;
        private IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public ProvinceServices(ICommonRepository<TinhThanh> tinhRepository, ICommonUoW commonUoW, IMapper mapper)
        {
            _TinhRepository = tinhRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
        }

        public List<TinhThanh> GetAllProvince(int? IdCountry=1)
        {
            var result  = _TinhRepository.FindAll(x=>x.IdCountry== IdCountry)
                .OrderBy(x => x.TenTt).OrderBy(x => x.Stt).ToList();
            return result;
        }


        public string GetProvinceNameById(int Id)
        {

            var provinceName = _TinhRepository.FindAll(x => x.Id == Id).Select(x => x.TenTt).FirstOrDefault();
            if(provinceName is null)
            {
                provinceName = "Việt Nam";
            }
            return provinceName;
            ;
        }
        public List<TinhThanhBannerDto> GetTopProvince()
        {
            var Data = _mapper.Map<List<TinhThanh>, List<TinhThanhBannerDto>>( _TinhRepository.FindAll().OrderBy(x=>x.Stt).Take(16).ToList());
            return Data;
        }
    }

    public class DistrictServices : IDistrictServices
    {
        private readonly ICommonRepository<Quan> _QuanRepository;
        private readonly ICommonRepository<TourDistrictTo> _tourDistrictRepository;
        private IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public DistrictServices(ICommonRepository<Quan> QuanRepository, ICommonUoW commonUoW, ICommonRepository<TourDistrictTo> tourDistrictRepository
                                , IMapper mapper)
        {
            _QuanRepository = QuanRepository;
            _commonUoW = commonUoW;
            _tourDistrictRepository = tourDistrictRepository;
            _mapper = mapper;
        }
        public Quan GetDistrictById(int Id)
        {
            var Data= _QuanRepository.GetById(Id);
            return Data;
        }

        public List<QuanDto> GetAllDistrict()
        {
            var quans= _QuanRepository.FindAll().Include(x=>x.IdTinhThanhNavigation).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).ToList();
            var quansdto = _mapper.Map<List<Quan>, List<QuanDto>>(quans);
            quansdto.ForEach(x => x.ProvinceName = quans.Where(y => y.Id == x.Id).Single().IdTinhThanhNavigation.TenTt);
            return quansdto;
        }

        public List<TinhThanh> ProvinceFromForTour()
        {
            var result  = _QuanRepository.FindAll(x => x.Tours.Any()).Include(x => x.IdTinhThanhNavigation)
                .Select(x=>x.IdTinhThanhNavigation).OrderBy(x=>x.Stt).ToList();
            return result;
        }

        public List<TinhThanh> ProvinceToForTour()
        {
            var result = _tourDistrictRepository.FindAll().Include(x=>x.IdDistrictToNavigation)
                .ThenInclude(x=>x.IdTinhThanhNavigation).Select(x=>x.IdDistrictToNavigation.IdTinhThanhNavigation)
                .OrderBy(x => x.Stt).Distinct().ToList();
            return result;
        }

        public string GetProvinceNameByDistrictId(int districtId)
        {
            var Data = _QuanRepository.FindAll(x => x.Id == districtId).Include(x=>x.IdTinhThanhNavigation).SingleOrDefault().IdTinhThanhNavigation.TenTt;
            return Data;
        }

        public List<Quan> GetDistrictByProvinceId(int provinceId)
        {
            var result = _QuanRepository.FindAll(x => x.IdTinhThanh == provinceId).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).ToList();
            return result;
        }
        public int[] GetDistrictIdsByProvinceIds(int[] provinceId)
        {
            List<int> district = new List<int>();
            foreach (var id in provinceId)
            {
                district.AddRange(_QuanRepository.FindAll(x => x.IdTinhThanh == id).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).Select(x => x.Id).ToList());
            }
            return district.ToArray();
        }

    }

    public class WardServices : IWardServices
    {
        private readonly ICommonRepository<Phuong> _PhuongRepository;
        private readonly ICommonRepository<Quan> _QuanRepository;
        private readonly ICommonUoW _commonUoW;
        public WardServices(ICommonRepository<Phuong> PhuongRepository, ICommonUoW commonUoW, ICommonRepository<Quan> quanRepository)
        {
            _PhuongRepository = PhuongRepository;
            _commonUoW = commonUoW;
            _QuanRepository = quanRepository;
        }

        public List<Phuong> GetAllWards()
        {

            var Data = _PhuongRepository.FindAll(x => x.Stt > 0)
                .OrderBy(x => x.Stt).OrderBy(x => x.Tenphuong).ToList();
            return Data; 
        }
        public List<Phuong> GetWardsByIdDistrict(int IdQuanHuyen)
        {

            var result  = _PhuongRepository.FindAll(x => x.Stt > 0 && x.IdQuan == IdQuanHuyen)
                .OrderBy(x => x.Stt).OrderBy(x => x.Tenphuong).ToList();
            return result;
        }
        public int? GetIdDistrictByIdWard(int IdPhuong)
        {
            var Data = _PhuongRepository.FindAll().FirstOrDefault(x => x.Id == IdPhuong).IdQuan;
            return Data;
        }
    }
}
