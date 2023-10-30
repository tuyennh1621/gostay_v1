
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoStay.Service
{
    public class PriceRangeServices : IPriceRangeServices
    {
        private readonly ICommonRepository<PriceRange> _PriceRangeRepository;
        private readonly ICommonUoW _commonUoW;
        public PriceRangeServices(ICommonRepository<PriceRange> PriceRangeRepository, ICommonUoW commonUoW)
        {
            _PriceRangeRepository = PriceRangeRepository;
            _commonUoW = commonUoW;
        }
        public IQueryable<PriceRange> GetAllPriceRange()
        {
            var listPriceRange = _PriceRangeRepository.FindAll(x => x.Deleted != 1);

            return listPriceRange;
        }

        public PagingList<PriceRange> GetPriceRangeList(FilterBase filter)
        {
            var listPriceRange = _PriceRangeRepository.FindAll(x => x.Deleted != 1);

            return listPriceRange.ConvertToPaging(filter.PageSize, filter.PageIndex);
        }

        public string? AddNewPriceRange(PriceRange data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _PriceRangeRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditPriceRange(PriceRange data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _PriceRangeRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeletePriceRange(int id)
        {
            try
            {
                var entity = _PriceRangeRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _PriceRangeRepository.SoftDelete(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
    }
}

