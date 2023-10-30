
using GoStay.Common.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using Uno.Equality;

namespace GoStay.DataDto.Air.ExtDOMSearchFlightsShort
{

    public class ExtDOMFlightData
    {
        public string DataSession { get; set; }
        //Danh sách chuyến đi
        public Dictionary<string, ExtDOMFlightInfo> DepartureFlights
        { 
            get;
            set; 
        }
        //Danh sách chuyến bay về
        public Dictionary<string, ExtDOMFlightInfo> ReturnFlights { get; set; }
        public Dictionary<string, ExtDOMFlightInfo> Flights { get; set; }

        public ExtDOMFlightData Clone()
        {
            return new ExtDOMFlightData
            {
                DataSession = DataSession,
                DepartureFlights = CloneDic(DepartureFlights),
                ReturnFlights = CloneDic(ReturnFlights),
                Flights = CloneDic(Flights)
            };
        }

        public Dictionary<string, ExtDOMFlightInfo> CloneDic(Dictionary<string, ExtDOMFlightInfo> OrgDic)
        {
            var dic = new Dictionary<string, ExtDOMFlightInfo>();

            foreach (var kv in OrgDic)
            {
                dic.Add(kv.Key, kv.Value.Clone());
            }

            return dic;
        }

    }
    public class AirSearchInOut
    {
        public int UserId { get; set; }
        //in1
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        public int FlightFilter { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int Present { get; set; }

        //in2
        public SearchAirParam searchAirParam { get; set; }
        //out
        public ExtDOMFlightData Data { get; set; }
    }
    public class ExtDOMFlightInfo
    {
        public string FlightSession { get; set; }
        public int Direction { get; set; }
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string LogoAirline { get; set; }
        public string FlightNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string? StartTime
        {
            get
            {
                return (StartDate.Hour/6).ToString();
            }
        }
        public DateTime EndDate { get; set; }
        public int Stops { get; set; }
        public int Duration { get; set; }
        //danh sách chặng
        public List<DOMFlightSegment> ListSegment { get; set; }
        //Danh sách hạng giá vé
        public List<DOMFareOption> FareOptions { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        //giá tham khảo
        public double? Price 
        { get 
            {
                return FareOptions.Min(x => x.PriceAdult+x.FeeAdult+x.TaxAdult+x.ServiceFee+x.IssueFee);
            } 
        }

        public ExtDOMFlightInfo Clone()
        {
            return new ExtDOMFlightInfo
            {
                FlightSession = this.FlightSession,
                Direction = this.Direction,
                AirlineCode = this.AirlineCode,
                Duration = this.Duration,
                EndDate = this.EndDate,
                EndPoint = this.EndPoint,
                FareOptions = CloneListFareOption(),
                FlightNumber = this.FlightNumber,
                StartDate = this.StartDate,
                StartPoint = this.StartPoint,
                Stops = this.Stops,
                ListSegment = CloneListFlightSegment(),
            };
        }

        public List<DOMFlightSegment> CloneListFlightSegment()
        {
            var lst = new List<DOMFlightSegment>();
            foreach (var item in this.ListSegment)
            {
                lst.Add(item.Clone());

            }
            return lst;
        }
        public List<DOMFareOption> CloneListFareOption()
        {
            var lst = new List<DOMFareOption>();
            foreach (var item in this.FareOptions)
            {
                lst.Add(item.Clone());

            }
            return lst;
        }
    }
    public class DOMFareOption
    {
        
        public string Class { get; set; }
        public double PriceAdult { get; set; }
        public double PriceChild { get; set; }
        public double PriceInfant { get; set; }
        public double FeeAdult { get; set; }
        public double FeeChild { get; set; }
        public double FeeInfant { get; set; }
        public double TaxAdult { get; set; }
        public double TaxChild { get; set; }
        public double TaxInfant { get; set; }
        public double TotalPrice { get; set; }
        public double ServiceFee { get; set; }
        public double IssueFee { get; set; }
        //Kỳ hạn xuất vé, sau thời gian này mã đặt chỗ sẽ bị hủy.NULL đối với vé phải xuất luôn
        public DateTime LastTkDate { get; set; }
        //Chi tiết giá vé của từng khách
        public DOMFareOption Clone()
        {
            return new DOMFareOption()
            {
                Class = this.Class,
                PriceAdult = this.PriceAdult,
                PriceChild = this.PriceChild,
                PriceInfant = this.PriceInfant,
                FeeAdult = this.FeeAdult,
                FeeChild = this.FeeChild,
                FeeInfant = this.FeeInfant,
                TaxAdult = this.TaxAdult,
                TaxChild = this.TaxChild,
                TaxInfant = this.TaxInfant,
                TotalPrice = this.TotalPrice,
                ServiceFee = this.ServiceFee,
                IssueFee = this.IssueFee,
                LastTkDate = this.LastTkDate,
            };
        }
    }
    public class DOMFareBreakdown
    {
        //Loại chi phí -BASEFARE: Giá vé /FAREDISCOUNT: Chiêt khấu giảm giá vé/APPLIEDFARE: Giá vé áp dụng/FEE: Phí
        public string ChargeType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double BaseAmount { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
    }

    public class DOMFlightSegment
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string FlightNumber { get; set; }
        public string Plane { get; set; }
        public int FlightTime { get; set; }
        public string Class { get; set; }
        public string AirlineCode { get; set; }

        public DOMFlightSegment Clone()
        {
            return new DOMFlightSegment
            {
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                StartPoint = this.StartPoint,
                EndPoint = this.EndPoint,
                FlightNumber = this.FlightNumber,
                Plane = this.Plane,
                FlightTime = this.FlightTime,
                Class = this.Class,
                AirlineCode = this.AirlineCode,
            };
        }
    }
}
