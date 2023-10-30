using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IHotelMamenitiServices
    {
        public string? AddNewHotelMameniti(HotelMameniti data);
        public string? EditHotelMameniti(HotelMameniti data);
        public string? DeleteHotelMameniti(int Id);
        public HotelMameniti GetHotelMamenitiByIdService(int IdService, int IdHotel);
        public IQueryable<HotelMameniti> GetAllHotelMameniti();


    }
}
