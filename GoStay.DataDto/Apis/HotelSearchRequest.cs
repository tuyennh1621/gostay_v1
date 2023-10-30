using System.Web.Mvc;

namespace GoStay.DataDto.Apis
{
    public class HotelSearchRequest
    {
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }

        public List<int>? Ratings { get; set; }
        public List<int>? Criterion { get; set; }

        public int IdTinhThanh { get; set; }
        public List<int>? IdQuans { get; set; }
        public List<int>? IdPhuong { get; set; }
        public List<int>? Types { get; set; }
        public double? ReviewScore { get; set; }
        public List<int>? ServicesHotel { get; set; }
        public List<int>? ServicesRoom { get; set; }

        public int? Palletbed { get; set; }
        public int? NumMature { get; set; }
        public int? NumChild { get; set; }
        public int? NumRoom { get; set; }

        public DateTime? CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class HotelSearchParaRequest
    {
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public int? Palletbed { get; set; }
        public int? NumMature { get; set; }
        public int? NumChild { get; set; }
        public string? CheckinDate { get; set; }
        public string? CheckoutDate { get; set; }
        public List<SelectListItem>? Rating { get; set; }
        public List<SelectListItem>? Criterion { get; set; }

        public List<SelectListItem>? Quans { get; set; }
        public List<SelectListItem>? Types { get; set; }
        public List<SelectListItem>? ServiceHotel { get; set; }
        public List<SelectListItem>? ServiceRoom { get; set; }
        public List<SelectListItem>? ReviewScore { get; set; }
        public int? IdPhuong { get; set; }

        public int? IDTinhthanh { get; set; }
        public string? TenTT { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public List<SelectListItem>? QuansChecked
        {
            get
            {
                if (Quans != null)
                {
                    return Quans.Where(s => s.Selected == true).Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value,
                        Text = s.Text,

                    }).ToList();
                }
                else return null;

            }
        }

        public string TenQuans
        {
            get
            {
                if (QuansChecked != null && QuansChecked.Count > 0)
                {
                    return "(" + string.Join(", ", QuansChecked.Select(kv => kv.Text)) + ")";
                }
                else return "";

            }
        }


    }
    public class RatingDto
    {
        public int? RatingValue { get; set; }
        public string RatingChar { get; set; }
    }

    public class Criterion
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string ValueEng { get; set; }
        public string ValueChi { get; set; }
        public string? ValueLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return ValueEng;
                    case "zh-CN":
                        return ValueChi;
                }
                return Value;
            }
        }

    }
    public class ForeignTravelDto
    {
        public int? ForeignTravelValue { get; set; }
        public string ForeignTravelChar { get; set; }
        public string ForeignTravelCharEng { get; set; }
        public string ForeignTravelCharChi { get; set; }
        public string? ForeignTravelCharLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return ForeignTravelCharEng;
                    case "zh-CN":
                        return ForeignTravelCharChi;
                }
                return ForeignTravelChar;
            }
        }
    }
}
