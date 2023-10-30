
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoStay.DataDto.Air.RequestSupport
{
    public class SupportRequestParams
    {
        public string RequestType { get; set; } //AIR: Vé máy bay /ISR: Bảo hiểm VISA: /Visa, hộ chiếu/ HOTEL: Phòng, khách sạn
        public string RequestKey { get; set; }
        //Key của đối tác tự sinh khi tạo request, hệ thống sẽ trả kèm về khi response
        public string ResponseKey { get; set; }
        public double MaxPrice { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContactInfo { get; set; }

    }
}
