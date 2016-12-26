using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMenuSkill.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Formats a date with the th, st, nd, or rd extenion.
        /// </summary>
        /// <remarks>
        /// Hat-tip: http://stackoverflow.com/a/21926632/489433
        /// </remarks>
        public static string ToStringWithSuffix(this DateTime dateTime, string format, string suffixPlaceHolder = "$")
        {
            if (format.LastIndexOf("d", StringComparison.Ordinal) == -1 || format.Count(x => x == 'd') > 2)
            {
                return dateTime.ToString(format);
            }

            string suffix;
            switch (dateTime.Day)
            {
                case 1:
                case 21:
                case 31:
                    suffix = "st";
                    break;
                case 2:
                case 22:
                    suffix = "nd";
                    break;
                case 3:
                case 23:
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }

            var formatWithSuffix = format.Insert(format.LastIndexOf("d", StringComparison.InvariantCultureIgnoreCase) + 1, suffixPlaceHolder);
            var date = dateTime.ToString(formatWithSuffix);

            return date.Replace(suffixPlaceHolder, suffix);
        }
    }
}