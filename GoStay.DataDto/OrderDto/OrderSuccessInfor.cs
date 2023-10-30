using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.OrderDto
{
    public class OrderSuccessInfor
    {
        public bool RoomOrner { get; set; }
        public bool CheckIn { get; set; }
        public string amount { get; set; }

        public OrderGetInfoDto OrderSuccessInforDto { get; set; }
    }
}
