using GoStay.Common;
using GoStay.Data.Enums;

namespace GoStay.DataDto.TourDto
{
    public class SuggestTourDto
    {
        public int? IdProvince { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Id { get; set; }
        public SuggestTourType Type { get; set; }
        public string Description
        {
            get
            {
                return Type.GetEnumDescription();
            }
        }
        public string? Img { get; set; }

    }
}
