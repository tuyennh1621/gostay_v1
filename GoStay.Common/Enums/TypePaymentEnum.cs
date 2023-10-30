using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Enums
{
    public enum TypePaymentEnum
    {
        [Description("Trả sau")]
        Postpaid = 4,
        [Description("Trả trước")]
        PrePayment = 7,

        [Description("Đặt cọc 10%")]
        Deposit  = 8,
    }
}
