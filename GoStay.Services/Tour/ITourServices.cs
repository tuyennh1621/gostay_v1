
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.TourDto;

namespace GoStay.Service
{
    public interface ITourServices
    {
        Tour GetTourById(int Id);
        DataTourAdminDto GetAllTour(int pageSize, int pageIndex);
        string? AddNewTour(Tour data, int[] IdDistrictTo, int[] vehicles);
        string? EditTour(TourAddDto data);
        string? DeleteTour(int id);
        IQueryable<TourStyle> GetAllTourStyle();
        IQueryable<TourTopic> GetAllTourTopic();
        string? AddNewTourDetail(TourDetail data);
        public List<TourDetail> GetTourDetail(FilterBase? filter);
        public string? EditTourDetail(TourDetail data);
        public List<TourRating> GetTourRating();
        public string? RemoveTourDetail(int IdDetail);
        public List<Vehicle> GetVehicle();
        public List<TourStartTime> GetTourStartTime();
        public int AddTourStartTime(string time);

    }

}
