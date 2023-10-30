using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.DOMGetBaggages
{
    public class BaggageParams
    {
        public string dataSession { get; set; }
        public string departureFlightSession { get; set; }
        public string returnFlightSession { get; set; }
    }

    public class BaggageData
    {
        public List<Baggage> DepartureBaggages { get; set; }
        public List<Baggage> ReturnBaggages { get; set; }


    }
    public class Baggage
    {
        public string AirlineCode { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public string Value { get; set; }
    }
}
