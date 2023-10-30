using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.RequestIssueTicket
{
    public class RequestIssueParams
    {
        //Mã giao dịch(trong ReservationCode lúc client gọi API đặt giữ chỗ)
        public string TransactionCode { get; set; }
        public string Note { get; set; }
    }
}
