using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class ParamSelectDateRoom
    {
        public int IdRoom { get; set; }
        public int IdHotel { get; set; }
        public int BasePrice { get; set; }
        public int MinNight { get; set; }

    }
}
