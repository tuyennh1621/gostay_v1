using System.Web.Mvc;

namespace GoStay.DataDto.Air
{
    public class SearchAirRequest
    {
        public int? PriceMin { get; set; }
        public int? PriceMax { get; set; }
        public string[] Directions { get; set; }
        public string[] Airlines { get; set; }
        public string[]? StartTime { get; set; }
        public string[]? ClassTk { get; set; }
        public string[]? TypeFlight { get; set; }
    }
    public class SearchAirParam
    {
        public int? PriceMin { get; set; }
        public int? PriceMax { get; set; }
        public List<SelectListItem>? Direction { get; set; }
        public List<SelectListItem>? Airlines { get; set; }
        public List<SelectListItem>? StartTime { get; set; }
        public List<SelectListItem>? ClassTk { get; set; }
        public List<SelectListItem>? TypeFlight { get; set; }
    }

    public class Airline
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class StartTime
    {
        public string Text { get; set; }
        public string TextEng { get; set; }
        public string TextChi { get; set; }
        public string? TextLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TextEng;
                    case "zh-CN":
                        return TextChi;
                }
                return Text;
            }
        }
        public string Value { get; set; }
    }
    public class TypeFlight
    {
        public string Text { get; set; }
        public string TextEng { get; set; }
        public string TextChi { get; set; }
        public string? TextLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TextEng;
                    case "zh-CN":
                        return TextChi;
                }
                return Text;
            }
        }
        public string Value { get; set; }
    }
    public class Direction
    {
        public string Text { get; set; }
        public string TextEng { get; set; }
        public string TextChi { get; set; }
        public string? TextLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TextEng;
                    case "zh-CN":
                        return TextChi;
                }
                return Text;
            }
        }
        public string Value { get; set; }
    }
    public class Class
    {
        public string Text { get; set; }
        public string TextEng { get; set; }
        public string TextChi { get; set; }
        public string? TextLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TextEng;
                    case "zh-CN":
                        return TextChi;
                }
                return Text;
            }
        }
        public string Value { get; set; }
    }

    public class OldRequest
    {
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        //public int PriceMin { get; set; }
        //public int PriceMax { get; set; }
        //public string[] Airline { get; set; }
        //public string[] StartTime { get; set; }
        //public string[] ClassTk { get; set; }
        //public string[] TypeFlight { get; set; }
    }
}
