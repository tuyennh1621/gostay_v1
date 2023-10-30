namespace GoStay.DataDto.Service
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public int? IdStyle { get; set; }
        public string? Name { get; set; }
        public int IdHotelorRoom { get; set; }
        public string? Icon { get; set; }
    }

    public class ServicesDto
    {
        public string[] IdServices { get; set; }
        public string? Name { get; set; }
        public int IdHotelorRoom { get; set; }
        public string? Icon { get; set; }
    }
    public class ServiceSearchDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int AdvantageLevel { get; set; }
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

    }
}
