
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
    public class TypeHotelServices : ITypeHotelServices
    {
        private readonly ICommonRepository<TypeHotel> _TypeHotelRepository;
        private readonly ICommonUoW _commonUoW;
        public TypeHotelServices(ICommonRepository<TypeHotel> TypeHotelRepository, ICommonUoW commonUoW)
        {
            _TypeHotelRepository = TypeHotelRepository;
            _commonUoW = commonUoW;
        }
        public IQueryable<TypeHotel> GetAllTypeHotel()
        {
            var listTypeHotel = _TypeHotelRepository.FindAll(x => x.Deleted != 1);

            return listTypeHotel;
        }
        public string GetTypeHotelName(int Id)
        {
            var obj = _TypeHotelRepository.FindSingle(x => x.Id == Id);
            if(obj == null)
            {
                return "Không tồn tại ID";
            }    
            return obj.Type;
        }
        public PagingList<TypeHotel> GetTypeHotelList(FilterBase filter)
        {
            var listTypeHotel = _TypeHotelRepository.FindAll(x => x.Deleted != 1);

            return listTypeHotel.ConvertToPaging(filter.PageSize, filter.PageIndex);
        }

        public string? AddNewTypeHotel(TypeHotel data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _TypeHotelRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditTypeHotel(TypeHotel data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _TypeHotelRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteTypeHotel(int id)
        {
            try
            {
                var entity = _TypeHotelRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _TypeHotelRepository.SoftDelete(entity);
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

