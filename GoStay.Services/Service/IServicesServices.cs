
using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace GoStay.Service
{ 
    public interface IServicesServices
    {
        public string? AddNewServices(DataAccess.Entities.Service data);
        public PagingList<DataAccess.Entities.Service> GetServicesList(FilterBase filter);
        public string? EditServices(DataAccess.Entities.Service data);
        public string? DeleteServices(int Id);

        public PagingList<DataAccess.Entities.Service> GetAllServices(FilterBase filter, int type);
        public DataAccess.Entities.Service GetServiceById(int? Id);
        public string AssignService(DataAccess.Entities.Service service, int Id);
        public IQueryable<DataAccess.Entities.Service> GetServices(int type);
        public List<DataAccess.Entities.Service> GetServicesAssigned(int IdHotelorRoom, int type);
        public List<DataAccess.Entities.Service> GetServicesNotAssigned(int IdHotelorRoom, int type);
        public bool CheckServiceName(string name);
        public IQueryable<DataAccess.Entities.Service> GetServicesSearch(int type);

    }
}
