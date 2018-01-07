using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SmoothLanding
{
    public class XMLEngine
    {
        static string fileNameWithPathPomodoro = "pomodoro.dat";
        static string fileNameWithPathStatistics = "statistics.dat";

        private static void GetTwinParameters(string str, out int x, out int y)
        {
            x = 0;
            y = 0;
            int locX = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str.Substring(i, 1) == "x")
                {
                    locX = i;
                    break;
                }
            }

            if (locX > 0)
            {
                string str1 = "";
                string str2 = "";

                str1 = str.Substring(0, locX);
                str2 = str.Substring(locX + 1, str.Length - locX - 1);

                if (DTC.IsNumeric(str1) && DTC.IsNumeric(str2))
                {
                    x = Convert.ToInt16(str1);
                    y = Convert.ToInt16(str2);
                }
            }
        }

        public static Pomodoro ReadFromXML(Pomodoro pomodoro)
        {
            if (!File.Exists(fileNameWithPathPomodoro))
            {
                // factory settings are valid
            }
            else
            {
                using (StreamReader sr = new StreamReader(fileNameWithPathPomodoro))
                {
                    string fileContent = sr.ReadToEnd();
                    if (fileContent != string.Empty)
                    {
                        XDocument xDoc = XDocument.Parse(fileContent);
                        var result = from c in xDoc.Descendants("Pomodoro")
                                     select new
                                     {
                                         #region MyRegion
                                         PomodorosToday = c.Element("PomodorosToday").Value,
                                         SliceNow = c.Element("SliceNow").Value,
                                         State = c.Element("State").Value,
                                         Minutes = c.Element("Minutes").Value,
                                         Seconds = c.Element("Seconds").Value
                                         #endregion
                                     };
                        foreach (var el in result)
                        {
                            int pomodorosToday = Convert.ToInt16(el.PomodorosToday);
                            int sliceNow = Convert.ToInt16(el.SliceNow);
                            Pomodoro.StateEnum state = (Pomodoro.StateEnum)Convert.ToInt16(el.State);
                            int minutes = Convert.ToInt16(el.Minutes);
                            int seconds = Convert.ToInt16(el.Seconds);

                            pomodoro.BackToLife(sliceNow, pomodorosToday, state, minutes, seconds);
                        }
                    }
                    else
                    {

                    }

                }
            }



            return pomodoro;
        }
        private static StringBuilder CreatePomodoroXML(Pomodoro pomodoro)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };

            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb, xmlWriterSettings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Pomodoro");

            xmlWriter.WriteStartElement("PomodorosToday");
            xmlWriter.WriteString(pomodoro.NumPomodorosToday.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SliceNow");
            xmlWriter.WriteString(pomodoro.SliceNow.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("State");
            xmlWriter.WriteString(Convert.ToString((int)pomodoro.State));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Minutes");
            xmlWriter.WriteString(pomodoro.Minutes.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Seconds");
            xmlWriter.WriteString(pomodoro.Seconds.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();

            return sb;
        }
        public static void WritePomodoro(Pomodoro pomodoro)
        {
            StringBuilder sb = CreatePomodoroXML(pomodoro);

            using (StreamWriter sw = new StreamWriter(fileNameWithPathPomodoro))
            {
                sw.Write(sb.ToString());
            }
        }

        public static List<StatsInfo> ReadStatisticsFromXML()
        {
            List<StatsInfo> statistics = new List<StatsInfo>();

            if (!File.Exists(fileNameWithPathStatistics))
            {
                // factory settings are valid
            }
            else
            {
                using (StreamReader sr = new StreamReader(fileNameWithPathStatistics))
                {
                    string fileContent = sr.ReadToEnd();
                    if (fileContent != string.Empty)
                    {
                        XDocument xDoc = XDocument.Parse(fileContent);
                        var result = from c in xDoc.Descendants("StatInfo")
                                     select new
                                     {
                                         #region MyRegion
                                         TheDate = c.Element("TheDate").Value,
                                         NumPomodorosRipe = c.Element("NumPomodorosRipe").Value,
                                         NumPomodorosUnripe = c.Element("NumPomodorosUnripe").Value
                                         #endregion
                                     };
                        foreach (var el in result)
                        {
                            DateTime theDate = DTC.GetDateFromStringYYYYMMDD(el.TheDate);
                            int numPomodorosRipe = Convert.ToInt16(el.NumPomodorosRipe);
                            int numPomodorosUnripe = Convert.ToInt16(el.NumPomodorosUnripe);

                            statistics.Add(new StatsInfo(theDate, numPomodorosRipe, numPomodorosUnripe));
                        }
                    }
                    else
                    {

                    }

                }
            }

            return statistics;
        }
        private static StringBuilder CreateStatisticsXML(List<StatsInfo> statistics)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };

            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb, xmlWriterSettings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Statistics");

            foreach(StatsInfo stat in statistics)
            {
                xmlWriter.WriteStartElement("StatInfo");

                xmlWriter.WriteStartElement("TheDate");
                xmlWriter.WriteString(DTC.GetDateStringYYYYMMDD(stat.TheDate));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("NumPomodorosRipe");
                xmlWriter.WriteString(stat.NumPomodorosRipe.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("NumPomodorosUnripe");
                xmlWriter.WriteString(stat.NumPomodorosUnripe.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();    // StatInfo
            }

            xmlWriter.WriteEndElement();    // Statistics
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();

            return sb;
        }
        public static void WriteStatistics(List<StatsInfo> statistics)
        {
            StringBuilder sb = CreateStatisticsXML(statistics);

            using (StreamWriter sw = new StreamWriter(fileNameWithPathStatistics))
            {
                sw.Write(sb.ToString());
            }
        }
        public static void UpdateStatistics(Pomodoro pomodoro)
        {
            List<StatsInfo> statistics = ReadStatisticsFromXML();

            StatsInfo statsToday = statistics.Find(i=>i.TheDate == pomodoro.TheDate);
            if (statsToday != null)
            {
                statsToday.SetStatistics(pomodoro.NumPomodorosToday, 0);
            }
            else
            {
                statsToday = new StatsInfo(pomodoro.TheDate, pomodoro.NumPomodorosToday, 0);
                statistics.Add(statsToday);
            }

            statistics = statistics.OrderByDescending(i=>i.TheDate).ToList();

            WriteStatistics(statistics);
        }
    }
}
