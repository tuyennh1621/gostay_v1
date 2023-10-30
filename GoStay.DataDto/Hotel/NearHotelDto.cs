using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class NearHotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public float HowFar { get; set; }
        public string Url { get; set; }

        public int IdType { get; set; }
        public string TypeName { get; set; }
        public string PriceRange { get; set; }
        public string? Slug {get;set;}
    }
}
