using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air.UpdatePaymentInfo
{
    public class PaymentInfoParams
    {
        public string PaymentInfo { get; set; }
        //Thông tin xuất hóa đơn(trường hợp khách muốn xuất hoa đơn)
        public InvoiceInfo InvoiceInfo { get; set; }
        //Session chiều đi của chuyến bay
        public string DepatureFlightSession { get; set; }

    }
    //- Thông tin liên hệ
    public class InvoiceInfo
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCity { get; set; }
        //Mã số thuế VAT
        public string TaxCode { get; set; }
        //Người nhận hóa đơn
        public string CompanyStaffName { get; set; }


    }
}
