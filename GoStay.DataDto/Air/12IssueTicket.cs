using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.IssueTicket
{
    public class IssueTicketParams
    {
        //Code đặt giữ chỗ trong ReservationCode
        public string PNRCode { get; set; }
        //Ghi chú xuất vé
        public string Remark { get; set; }
        public string AirlineCode { get; set; }
        //Mặt vé(dùng lưu trữ thông tin cho kế toán)
        public string BookingInfo { get; set; }

    }
}
