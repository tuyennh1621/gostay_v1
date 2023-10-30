
using GoStay.DataDto.Air.DOMMakeReservation;

namespace GoStay.DataDto.Air.INTMakeReservation
{
    public class ReservationParams
    {
        public string DataSession { get; set; }
        public string GroupSession { get; set; }
        public string DepartureFlightSession { get; set; }
        public string ReturnFlightSession { get; set; }
        public List<PassengerInfo> ListPassengers { get; set; }
        public ContactInfo contactInfo { get; set; }
    }
}
