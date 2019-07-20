using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothLandingUWP
{
    public class SettingsInfo
    {
        public enum VistaTypeEnum { Image, HTML5 }

        public int DayEndHour { get; set; }
        public int DayEndMinute { get; set; }
        public VistaTypeEnum VistaType { get; set; }
        public int NumAnimation { get; set; }
        private string animationFolderPrefix = "animation";

        public SettingsInfo()
        {
            DayEndHour = 6;
            DayEndMinute = 55;
            VistaType = VistaTypeEnum.HTML5;
            NumAnimation = 1;
        }
        
        public bool IsItAfterDayEndNow()
        {
            DateTime dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DayEndHour, DayEndMinute, 0);
            return DateTime.Now > dtEnd;
        }

        public string GetCurrentAnimationFolder()
        {
            return animationFolderPrefix + DTC.GetDoubleDigit(NumAnimation);
        }
        public string GetBlankAnimationFolder()
        {
            return animationFolderPrefix + DTC.GetDoubleDigit(0);
        }
    }
}
