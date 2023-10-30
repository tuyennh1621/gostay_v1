
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto;
using GoStay.DataDto.Hành_Chính;

namespace GoStay.Services.WebSupport
{
    public interface IProvinceServices
    {
        public List<TinhThanh> GetAllProvince(int? IdCountry = 1);
        string GetProvinceNameById(int IDTinhThanh);
        List<TinhThanhBannerDto> GetTopProvince();
    }
    public interface IDistrictServices
    {
        Quan GetDistrictById(int Id);
        List<QuanDto> GetAllDistrict();
        List<TinhThanh> ProvinceFromForTour();
        List<TinhThanh> ProvinceToForTour();
        string GetProvinceNameByDistrictId(int districtId);
        List<Quan> GetDistrictByProvinceId(int provinceId);
        int[] GetDistrictIdsByProvinceIds(int[] provinceId);
    }

    public interface IWardServices
    {
        List<Phuong> GetAllWards();
        List<Phuong> GetWardsByIdDistrict(int IdQuanHuyen);
        int? GetIdDistrictByIdWard(int IdPhuong);
    }
}
