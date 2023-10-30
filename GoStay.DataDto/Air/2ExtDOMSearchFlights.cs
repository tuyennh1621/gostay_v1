
using GoStay.Common.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Web.Helpers;
using Uno.Equality;

namespace GoStay.DataDto.Air.ExtDOMSearchFlights
{
    public class FlightParams
    {
        //Kiểu hành trình:
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        //Kênh thực hiện request từ client
        public string ClientVia { get; set; }
        //Điều kiện lọc chuyến bay
        public FlightFilter FlightFilter { get; set; }
    }
    public class FlightFilter
    {
        //        Lọc hạng vé 
        //0: Lấy toàn bộ hạng vé 
        //1: Lấy hạng rẻ nhất
        public int FilterType { get; set; }

    }

    public class ExtDOMFlightData
    {
        //Phiên dữ liệu tìm kiếm
        public string DataSession { get; set; }
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        //Danh sách chuyến đi
        public Dictionary<string, ExtDOMFlightInfo> DepartureFlights { get; set; }
        //Danh sách chuyến bay về
        public Dictionary<string, ExtDOMFlightInfo> ReturnFlights { get; set; }
        public Dictionary<string, ExtDOMFlightInfo> Flights { get; set; }

    }
    public class ExtDOMFlightInfo
    {
        //Phiên dữ liệu chuyến bay
        public string FlightSession { get; set; }
        public string AirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Stops { get; set; }
        public int Duration { get; set; }
        //Danh sách chặng bay
        public List<DOMFlightSegment> ListSegment { get; set; }
        //Danh sách hạng giá vé
        public List<DOMFareOption> FareOptions { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        //giá tham khảo
        public double? Price { get 
            {
                return FareOptions.Min(x => x.PriceAdult+x.FeeAdult+x.TaxAdult+x.ServiceFee+x.IssueFee);
            } 
        }
    }
    public class DOMFareOption
    {
        //Phiên dữ liệu hạng vé
        public string FareOptionSession { get; set; }
        //Hạng vé cho hành trình nối chặng
        public Dictionary<string, string> Classes { get; set; }
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
        public List<DOMFareBreakdown> FareBreakdowns { get; set; }
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
    }

}
