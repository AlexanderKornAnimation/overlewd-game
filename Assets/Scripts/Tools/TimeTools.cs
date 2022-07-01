using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class TimeTools
    {
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
                s += min.ToString("D2") + "m";
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
