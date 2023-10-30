namespace GoStay.DataAccess.Entities
{
    public partial class TourStyle
    {
        public TourStyle()
        {
            Tours = new HashSet<Tour>();
        }

        public byte Id { get; set; }
        public string? TourStyle1 { get; set; }
        public string? TourStyleEng { get; set; }
        public string? TourStyleChi { get; set; }
        public string? TourStyleLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TourStyleEng;
                    case "zh-CN":
                        return TourStyleChi;
                }
                return TourStyle1;
            }
        }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
