using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothLandingUWP
{
    public class SettingsInfo
    {
        public int DayEndHour { get; set; }
        public int DayEndMinute { get; set; }

        public SettingsInfo()
        {
            DayEndHour = 6;
            DayEndMinute = 55;
        }
        
        public bool IsItAfterDayEndNow()
        {
            DateTime dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DayEndHour, DayEndMinute, 0);
            return DateTime.Now > dtEnd;
        }
    }
}
