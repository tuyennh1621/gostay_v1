

namespace GoStay.DataDto.Air.DOMIssueReservation
{
    public class ReservationParams
    {
        public string DataSession { get; set; }
        public string DepartureFlightSession { get; set; }
        public string ReturnFlightSession { get; set; }
        public List<PassengerInfo> ListPassengers { get; set; }
        public ContactInfo contactInfo { get; set; }
        public string ClientVia { get; set; }

    }
    public class PassengerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //Kiểu khách hàng- ADT: người lớn /CHD: trẻ em /INF: trẻ sơ sinh
        public string Type { get; set; }
        public byte Gender { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public string? PassportExpiryDate { get; set; }
        public string? PassportIssueCountry { get; set; }
        public string? PassportNumber { get; set; }
        //Mã gói hành lý chuyến đi Baggage.Code(lưu ý để null hoặc rỗng nếu không chọn hành lý)
        public string BaggageDeparture { get; set; }
        //Mã gói hành lý chuyến về
        public string BaggageReturn { get; set; }

    }
    public class ContactInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //Kiểu khách hàng- ADT: người lớn /CHD: trẻ em /INF: trẻ sơ sinh
        public byte Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Note { get; set; }


    }
    //    - Thông tin mã đặt chỗ
    //- Khi xuất ngay lỗi mã đặt chỗ sẽ trả về rỗng với hãng Jetstar, Vietjet Air.Riêng Vietnam Airlines
    //nếu không xuất ngay được vẫn có thể trả về mã đặt chỗ PNR(nhưng chưa xuất).
    //- Trong mọi trường hợp lỗi, đều cần kiểm tra kỹ thông tin trên hãng, xem vé đã thực xuất hay
    //không? Tuyệt đối không tiếp tục xuất ngay với thông tin vừa request, để tránh xuất đúp.
    public class ReservationCode
    {
        public string DepartureCode { get; set; }
        //Mã đặt chỗ chiều về string.empty nếu là 1 chiều Có thể cùng mã DepartureCode nếu khứ hồi
        public string ReturnCode { get; set; }
        //Mã giao dịch(chú ý có thể trùng, không dùng làm key lưu dữ liệu)
        public string TransactionCode { get; set; }


    }
}
