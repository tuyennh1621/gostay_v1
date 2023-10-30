using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public interface IDataContextHelper
    {
        public string ConnetionString { get; }
        public string providerName { get; }

    }
}
