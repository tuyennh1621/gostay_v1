using GoStay.DataDto.Air.DOMMakeReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.DOMMakeMultiReservation
{
    public class MultiReservationParams
    {
        public string DepartureDataSession { get; set; }//Phiên dữ liệu tìm kiếm trong DOMFlightData
        public string ReturnDataSession { get; set; }
        public string DepartureFlightSession { get; set; }//Phiên dữ liệu chuyến bay chiều đi trong DOMFlightInfo
        public string ReturnFlightSession { get; set; }
        public List<PassengerInfo> ListPassengers { get; set; }
        public ContactInfo contactInfo { get; set; }
        public string ClientVia { get; set; }
    }
}
