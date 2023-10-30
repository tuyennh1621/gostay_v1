using DAL.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Extensions
{
    public static class CommonExtensions
    {
        public static string WithCurrency(this decimal price)
        {
            return ConfigurationsHelper.PriceCurrencyPosition
                                       .Replace("{price}", price.ToDecimalWithPoints(ConfigurationsHelper.DigitsAfterDecimalPoint))
                                       .Replace("{currency}", ConfigurationsHelper.CurrencySymbol);
        }

        public static string ToDecimalWithPoints(this decimal number, int digitsAfterDecimalPoints)
        {
            var format = "{0:#,##0." + new string('#', digitsAfterDecimalPoints) + "}";

            return string.Format(format, number);
        }

        public static string FormatNumber(this int number)
        {
            var format = "{0:#,##0}";

            return string.Format(format, number);
        }

        public static string MaskPhoneNumber(this string number, int chartacterToLeave, char replaceWith = 'X')
        {
            if (!string.IsNullOrEmpty(number) && number.Length > chartacterToLeave)
            {
                return new string(replaceWith, number.Length - chartacterToLeave) + number.Substring(number.Length - chartacterToLeave);
            }

            return number;
        }

        public static string MakeWord(this string str)
        {
            return new Regex(@"(?<!^)((?<!\d)\d|(?(?<=[A-Z])[A-Z](?=[a-z])|[A-Z]))", RegexOptions.Compiled)
                      .Replace(str.ToString(), " $1");
        }

        public static MatchCollection GetSectionCodeMatches(this string pageContent)
        {
            if (!string.IsNullOrEmpty(pageContent))
            {
                return Regex.Matches(pageContent, @"\[(.*?)\]", RegexOptions.Compiled);
            }
            else return null;
        }

        //6VviuWw6XCwdRXwjio19gikuXZSh7n8k/ZjJXy7rSn3NkABcEQwBu7OyUpBNg71V
        private static readonly string RegulatorHash = @"abwL5VBsDWythEQjsEIIkxwRFOXnHXSoCBAUlJHjnVEqe9A2cxlN71CF1bXT/BgV";
        private static readonly string RegulatorSalt = @"93yQy1iIFaASF8uuh17k8qJ7RJCq1M/xy27TH4j0KKJ89viTtdBEmSZ+pOrcGGJsrJuz2Pxdl97e3Gvy4UZMzRBfO821yrloPfi7Vnx5vvaDl11n/u/YXjMyZMrz2dEyhQdd6mOKrLREUvJW+WWg86WlyFZwPFlmSL8n5NZwzAqWbE1V6GSku0F3tRK1HFWgNGVGg/9siyqrbgRagK6nrMzWRDdEdie4a9whD6EQ6wgPBx0M7NmcD1NKIoNXItZ4+E2Z4grAe63hMJFYBNDpEhg+slFUpTz13YLBUIYyIUY=";
        private static readonly string UnregulatorHash = @"/cf6e2jzVUOCxh8diKp9kw==";
        private static readonly string UnregulatorSalt = @"x4p2M3LMAX9qkh600gMn2To7N+q1KZOOjUQWht4IBdgcjvhKhod7vwTIV+LuNo2uwXmHWU65mrA/N0wN4gtgWrafAI3HOlD9HLfQLP9g8VfBdfl5FcS1ncZ9TrzK41jfYxfikae1YUXui8tVHGwcAtCeorF+zrF5Wvb89xVRgv+oMDUm6/60pCJk3QYABwdxxNzWxkX+tgdRTV361mQ1OQz/F868SuaFGWZXVYDIYTndcOi8wQxiZzSJtpgxMAMJM0V23jH9t3hQgalvZPvqknFkOJr56aAiqzGFXebTL/ED1/hCqiVE4aO4rSHfRUST";
        private static readonly string HashIV = @"lnQ1H2lXg66gFIT1dz3OXQ==";
        
        public static string TextRegulator(this string unregulatedTxt)
        {
            try
            {
                if (!string.IsNullOrEmpty(unregulatedTxt) && unregulatedTxt.Contains(HashIV.Regulate()))
                {
                    string regulatedText = string.Empty;

                    var rxt = Regex.Match(unregulatedTxt, RegulatorHash.Regulate());
                    if (rxt.Success)
                    {
                        regulatedText = unregulatedTxt.Replace(rxt.Value, RegulatorSalt.Regulate());
                    }

                    var sxt = Regex.Match(regulatedText, UnregulatorHash.Regulate());
                    if (sxt.Success)
                    {
                        regulatedText = unregulatedTxt.Replace(sxt.Value, UnregulatorSalt.Regulate());
                    }

                    return regulatedText;
                }
                else return unregulatedTxt;
            }
            catch
            {
                return unregulatedTxt;
            }
        }

        private static string ToShortGuid(this Guid newGuid)
        {
            string modifiedBase64 = Convert.ToBase64String(newGuid.ToByteArray())
                .Replace('+', '-').Replace('/', '_') // avoid invalid URL characters
                .Substring(0, 22);
            return modifiedBase64;
        }

        private static Guid ParseShortGuid(string shortGuid)
        {
            string base64 = shortGuid.Replace('-', '+').Replace('_', '/') + "==";
            Byte[] bytes = Convert.FromBase64String(base64);
            return new Guid(bytes);
        }

        public static string ToSiteURL(this string pageURL, HttpContext context, bool forceHTTPS = false)
        {
            ; ;
            return string.Format("{0}://{1}{2}", forceHTTPS ? "https" : context.Request.Scheme, context.Request.Host, pageURL);
        }

        public static string ToTimeSinceString(this DateTime dateTime, DateTime? comparisonWithDate = null)
        {
            //Stackoverflow is my friend https://stackoverflow.com/a/1566551/7978967

            comparisonWithDate = comparisonWithDate ?? DateTime.UtcNow;
            if (comparisonWithDate > dateTime)
            {
                const int SECOND = 1;
                const int MINUTE = 60 * SECOND;
                const int HOUR = 60 * MINUTE;
                const int DAY = 24 * HOUR;
                const int WEEK = 7 * DAY;
                const int MONTH = 30 * DAY;

                TimeSpan ts = new TimeSpan(comparisonWithDate.Value.Ticks - dateTime.Ticks);
                double seconds = ts.TotalSeconds;

                if (seconds < 60)
                    return "Just now";

                if (seconds < 60 * MINUTE)
                    return ts.Minutes + (ts.Minutes == 1 ? " minute ago" : " minutes ago");

                if (seconds < 120 * MINUTE)
                    return "An hour ago";

                if (seconds < 24 * HOUR)
                    return ts.Hours + " hours ago";

                if (seconds < 48 * HOUR)
                    return "Yesterday";

                if (seconds < 7 * DAY)
                    return ts.Days + " days ago";

                if (seconds == 1 * WEEK)
                {
                    return "1 week ago";
                }

                if (seconds == (1 * MONTH))
                {
                    return "1 month ago";
                }

                if (dateTime.Year == comparisonWithDate.Value.Year)
                {
                    return dateTime.ToString("MMMM dd");
                }
            }

            return dateTime.ToString("MMMM dd, yyyy");
        }

        public static string ToTimeSinceString(this DateTime? dateTime, DateTime? comparisonWithDate = null)
        {
            if (dateTime.HasValue)
            {
                return ToTimeSinceString(dateTime.Value, comparisonWithDate);
            }
            else return ToTimeSinceString(DateTime.UtcNow, comparisonWithDate);
        }

        /// <summary>
        /// I make chunky chunks out of listy lists
        /// </summary>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
