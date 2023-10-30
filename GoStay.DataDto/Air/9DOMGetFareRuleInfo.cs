using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.DOMGetFareRuleInfo
{
    public class FareRuleParams
    {
        //Phiên dữ liệu tìm kiếm trong DOMFlightData
        public string DataSession { get; set; }
        //Phiên dữ liệu chuyến bay trong DOMFlightInfo
        public string FlightSession { get; set; }

    }
}
