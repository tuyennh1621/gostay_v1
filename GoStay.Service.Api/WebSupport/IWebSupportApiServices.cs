using GoStay.DataAccess.Entities;
using GoStay.DataDto;
using GoStay.DataDto.Banner;
using GoStay.DataDto.Hành_Chính;
using GoStay.DataDto.TourDto;

namespace GoStay.Service.Api.WebSupport
{
    public interface IWebSupportApiServices
    {
        public List<BannerDetailDto> GetBannerDetail();

        public List<TinhThanh> GetAllProvince();
        public string GetProvinceNameById(int Id);
        public List<TinhThanhBannerDto> GetTopProvince();

        List<QuanDto> GetAllDistrict();
        Quan GetDistrictById(int Id);
        List<TinhThanh> ProvinceFromForTour();
        List<TinhThanh> ProvinceToForTour();
        string GetProvinceNameByDistrictId(int Id);
        List<Quan> GetDistrictByProvinceId(int Id);
        int[] GetDistrictIdsByProvinceIds(int[] Id);

        List<Phuong> GetAllWards();
        List<Phuong> GetWardsByIdDistrict(int Id);
        int? GetIdDistrictByIdWard(int Id);
    }
}