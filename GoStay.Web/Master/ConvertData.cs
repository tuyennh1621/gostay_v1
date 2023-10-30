
namespace GoStay.Web.Master
{
    public class ConvertData
    {
        public static string ConvertNumberClick (int? click)
        {
            click = click ?? 1;
            var num = click.ToString() ;
            if (click > 999) {
                num = string.Format("{0:#,0, K}", click);
            }
            if (click > 999999)
            {
                num = string.Format("{0:0.00 '#Num.'}", click);

            }
            return num;
        }
    }
}
