using GoStay.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
namespace GoStay.Common.Helper
{
    public static class ParamHelperExtension
    {
        public static string[] CheckParamUrl(this string[] param)
        {
            if (param.IsNullOrEmpty() != true)
            {
                if (param.Count() == 1 && param[0] != null)
                {
                    param = param[0].Split(',');
                }
                else
                {
                    if (param.Count() > 1)
                    { }
                    else
                    {
                        param = null;
                    }
                }
            }
            else
            {
                param = null;
            }
            return param;
        }
        public static string TranArrayIntToString(this int[]? param)
        {
            string value = "";
            for(int i=0;i<param.Count();i++)
            {
                if (i == 0)
                {
                    value = value + param[i].ToString();
                }
                else
                {
                    value = value +","+ param[i].ToString();
                }
            }
            return value;
        }
    }
}
