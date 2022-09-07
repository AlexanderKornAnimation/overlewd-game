using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class TimeTools
    {
        public static bool LessTimeDiff(string toDate, TimeSpan timeDiff)
        {
            var date = DateTime.Parse(toDate);
            var now = DateTime.Now;
            return now <= date ? (date - now) < timeDiff : false;
        }

        public static bool PeriodIsActive(string startDate, string endDate)
        {
            if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                return false;

            var start = String.IsNullOrEmpty(startDate) ? default : DateTime.Parse(startDate);
            var end = String.IsNullOrEmpty(endDate) ? default : DateTime.Parse(endDate);
            var now = DateTime.Now;

            if (String.IsNullOrEmpty(startDate))
            {
                return now <= end;
            }

            if (String.IsNullOrEmpty(endDate))
            {
                return now >= start;
            }

            return now >= start && now <= end;
        }

        public static bool PeriodIsEnded(string startDate, string endDate)
        {
            if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                return false;

            var start = String.IsNullOrEmpty(startDate) ? default : DateTime.Parse(startDate);
            var end = String.IsNullOrEmpty(endDate) ? default : DateTime.Parse(endDate);
            var now = DateTime.Now;

            if (String.IsNullOrEmpty(startDate))
            {
                return now >= end;
            }

            if (String.IsNullOrEmpty(endDate))
            {
                return false;
            }

            return now >= end;
        }

        public static bool PeriodIsStarted(string startDate, string endDate)
        {
            if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                return false;

            var start = String.IsNullOrEmpty(startDate) ? default : DateTime.Parse(startDate);
            var end = String.IsNullOrEmpty(endDate) ? default : DateTime.Parse(endDate);
            var now = DateTime.Now;

            if (String.IsNullOrEmpty(startDate))
            {
                return true;
            }

            if (String.IsNullOrEmpty(endDate))
            {
                return now >= start;
            }

            return now >= start;
        }

        public static string AvailableTimeToString(string availableTime)
        {
            if (String.IsNullOrEmpty(availableTime))
                return null;

            var available = DateTime.Parse(availableTime);
            var now = DateTime.Now;
            return now < available ? TimeToString(available - now) : null;
        }

        public static string TimeToString(TimeSpan time)
        {
            var s = "";
            var sec = time.Seconds;
            var min = time.Minutes;
            var hour = time.Hours;
            var day = time.Days;

            if (day > 0)
            {
                s += day.ToString() + "d ";
                s += hour.ToString("D2") + "h ";
            }
            else if (hour > 0)
            {
                s += hour.ToString() + "h ";
                s += min.ToString("D2") + "m";
            }
            else
            {
                s += min.ToString("D2") + "m ";
                s += sec.ToString("D2") + "s";
            }

            return s;
        }
    }
}
