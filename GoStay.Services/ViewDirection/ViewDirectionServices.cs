
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
    public class ViewDirectionServices : IViewDirectionServices
    {
        private readonly ICommonRepository<ViewDirection> _ViewDirectionRepository;
        private readonly ICommonUoW _commonUoW;
        public ViewDirectionServices(ICommonRepository<ViewDirection> ViewDirectionRepository, ICommonUoW commonUoW)
        {
            _ViewDirectionRepository = ViewDirectionRepository;
            _commonUoW = commonUoW;
        }
        public IQueryable<ViewDirection> GetAllViewDirection()
        {
            var listViewDirection = _ViewDirectionRepository.FindAll(x => x.Deleted != 1).OrderBy(x=>x.NameView);

            return listViewDirection;
        }

        public PagingList<ViewDirection> GetViewDirectionList(FilterBase filter)
        {
            var listViewDirection = _ViewDirectionRepository.FindAll(x => x.Deleted != 1);

            return listViewDirection.ConvertToPaging(filter.PageSize, filter.PageIndex);
        }

        public string? AddNewViewDirection(ViewDirection data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _ViewDirectionRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditViewDirection(ViewDirection data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _ViewDirectionRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteViewDirection(int id)
        {
            try
            {
                var entity = _ViewDirectionRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _ViewDirectionRepository.SoftDelete(entity);
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

