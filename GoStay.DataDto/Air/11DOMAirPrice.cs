using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.DOMAirPrice
{
    public class PNRParams
    {
        //Code đặt giữ chỗ trong ReservationCode
        public string PNRCode { get; set; }//NNYPSD
        //Hãng hàng không
        public string AirlineCode { get; set; }//VN
        //Mặt vé(dùng lưu trữ thông tin cho kế toán)
        public string BookingInfo { get; set; }

    }
    public class PNRPriceQuoteResponse
    {
        //Thông tin mặt vé
        public string PNRContent { get; set; }
        //Thông tin giá vé
        public string PriceQuoteContent { get; set; }
        //Số khách người lớn và trẻ em
        public int ADTCHDCount { get; set; }
        //Tổng tiền đặt chỗ
        public double Amount { get; set; }


    }
}
