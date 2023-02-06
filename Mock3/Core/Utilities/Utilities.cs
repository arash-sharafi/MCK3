using System;
using System.Globalization;

namespace Mock3.Core.Utilities
{
    public static class Utilities
    {
        public const int VoucherValidationInMonth = 6;
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

            return (year + "/" + month + "/" + day, int.Parse(year + month + day));
        }

        public static bool IsVoucherExpired(string expirationDate)
        {
            int expirationDateValue = int.Parse(expirationDate.Replace("/", ""));
            int todayDateValue = int.Parse(Utilities.Today().StringValue.Replace("/", ""));
            return todayDateValue > expirationDateValue;
        }

        public static string GetVoucherExpirationDate(string createDate, int monthsToExpire)
        {
            var createDateInt = int.Parse(
                createDate.Replace("/", string.Empty));


            int exYear = int.Parse(Convert.ToString(createDateInt).Substring(0, 4));
            int exMonth = int.Parse(Convert.ToString(createDateInt).Substring(4, 2));
            int exDay = int.Parse(Convert.ToString(createDateInt).Substring(6, 2));

            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(exYear, exMonth, exDay, pc);
            var monthsLater = dt.AddMonths(monthsToExpire);

            string expirationDate = pc.GetYear(monthsLater).ToString()
                                    + "/"
                                    + (pc.GetMonth(monthsLater).ToString().Length == 2 ?
                                        pc.GetMonth(monthsLater).ToString() :
                                        "0" + pc.GetMonth(monthsLater).ToString())
                                    + "/"
                                    + (pc.GetDayOfMonth(monthsLater).ToString().Length == 2 ?
                                        pc.GetDayOfMonth(monthsLater).ToString() :
                                        "0" + pc.GetDayOfMonth(monthsLater).ToString());

            return expirationDate;
        }

    }
}