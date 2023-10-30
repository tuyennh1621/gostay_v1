
using System.Runtime.InteropServices;
using System.Web.Helpers;

namespace GoStay.DataDto.Air.DOMSearchFlights
{
    public class FlightParams
    {
        //Kiểu hành trình: 1: 1 chiều 2: Khứ hồi
        public int ItineraryType { get; set; }
        //Mã sân bay, thành phố đi
        public string StartPoint { get; set; }//HAN
        //Mã sân bay, thành phố đến
        public string EndPoint { get; set; }//SGN
        //Ngày khởi hành (dd/MM/yyyy)
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        //Kênh thực hiện request từ client
        public string ClientVia { get; set; }//Mobile,Web CMS ,Agent...

    }
    public class DOMFlightData
    {
        //Phiên dữ liệu tìm kiếm
        public string DataSession { get; set; }
        //Kiểu hành trình
        public int ItineraryType { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        //Danh sách chuyến đi
        public Dictionary<string, DOMFlightInfo> DepartureFlights { get; set; }
        //Danh sách chuyến bay về
        public Dictionary<string, DOMFlightInfo> ReturnFlights { get; set; }

    }
    public class DOMFlightInfo
    {
        //Phiên dữ liệu chuyến bay
        public string FlightSession { get; set; }
        public string AirlineCode { get; set; }
        //Số hiệu chuyến bay(là số hiệu chuyến bay đầu tiên trong các chặng)
        public string FlightNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Stops { get; set; }
        public int Duration { get; set; }
        public double PriceAdult { get; set; }
        public double PriceChild { get; set; }
        public double PriceInfant { get; set; }
        public double FeeAdult { get; set; }
        public double FeeChild { get; set; }
        public double FeeInfant { get; set; }

        public double TaxAdult { get; set; }
        public double TaxChild { get; set; }
        public double TaxInfant { get; set; }
        //Tổng giá(đã bao gồm thuế phí, không bao gồm phí dịch vụ ServiceFee, phí xuất IssueFee)
        public double TotalPrice { get; set; }
        public double ServiceFee { get; set; }
        //Phí xuất vé
        public double IssueFee { get; set; }
        //Danh sách chặng bay
        public List<DOMFlightSegment> ListSegment { get; set; }
        //Chi tiết giá vé của từng khách
        public List<DOMFareBreakdown> FareBreakdowns { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        //Kỳ hạn xuất vé, sau thời gian này mã đặt chỗ sẽ bị hủy. NULL đối với vé phải xuất luôn
        public DateTime LastTkDate { get; set; }
    }
    public class DOMFareBreakdown
    {
        //Loại hành khách
        public string PassengerType { get; set; }
        //Các thành phần trong giá vé
        public List<DOMChagreInfo> Chagres { get; set; }
    }
    public class DOMChagreInfo
    {
        //Loại chi phí- BASEFARE: Giá vé/ FAREDISCOUNT: /Chiêt khấu giảm giá vé /APPLIEDFARE: Giá vé áp dụng/ FEE: Phí
        public string ChargeType { get; set; }
        //Mã loại phí(đang cập nhật)
        public string Code { get; set; }
        public string Description { get; set; }
        // Chưa thuế
        public double BaseAmount { get; set; }
        // Thuế
        public double TaxAmount { get; set; }
        // tổng
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
        //Thời gian bay(phút)
        public int FlightTime { get; set; }
        public string Class { get; set; }
        public string AirlineCode { get; set; }
    }
}
