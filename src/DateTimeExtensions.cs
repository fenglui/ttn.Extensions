﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace System
{
    public enum AceptLangOfDateTimeExtensions
    {
        eng = 0,
        zh
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeExtensions
    {
        public static string RelativeDate(this DateTime date, AceptLangOfDateTimeExtensions acceptLang)
        {
            var timespan = DateTime.Now.Subtract(date);


            if (timespan.Days >= 1)
            {
                switch (acceptLang)
                {
                    default /* otherwise */:
                        return date.ToString();
                    case AceptLangOfDateTimeExtensions.zh /* chinese */:
                        return date.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            if (timespan.Hours >= 1)
            {
                string tpl = "Over {0} hours ago.";

                switch (acceptLang)
                {
                    case AceptLangOfDateTimeExtensions.zh /* chinese */:
                        tpl = "{0}小时以前。";
                        break;
                }

                return string.Format(tpl, timespan.Hours);
            }

            if (timespan.Minutes >= 1)
            {
                string tpl = "Over {0} minutes ago.";

                switch (acceptLang)
                {
                    case AceptLangOfDateTimeExtensions.zh /* chinese */:
                        tpl = "{0}分钟以前。";
                        break;
                }

                return string.Format(tpl, timespan.Minutes);
            }
            else
            {
                string tpl = "{0} seconds ago.";

                switch (acceptLang)
                {
                    case AceptLangOfDateTimeExtensions.zh /* chinese */:
                        tpl = "{0}秒以前。";
                        break;
                }

                return string.Format(tpl, timespan.Seconds);
            }
        }

        public static int DaysLeft(this DateTime date)
        {
            return date.Subtract(DateTime.Today).Days;
        }

        public static int HolidayDaysLeft(int Month, int Day)
        {
            int Year = DateTime.Today.Year;

            var xdate = new DateTime(Year, Month, Day);

            if (xdate <= DateTime.Today.Date)
            {
                xdate = xdate.AddYears(1);
            }

            return xdate.DaysLeft();
        }

        public static DateTime GetDate(int year, int month, DayOfWeek dayOfWeek, int weekOfMonth)
        {
            // TODO: some range checking (>0, for example)
            DateTime day = new DateTime(year, month, 1);
            while (day.DayOfWeek != dayOfWeek) day = day.AddDays(1);
            if (weekOfMonth > 0)
            {
                return day.AddDays(7 * (weekOfMonth - 1));
            }
            else
            { // treat as last
                DateTime last = day;
                while ((day = day.AddDays(7)).Month == last.Month)
                {
                    last = day;
                }
                return last;
            }
        }

        public static string GetExtDateStr(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("mm.dd.yyyy");
            }

            return String.Empty;
        }
    }
}
