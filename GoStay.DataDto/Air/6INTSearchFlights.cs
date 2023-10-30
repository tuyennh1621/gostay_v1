using GoStay.DataDto.Air.DOMSearchFlights;

namespace GoStay.DataDto.Air.INTSearchFlights
{
    public class FlightParams
    {
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        public string ClientVia { get; set; }
        public FlightFilter FlightFilter { get; set; }
    }
    public class FlightFilter
    {
        public List<string> FareClasses { get; set; }
    }
    public class INTFlightData
    {
        public string DataSession { get; set; }
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        public Dictionary<string, INTFlightGroup> FlightGroups { get; set; }
    }
    public class INTFlightGroup
    {
        public string GroupSession { get; set; }
        public string PlatingCarrier { get; set; }
        public string CabinClass { get; set; }
        public double ServiceFee { get; set; }
        public double PriceAdult { get; set; }
        public double PriceChild { get; set; }
        public double PriceInfant { get; set; }
        public bool HasChangedClass { get; set; }
        public double TotalPrice { get; set; }
        public Dictionary<string, INTFlightInfo> DepartureFlights { get; set; }
        public Dictionary<string, INTFlightInfo> ReturnFlights { get; set; }

    }
    public class INTFlightInfo
    {
        public string FlightSession { get; set; }
        public string AirlineCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Stops { get; set; }
        public int Duration { get; set; }
        public List<INTFlightSegment> ListSegment { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime LastTkDate { get; set; }
    }
    public class INTFlightSegment
    {
        public string AirlineCode { get; set; }
        public string ClassAdult { get; set; }
        public string ClassChild { get; set; }
        public string ClassInfant { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime StartTerminal { get; set; }
        public string EndTerminal { get; set; }
        public string Plane { get; set; }
        public string FlightNumber { get; set; }
        public int FlightTime { get; set; }
        public bool  IsLastItem { get; set; }
        public int StopTime { get; set; }
    }
}
