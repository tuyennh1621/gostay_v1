using GoStay.DataDto.Air.DOMSearchFlights;
using GoStay.DataDto.Air;
using GoStay.Service.Api.Air.Base;
using GoStay.DataDto.Air.ExtDOMSearchFlights;
using GoStay.DataDto.Air.DOMGetBaggages;
using GoStay.DataDto.Air.DOMMakeReservation;
using GoStay.DataDto.Air.DOMMakeMultiReservation;
using GoStay.DataDto.Air.INTSearchFlights;
using GoStay.DataDto.Air.INTGetFareRuleInfo;
using GoStay.DataDto.Air.DOMAirPrice;
using GoStay.DataDto.Air.IssueTicket;
using System.Security.Cryptography;
using System.Security.Policy;
using GoStay.DataDto.Air.OpenPNR;
using GoStay.DataDto.Air.UpdatePaymentInfo;
using GoStay.DataDto.Air.RequestIssueTicket;
using GoStay.DataDto.Air.RequestSupport;
using GoStay.DataDto.Air.DOMIssueReservation;
using GoStay.DataAccess.Entities;

namespace GoStay.Service.Api.Air
{
    public partial class AirApiServices : ApiAirBase, IAirApiServices
    {
        // 1- Thực hiện việc submit các tiêu chí tìm kiếm để tìm danh sách các chuyến bay và các mức giá
        //của chuyến bay nội địa 
        //- Phí dịch vụ, phí xuất vé tính trên cả chiều đi và về(nếu khứ hồi) với Người lớn và Trẻ em
        public ResultData<DOMFlightData> DOMSearchFlights(RequestData<DataDto.Air.DOMSearchFlights.FlightParams> request)
        {
            var response = Post<RequestData<DataDto.Air.DOMSearchFlights.FlightParams>,
                                DOMFlightData>("AirData/DOMSearchFlights", request);
            return response;
        }
        // 2 Thực hiện việc submit các tiêu chí tìm kiếm để tìm danh sách các chuyến bay và các hạng vé
        //của chuyến bay nội địa 
        //- Phí dịch vụ, phí xuất vé tính trên cả chiều đi và về(nếu khứ hồi) với Người lớn và Trẻ em
        public ResultData<ExtDOMFlightData> ExtDOMSearchFlightsFull(RequestData<DataDto.Air.ExtDOMSearchFlights.FlightParams> request)
        {
            var response = Post<RequestData<DataDto.Air.ExtDOMSearchFlights.FlightParams>,
                                ExtDOMFlightData>("AirData/ExtDOMSearchFlights", request);
            return response;
        }
        public ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData> ExtDOMSearchFlights(RequestData<DataDto.Air.ExtDOMSearchFlights.FlightParams> request)
        {
            var response = Post<RequestData<DataDto.Air.ExtDOMSearchFlights.FlightParams>,
                                DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData>("AirData/ExtDOMSearchFlights", request);
            return response;
        }
        //  3 - Thực hiện việc submit thông tin của chuyến bay đã lựa chọn để lấy về thông tin hành lý ký
        //gửi
        //- Bổ sung hành lý ký gửi chỉ cho 2 hãng hàng không là Vietjet Air và Jesta Airways
        public ResultData<BaggageData> DOMGetBaggages(RequestData<BaggageParams> request)
        {
            var response = Post<RequestData<BaggageParams>, ResultData<BaggageData>>("AirData/DOMGetBaggages", request);
            return response.Data;
        }
        //  4  - Thực hiện việc submit thông tin hành khách, hành lý, thông tin chuyến bay để đặt giữ chỗ
        //- Đối với chuyến bay khứ hồi, code đặt giữ chỗ chiều đi và về có thể giống nhau nếu cùng 1 hãng
        public ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> DOMMakeReservation(RequestData<DataDto.Air.DOMMakeReservation.ReservationParams> request)
        {
            var response = Post<RequestData<DataDto.Air.DOMMakeReservation.ReservationParams>, ResultData<DataDto.Air.DOMMakeReservation.ReservationCode>>("AirData/DOMMakeReservation", request);
            return response.Data;
        }
        // 5 - Tương tự hàm DOMMakeReservation, thực hiện cho việc đặt chỗ trên nhiều DataSession khác
        //        nhau
        //Ví dụ: Client cho user Search khứ hồi(nhận được DataSession), user chọn chuyến đi(hoặc về) và
        //thực hiện tìm lại chuyến về(hoặc đi) trên 1 ngày khác, lúc này client phải thực hiện thêm 1
        //Search mới dạng 1 chiều(nhận được DataSession mới tương ứng)
        //- Hàm đặt giữ chỗ này có thể dùng thay thế DOMMakeReservation
        public ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> DOMMakeMultiReservation(RequestData<MultiReservationParams> request)
        {
            var response = Post<RequestData<MultiReservationParams>, ResultData<DataDto.Air.DOMMakeReservation.ReservationCode>>("AirData/DOMMakeMultiReservation", request);
            return response.Data;
        }
        //  6 - Thực hiện việc submit các tiêu chí tìm kiếm để tìm danh sách các chuyến bay và các
        //mức giá của chuyến bay quốc tế
        //- Tổng là giá trên cả hành trình đã bao gồm thuế phí sân bay.Phí dịch vụ tính trên hành
        //trình (1 chiều hay khứ hồi giống nhau), theo số Người lớn và Trẻ em
        public ResultData<INTFlightData> INTSearchFlights(RequestData<DataDto.Air.INTSearchFlights.FlightParams> request)
        {
            var response = Post<RequestData<DataDto.Air.INTSearchFlights.FlightParams>,
                        INTFlightData>("AirData/INTSearchFlights", request);
            return response;
        }
        // 7 - Thực hiện việc submit thông tin hành khách, hành lý, thông tin chuyến bay để đặt giữ chỗ
        public ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> 
            INTMakeReservation(RequestData<DataDto.Air.INTMakeReservation.ReservationParams> request)
        {
            var response = Post<RequestData<DataDto.Air.INTMakeReservation.ReservationParams>,
                        DataDto.Air.DOMMakeReservation.ReservationCode> ("AirData/INTMakeReservation", request);
            return response;
        }

