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
        static string fileNameWithPath = "pomodoro.dat";

        public static void WritePomodoro(Pomodoro pomodoro)
        {
            StringBuilder sb = CreateXML(pomodoro);

            using (StreamWriter sw = new StreamWriter(fileNameWithPath))
            {
                sw.Write(sb.ToString());
            }
        }

        private static StringBuilder CreateXML(Pomodoro pomodoro)
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
            xmlWriter.WriteString(pomodoro.PomodorosToday.ToString());
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

        public static Pomodoro ReadFromXML(Pomodoro pomodoro)
        {
            if (!File.Exists(fileNameWithPath))
            {
                // factory settings are valid
            }
            else
            {
                using (StreamReader sr = new StreamReader(fileNameWithPath))
                {
                    string fileContent = sr.ReadToEnd();
                    if(fileContent != string.Empty)
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
    }
}
