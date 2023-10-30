using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class DataContextHelper : IDataContextHelper
    {
        private readonly IConfiguration _configuration;


        public DataContextHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnetionString = _configuration.GetConnectionString("OnlineQuizConn");
            providerName = "System.Data.SqlClient";
        }
        public string ConnetionString { get; }
        public string providerName { get; }

    }
}