        //  8 - Do đặc tính dữ liệu thay đổi liên tục nên dữ liệu tìm kiếm chỉ tồn tại trong một thời gian
        //nhất định
        //- Client chủ động thực hiện việc giải phóng tài nguyên dữ liệu tìm kiếm chuyến bay
        //(trong các trường hợp user thực hiện lại phiên tìm kiếm khác, đóng trình duyệt, tắt ứng dụng,...)
        //- Mặc định Server sẽ tự giải phóng tài nguyên sau một khoảng thời gian timeout(10 mins), lúc
        //này nếu client thực hiện các tác vụ(đặt giữ chỗ, lấy hành lý,...)
        public ResultData<string> ReleaseDataSession(RequestData<string> request)
        {
            // input: data session trong INTFlightData/DOMFlightData
            var response = Post<RequestData<string>,string
                        >("AirData/ReleaseDataSession", request);
            return response;
        }
        // 9 - Lấy thông tin điều kiện vé & hành lý cho chuyến bay nội địa
        public ResultData<List<string>> DOMGetFareRuleInfo(RequestData<DataDto.Air.DOMGetFareRuleInfo.FareRuleParams> request)
        {
            // input: data session trong INTFlightData/DOMFlightData
            var response = Post<RequestData<DataDto.Air.DOMGetFareRuleInfo.FareRuleParams>, List<string>
                        >("AirData/DOMGetFareRuleInfo", request);
            return response;
        }
        // 10  - Lấy thông tin điều kiện vé & hành lý cho chuyến bay quốc tế
        //- Điều kiện giá vé này áp dụng trên INTFlightGroup
        public ResultData<List<INTRules>> INTGetFareRuleInfo(RequestData<FareRuleParams> request)
        {
            var response = Post<RequestData<FareRuleParams>, List<INTRules>
                        > ("AirData/INTGetFareRuleInfo", request);
            return response;
        }
        //  11  - Thực hiện lấy mặt vé và thông tin giá chuyến bay nội địa
        //- Đối với hành trình khứ hồi mà code đặt giữ chỗ chiều đi & về giống nhau thì chỉ cần thực hiện
        //xuất vé với code này 1 lần
        public ResultData<PNRPriceQuoteResponse> DOMAirPrice(RequestData<DataDto.Air.DOMAirPrice.PNRParams> request)
        {
            var response = Post<RequestData<DataDto.Air.DOMAirPrice.PNRParams>, PNRPriceQuoteResponse
                        >("AirData/DOMAirPrice", request);
            return response;
        }
        //   12  Thực hiện xuất vé cho chuyến bay nội địa
        //- Đối với hành trình khứ hồi mà code đặt giữ chỗ chiều đi & về giống nhau thì chỉ cần thực hiện
        //xuất vé với code này 1 lần
        public ResultData<string> IssueTicket(RequestData<IssueTicketParams> request)
        {
            var response = Post<RequestData<IssueTicketParams>, string
                        >("AirData/IssueTicket", request);
            //output : Mã đặt chỗ đối với VJ, JQ Mặt vé đối với VN
            return response;
        }
        // 13 - Thực hiện việc mở mặt vé dựa theo mã đặt chỗ của các hãng Vietnam Airlines, Vietjet Air,
        //Jetstar.Mặt vé bao gồm thông tin: Tình trang thanh toán, hành trình, danh sách hành khách,
        //giá tiền và thuế phí.
        public ResultData<PNRResponse> OpenPNR(RequestData<DataDto.Air.OpenPNR.PNRParams> request)
        {
            var response = Post<RequestData<DataDto.Air.OpenPNR.PNRParams>, PNRResponse
                        >("AirData/OpenPNR", request);
            return response;
        }
        // 14 - Cập nhật thông tin thanh toán của khách sau khi thực hiện đặt giữ chỗ thành công.
        public ResultData<string> UpdatePaymentInfo(RequestData<PaymentInfoParams> request)
        {
            var response = Post<RequestData<PaymentInfoParams>,string
                        >("AirData/UpdatePaymentInfo", request);
            //output : non
            return response;
        }
        // 15 - Với các vé khó đối tác không thể tự xuất cần gửi yêu cầu sang bộ phận TKT xuất vé hộ.
        public ResultData<string> RequestIssueTicket(RequestData<RequestIssueParams> request)
        {
            var response = Post<RequestData<RequestIssueParams>, string
                        >("AirData/RequestIssueTicket", request);
            //output : non
            return response;
        }
        // 16 - Gửi các yêu cầu hỗ trợ cập nhật giao dịch, thay đổi thông tin, hành trình, thêm hành lý...
        public ResultData<string> RequestSupport(RequestData<SupportRequestParams> request)
        {
            var response = Post<RequestData<SupportRequestParams>, string
                        >("AirData/RequestSupport", request);
            //output : non
            return response;
        }
        // 17 - Thực hiện việc thanh toán ngay đặt chỗ
        //- Đối với chuyến bay khứ hồi, code đặt chỗ chiều đi và về có thể giống nhau nếu cùng 1 hãng
        public ResultData<DataDto.Air.DOMIssueReservation.ReservationCode> 
                        DOMIssueReservation(RequestData<DataDto.Air.DOMIssueReservation.ReservationParams> request)
        {
            var response = Post<RequestData<DataDto.Air.DOMIssueReservation.ReservationParams>, DataDto.Air.DOMIssueReservation.ReservationCode
                        >("AirData/DOMIssueReservation", request);
            //output : non
            return response;
        }
    }
}
