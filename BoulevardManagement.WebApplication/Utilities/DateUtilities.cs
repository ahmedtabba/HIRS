using System;

namespace BoulevardManagement.WebApplication.Utilities
{
    public static class DateUtilities
    {
        public static string GetPrettyDate(TimeSpan s)
        {
            // 1.
            // Get time span elapsed since the date.
            //TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;
            var culture = System.Globalization.CultureInfo.CurrentCulture.ToString();

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    if (culture.Contains("ar"))
                    {
                        return "الآن";

                    }
                    else
                    {
                        return "just now";

                    }
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    if (culture.Contains("ar"))
                    {
                        return "منذ دقيقة";

                    }
                    else
                    {
                        return "1 minute ago";

                    }
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    if (culture.Contains("ar"))
                    {
                        return "منذ" + Math.Floor((double)secDiff / 60) + "دقيقة";
                    }
                    else
                    {
                        return string.Format("{0} minutes ago",
           Math.Floor((double)secDiff / 60));
                    }
       
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    if (culture.Contains("ar"))
                    {
                        return "منذ ساعة";

                    }
                    else
                    {
                        return "1 hour ago";

                    }
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    if (culture.Contains("ar"))
                    {
                        return "منذ" + Math.Floor((double)secDiff / 3600) + "ساعة";

                    }
                    else
                    {
                        return string.Format("{0} hours ago",
                      Math.Floor((double)secDiff / 3600));
                    }
                  
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                if (culture.Contains("ar"))
                {
                    return "البارحة";

                }
                else
                {
                    return "yesterday";

                }
            }
            if (dayDiff < 7)
            {
                if (culture.Contains("ar"))
                {
                    return "منذ" + Math.Floor((double)dayDiff) + "يوم";

                }
                else
                {
                    return string.Format("{0} days ago",
                                       dayDiff);
                }
               
            }
            if (dayDiff < 31)
            {
                if (culture.Contains("ar"))
                {
                    return "منذ" + Math.Floor((double)secDiff / 7) + "أسبوع";

                }
                else
                {
                    return string.Format("{0} weeks ago",
                   Math.Ceiling((double)dayDiff / 7));
                }
               
            }
            if (culture.Contains("ar"))
            {
                return "أكثر من شهر";

            }
            else
            {
                return "More than month";
            }
            
        }
    }
}