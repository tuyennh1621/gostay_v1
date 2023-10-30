using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.INTGetFareRuleInfo
{
    public class FareRuleParams
    {
        //Phiên dữ liệu tìm kiếm trong INTFlightData
        public string DataSession { get; set; }
        //Phiên dữ liệu tìm kiếm trong INTFlightData
        public string GroupSession { get; set; }
        public bool IsDeparture { get; set; }
    }
    public class INTRules
    {
        //Phiên dữ liệu tìm kiếm trong INTFlightData
        public string RulesTitle { get; set; }
        //Phiên dữ liệu tìm kiếm trong INTFlightData

        public List<string> RulesTextList { get; set; }

    }
}
