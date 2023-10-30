using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.OpenPNR
{
    public class PNRParams
    {
        //Code đặt giữ chỗ trong ReservationCode
        public string PNRCode { get; set; }
        //Phiên dữ liệu group trong INTFlightGroup
        public string AirlineCode { get; set; }

    }
    public class PNRResponse
    {
        //Mặt vé dạng HTML
        public string PNRContent { get; set; }

    }
}
