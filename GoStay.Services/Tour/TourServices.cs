
using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.TourDto;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GoStay.Service
{
    public class TourServices : ITourServices
    {
        private readonly ICommonRepository<Tour> _tourRepository;
        private readonly ICommonRepository<TourStyle> _tourStyleRepository;
        private readonly ICommonRepository<TourTopic> _tourTopicRepository;
        private readonly ICommonRepository<TourDetail> _tourDetailRepository;
        private readonly ICommonRepository<TourDistrictTo> _tourDistrictToRepository;
        private readonly ICommonRepository<TourVehicle> _tourVehicleRepository;
        private readonly ICommonRepository<Vehicle> _vehicleRepository;
        private readonly ICommonRepository<TourRating> _ratingRepository;
        private readonly ICommonRepository<TourStartTime> _startTimeRepository;

        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;

        public DateTime currentDate = DateTime.Now;
        public TourServices(ICommonRepository<Tour> tourRepository, ICommonUoW commonUoW, ICommonRepository<TourStyle> tourStyleRepository,
            ICommonRepository<TourTopic> tourTopicRepository, ICommonRepository<TourDetail> tourDetailRepository,
            ICommonRepository<TourDistrictTo> tourDistrictToRepository, IMapper mapper, ICommonRepository<TourVehicle> tourVehicalRepository,
            ICommonRepository<Vehicle> vehicalRepository, ICommonRepository<TourRating> ratingRepository, ICommonRepository<TourStartTime> startTimeRepository)
        {
            _tourRepository = tourRepository;
            _commonUoW = commonUoW;
            _tourStyleRepository = tourStyleRepository;
            _tourTopicRepository = tourTopicRepository;
            _tourDetailRepository = tourDetailRepository;
            _tourDistrictToRepository = tourDistrictToRepository;
            _mapper = mapper;
            _tourVehicleRepository = tourVehicalRepository;
            _vehicleRepository = vehicalRepository;
            _ratingRepository = ratingRepository;
            _startTimeRepository = startTimeRepository;
        }

        public Tour GetTourById(int Id)
        {
            var Tour = _tourRepository.GetById(Id);

            return Tour;
        }
        public DataTourAdminDto GetAllTour(int pageSize, int pageIndex)
        {
            DataTourAdminDto data = new DataTourAdminDto();
            var total = _tourRepository.FindAll(x => x.Deleted != 1).Count();
            var listTour = _tourRepository.FindAll(x => x.Deleted != 1).Include(x => x.IdTourStyleNavigation)
                .Include(x => x.TourDistrictTos).Include(x => x.TourVehicles).Include(x => x.RatingNavigation)
                .OrderByDescending(x => x.InDate).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            data.Total = total;
            data.Tours = listTour;
            return data;
        }
        public IQueryable<TourStyle> GetAllTourStyle()
        {
            var listTour = _tourStyleRepository.FindAll().OrderBy(x => x.TourStyle1);

            return listTour;
        }
        public IQueryable<TourTopic> GetAllTourTopic()
        {
            var listTour = _tourTopicRepository.FindAll().OrderBy(x => x.TourTopic1);

            return listTour;
        }
        public List<TourDetail> GetTourDetail(FilterBase? filter)
        {
            var listTour = _tourDetailRepository.FindAll().Where(x => x.IdTours == filter.IdObj && x.Deleted != 1).ToList();

            return listTour;
        }
        public List<Vehicle> GetVehicle()
        {
            var list = _vehicleRepository.FindAll().ToList();

            return list;
        }
        public List<TourRating> GetTourRating()
        {
            var list = _ratingRepository.FindAll().ToList();

            return list;
        }
        public List<TourStartTime> GetTourStartTime()
        {
            var list = _startTimeRepository.FindAll().ToList();

            return list;
        }
        public int AddTourStartTime(string time)
        {
            try
            {
                var temp = _startTimeRepository.FindAll(x => x.StartDate == time);
                if (temp.Count() == 0)
                {
                    TourStartTime data = new TourStartTime() { StartDate = time };
                    _commonUoW.BeginTransaction();

                    _startTimeRepository.Insert(data);
                    _commonUoW.Commit();
                    return data.Id;
                }
                else
                {
                    return temp.First().Id;
                }
            }
            catch
            {
                _commonUoW.RollBack();
                return 0;
            }
        }
        public string? AddNewTour(Tour data, int[] IdDistrictTo, int[] Vehicles)
        {
            try
            {
                data.InDate = (int)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;
                _commonUoW.BeginTransaction();

                _tourRepository.Insert(data);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();

                foreach (var item in IdDistrictTo)
                {
                    _tourDistrictToRepository.Insert(new TourDistrictTo() { IdTour = data.Id, IdDistrictTo = item });
                }
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();
                foreach (var item in Vehicles)
                {
                    _tourVehicleRepository.Insert(new TourVehicle() { IdTour = data.Id, IdVehicle = (byte)item });
                }
                _commonUoW.Commit();

                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }
        public string? AddNewTourDetail(TourDetail data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _tourDetailRepository.Insert(data);
                _commonUoW.Commit();

                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditTour(TourAddDto data)
        {
            try
            {
                _commonUoW.BeginTransaction();

                var tour = _tourRepository.FindAll(x => x.Id == data.Id).Take(1).AsNoTracking().SingleOrDefault();
                if (data.StartDateString != "")
                {
                    data.StartDateString = data.StartDateString.Trim();
                    try
                    {
                        tour.StartDate = DateTime.ParseExact(data.StartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        _commonUoW.Commit();
                        data.IdStartTime = AddTourStartTime(data.StartDateString);
                        _commonUoW.BeginTransaction();

                    }
                }
                else
                {
                    tour.StartDate = new DateTime(2100, 1, 1);
                }
                tour.TourName = data.TourName;
                tour.IdUser = data.IdUser;
                tour.IdTourStyle = data.IdTourStyle;
                tour.IdTourTopic = data.IdTourTopic;
                tour.IdDistrictFrom = data.IdDistrictFrom;
                tour.Price = data.Price;
                tour.Discount = data.Discount;
                tour.Content = data.Content;
                tour.TourSize = data.TourSize;
                tour.Locations = data.Locations;
                tour.Style = data.Style;
                tour.Rating = data.Rating;
                tour.IdStartTime = data.IdStartTime;

                tour.Descriptions = data.Descriptions;
                tour.SearchKey = tour.TourName.RemoveUnicode();
                tour.SearchKey = tour.SearchKey.Replace(" ", string.Empty).ToLower();
                if (tour.Discount is null)
                    tour.Discount = 0;
                tour.Status = 1;
                tour.ActualPrice = tour.Price * (100 - (double)tour.Discount) / 100;
                tour.InDate = (int)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;

                _tourRepository.Update(tour);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();

                var listdistrictold = _tourDistrictToRepository.FindAll(x => x.IdTour == data.Id).ToList();
                _tourDistrictToRepository.RemoveMultiple(listdistrictold);
                foreach (var item in data.IdDistrictTo)
                {
                    _tourDistrictToRepository.Insert(new TourDistrictTo() { IdTour = data.Id, IdDistrictTo = item });
                }
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();
                var listvehicleold = _tourVehicleRepository.FindAll(x => x.IdTour == data.Id).ToList();
                _tourVehicleRepository.RemoveMultiple(listvehicleold);
                foreach (var item in data.Vehicle)
                {
                    _tourVehicleRepository.Insert(new TourVehicle() { IdTour = data.Id, IdVehicle = (byte)item });
                }

                _commonUoW.Commit();

                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }
        public string? EditTourDetail(TourDetail data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _tourDetailRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteTour(int id)
        {
            try
            {
                _commonUoW.BeginTransaction();
                var tour = _tourRepository.GetById(id);
                tour.Deleted = 1;
                _tourRepository.Update(tour);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }

        public string? RemoveTourDetail(int IdDetail)
        {
            try
            {
                _commonUoW.BeginTransaction();
                var tourdetailentity = _tourDetailRepository.GetById(IdDetail);
                if (tourdetailentity != null)
                {
                    tourdetailentity.Deleted = 1;
                    _tourDetailRepository.Update(tourdetailentity);
                    _commonUoW.Commit();
                    return $"{ErrorCodeMessage.Success.Value}";

                }
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.ObjectNull.Value}";

            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
    }


}

