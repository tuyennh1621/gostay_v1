using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.DOMMakeReservation
{
    public class ReservationParams

    {
        public string DataSession { get; set; }
        public string DepartureFlightSession { get; set; }
        public string ReturnFlightSession { get; set; }
        public List<PassengerInfo> ListPassengers { get; set; }
        public ContactInfo contactInfo { get; set; }
        public string ClientVia { get; set; }//200

    }
    public class PassengerInfo

    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public byte Gender { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public string? PassportExpiryDate { get; set; }
        public string? PassportIssueCountry { get; set; }
        public string? PassportNumber { get; set; }
        public string? BaggageDeparture { get; set; }
        public string? BaggageReturn { get; set; }

    }
    public class ContactInfo

    {
        public byte   Gender    { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public string Phone     { get; set; }
        public string Email     { get; set; }
        public string Address   { get; set; }
        public string Company   { get; set; }
        public string Note      { get; set; }

    }

    public class ReservationCode

    {
        public string DepartureCode { get; set; }
        public string ReturnCode { get; set; }
        public string TransactionCode { get; set; }

    }
}
