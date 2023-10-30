
using Microsoft.Extensions.Configuration;

using DAL.Entities;
using IDataContextHelper = DAL.Entities.IDataContextHelper;

namespace GoStay.Service
{


    public class AdminServices : IAdminServices
    {
        private readonly IConfiguration _configuration;
        private readonly IDataContextHelper _contextHelper;

        public AdminServices(IConfiguration configuration,  IDataContextHelper contextHelper)
        {
            _configuration = configuration;
            _contextHelper = contextHelper;
        }





    }

    
}
