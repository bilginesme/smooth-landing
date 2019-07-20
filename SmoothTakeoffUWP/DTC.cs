using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SmoothLandingUWP
{
    public class DTC
    {
        public static bool IsNumeric(object input)
        {
            if (input == null) return false;

            string text = input.ToString();
            bool result = false;

            if (text.Length > 0)
            {
                char[] acceptedChars = "0123456789,.-'".ToCharArray();
                result = true; // innocent until proven guilty

                // look for the first non-numeric character in the input string
                for (int i = 0; i < text.Length; i++)
                {
                    // if the character is NOT in the list of valid characters, it is not a number
                    if (text.LastIndexOfAny(acceptedChars, i, 1) < 0)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
 
        public static string GetSmartDate(DateTime theDate, bool addYear)
        {
            string result = "";

            result = theDate.Day + " " +
                System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(theDate.Month) + " " +
                System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(theDate.DayOfWeek);
            if (addYear) result += " " + theDate.Year.ToString().Substring(2, 2);
            return result;
        }

        public static string GetDateStringYYYYMMDD(DateTime dt)
        {
            string strYear = dt.Year.ToString();
            string strMonth = dt.Month.ToString();
            if (strMonth.Length == 1) strMonth = "0" + strMonth;
            string strDay = dt.Day.ToString();
            if (strDay.Length == 1) strDay = "0" + strDay;

            return strYear + strMonth + strDay;
        }
        public static DateTime GetDateFromStringYYYYMMDD(string strDateTime)
        {
            DateTime dateTime = DateTime.MinValue;

            if (strDateTime.Length == 8)
            {
                int year = Convert.ToInt16(strDateTime.Substring(0, 4));
                int month = Convert.ToInt16(strDateTime.Substring(4, 2));
                int day = Convert.ToInt16(strDateTime.Substring(6, 2));

                dateTime = new DateTime(year, month, day);
            }

            return dateTime;
        }

        public static string GetSmartPausedTime(TimeSpan ts)
        {
            string strResult = string.Empty;

            if(ts.Hours>0)
            {
                strResult = "> 1hr";
            }
            else
            {
                string strMins = ts.Minutes.ToString();
                if (strMins.Length < 2)
                    strMins = "0" + strMins;
                strMins = strMins + ":";

                strResult = strResult + strMins;


                string strSecs = ts.Seconds.ToString();
                if (strSecs.Length < 2)
                    strSecs = "0" + strSecs;

                strResult = strResult + strSecs;
            }
            
            

            return strResult;
        }

        public static string GetSmartTrashTime(TimeSpan ts)
        {
            string strResult = string.Empty;

            string strHrs = ts.Hours.ToString();
            if (strHrs.Length < 2)
                strHrs = "0" + strHrs;
            strHrs = strHrs + ":";

            strResult = strResult + strHrs;

            string strMins = ts.Minutes.ToString();
            if (strMins.Length < 2)
                strMins = "0" + strMins;
            strMins = strMins + ":";

            strResult = strResult + strMins;


            string strSecs = ts.Seconds.ToString();
            if (strSecs.Length < 2)
                strSecs = "0" + strSecs;

            strResult = strResult + strSecs;



            return strResult;
        }

        public static DateTime GetStartOfWeek(DateTime dt)
        {
            bool isOK = false;

            while (!isOK)
            {
                if (dt.DayOfWeek == DayOfWeek.Monday)
                {
                    isOK = true;
                }
                else
                {
                    dt = dt.AddDays(-1);
                }
            }

            return dt;
        }

        public static string GetDoubleDigit(int k)
        {
            if(k >= 0 && k < 10)
            {
                return "0" + k;
            }
            else if (k >= 10 && k < 100)
            {
                return k.ToString();
            }
            else
            {
                return "00";
            }
        }
    }
}
