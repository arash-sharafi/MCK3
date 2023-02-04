using System;
using System.Globalization;

namespace Mock3.Core.Utilities
{
    public static class Utilities
    {
        public static (string StringValue, int IntigerValue) Today()
        {
            var persian = new PersianCalendar();

            var year = persian.GetYear(DateTime.Now).ToString();
            string month;
            string day;

            if (persian.GetMonth(DateTime.Now) < 10)
            {
                month = "0" + persian.GetMonth(DateTime.Now).ToString();
            }
            else
            {
                month = persian.GetMonth(DateTime.Now).ToString();
            }

            if (persian.GetDayOfMonth(DateTime.Now) < 10)
            {
                day = "0" + persian.GetDayOfMonth(DateTime.Now).ToString();
            }
            else
            {
                day = persian.GetDayOfMonth(DateTime.Now).ToString();
            }

            return (year + "/" + month + "/" + day, Int32.Parse(year + month + day));
        }

    }
}