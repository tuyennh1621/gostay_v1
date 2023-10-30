namespace GoStay.DataDto.ServiceDto
{
    public class ServiceDetailHotelDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string? NameEng { get; set; }
        public string? NameChi { get; set; }

        public string? NameLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return NameEng;
                    case "zh-CN":
                        return NameChi;
                }
                return Name;
            }
        }


        public byte? AdvantageLevel { get; set; }
    }
}
