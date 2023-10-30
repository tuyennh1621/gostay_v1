
using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IPriceRangeServices
    {
        public string? AddNewPriceRange(PriceRange data);
        public string? EditPriceRange(PriceRange FormData);
        public PagingList<PriceRange> GetPriceRangeList(FilterBase filter);
        public IQueryable<PriceRange> GetAllPriceRange();
        public string? DeletePriceRange(int Id);
    }
}
