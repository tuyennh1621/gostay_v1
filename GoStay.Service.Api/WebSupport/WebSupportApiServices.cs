using GoStay.DataAccess.Entities;
using GoStay.DataDto;
using GoStay.DataDto.Banner;
using GoStay.DataDto.Hành_Chính;
using GoStay.DataDto.TourDto;
using GoStay.Service.Api.Base;
using System.Linq;

namespace GoStay.Service.Api.WebSupport
{
    public class WebSupportApiServices : ApiServiceBase, IWebSupportApiServices
    {
        //Banner
        public List<BannerDetailDto> GetBannerDetail()
        {
            var response = Get<List<BannerDetailDto>>("WebSupport/banner-detail");
            return response.Data;
        }
        //Province
        public List<TinhThanh> GetAllProvince()
        {
            var response = Get<List<TinhThanh>>("WebSupport/provinces");
            return response.Data;
        }
        public string GetProvinceNameById(int Id)
        {
            var response = Get<string>("WebSupport/province-name-by-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public List<TinhThanhBannerDto> GetTopProvince()
        {
            var response = Get<List<TinhThanhBannerDto>>("WebSupport/top-provinces");
            return response.Data;
        }
        //District
        public List<QuanDto> GetAllDistrict()
        {
            var response = Get<List<QuanDto>>("WebSupport/districts");
            return response.Data;
        }
        public Quan GetDistrictById(int Id)
        {
            var response = Get<Quan>("WebSupport/district-by-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public List<TinhThanh> ProvinceFromForTour()
        {
            var response = Get<List<TinhThanh>>("WebSupport/provincefrom-for-tour");
            return response.Data;
        }
        public List<TinhThanh> ProvinceToForTour()
        {
            var response = Get<List<TinhThanh>>("WebSupport/provinceto-for-tour");
            return response.Data;
        }
        public string GetProvinceNameByDistrictId(int Id)
        {
            var response = Get<string>("WebSupport/province-name-by-district-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public List<Quan> GetDistrictByProvinceId(int Id)
        {
            var response = Get<List<Quan>>("WebSupport/districts-by-province-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public int[] GetDistrictIdsByProvinceIds(int[] Id)
        {
            var response = Get<int[]>("WebSupport/districtids-by-provinceids", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        //Wards
        public List<Phuong> GetAllWards()
        {
            var response = Get<List<Phuong>>("WebSupport/wards");
            return response.Data;
        }
        public List<Phuong> GetWardsByIdDistrict(int Id)
        {
            var response = Get<List<Phuong>>("WebSupport/wards-by-district-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public int? GetIdDistrictByIdWard(int Id)
        {
            var response = Get<int?>("WebSupport/district-id-by-ward-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
    }
}