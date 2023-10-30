using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ConfigurationEntity
    {
        public int ConfigurationID { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public int ConfigurationType { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }

    public class ConfigurationEntityWithTotalRecordCount : ConfigurationEntity
    {
        public int TotalRecords { get; set; }
    }
}
