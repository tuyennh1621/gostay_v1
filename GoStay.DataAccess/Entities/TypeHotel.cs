using GoStay.DataAccess.Base;

namespace GoStay.DataAccess.Entities
{
    public partial class TypeHotel : BaseEntity
    {
        public TypeHotel()
        {
            Hotels = new HashSet<Hotel>();
        }
        public string? Type { get; set; }
        public string? TypeEng { get; set; }
        public string? TypeChi { get; set; }
        public string? TypeLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return TypeEng;
                    case "zh-CN":
                        return TypeChi;
                }
                return Type;
            }
        }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
