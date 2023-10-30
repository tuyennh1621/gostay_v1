
using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IViewDirectionServices
    {
        public string? AddNewViewDirection(ViewDirection data);
        public string? EditViewDirection(ViewDirection FormData);
        public PagingList<ViewDirection> GetViewDirectionList(FilterBase filter);
        public IQueryable<ViewDirection> GetAllViewDirection();
        public string? DeleteViewDirection(int Id);


    }
}
