
using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface ITypeHotelServices
    {
        public string? AddNewTypeHotel(TypeHotel data);
        public string? EditTypeHotel(TypeHotel FormData);
        public PagingList<TypeHotel> GetTypeHotelList(FilterBase filter);
        public IQueryable<TypeHotel> GetAllTypeHotel();
        public string? DeleteTypeHotel(int Id);
        public string GetTypeHotelName(int Id);


    }
}
