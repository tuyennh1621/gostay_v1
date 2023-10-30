namespace GoStay.DataAccess.Entities
{
    public partial class TourTopic
    {
        public TourTopic()
        {
            Tours = new HashSet<Tour>();
        }

        public byte Id { get; set; }
        public string? TourTopic1 { get; set; }
        public string? TourTopicEng { get; set; }
        public string? TourTopicChi { get; set; }
        public string? TourTopicLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TourTopicEng;
                    case "zh-CN":
                        return TourTopicChi;
                }
                return TourTopic1;
            }
        }
        public virtual ICollection<Tour> Tours { get; set; }
    }
}
