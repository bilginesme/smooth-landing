using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SmoothLandingUWP
{
    public class XMLEngine
    {
        static string fileNamePomodoro = "pomodoro.dat";
        static string fileNameStatistics = "statistics.dat";
        private enum FileTypeEnum { Pomodoro, Statistics }

        #region Public Methods
        public static async void GetPomodoroFromXML(Pomodoro pomodoro, Action<Pomodoro> callback)
        {
            string fileContent = await GetTextFromFile(FileTypeEnum.Pomodoro);

            if (fileContent != string.Empty)
            {
                XDocument xDoc = XDocument.Parse(fileContent);
                var result = from c in xDoc.Descendants("Pomodoro")
                             select new
                             {
                                 TheDay = c.Element("TheDay").Value,
                                 PomodorosToday = c.Element("PomodorosToday").Value,
                                 SliceNow = c.Element("SliceNow").Value,
                                 State = c.Element("State").Value,
                                 Minutes = c.Element("Minutes").Value,
                                 Seconds = c.Element("Seconds").Value
                             };
                foreach (var el in result)
                {
                    DateTime theDay = DTC.GetDateFromStringYYYYMMDD(el.TheDay);
                    int pomodorosToday = Convert.ToInt16(el.PomodorosToday);
                    int sliceNow = Convert.ToInt16(el.SliceNow);
                    Pomodoro.StateEnum state = (Pomodoro.StateEnum)Convert.ToInt16(el.State);
                    int minutes = Convert.ToInt16(el.Minutes);
                    int seconds = Convert.ToInt16(el.Seconds);

                    pomodoro.BackToLife(theDay, sliceNow, pomodorosToday, state, minutes, seconds);
                }
            }

            callback?.Invoke(pomodoro);
        }
        public static async Task<bool> UpdatePomodoro(Pomodoro pomodoro)
        {
            StringBuilder sb = CreatePomodoroXML(pomodoro);

            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync(fileNamePomodoro);
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, sb.ToString());
            }
            catch (Exception e)
            {

            }

            return true;
        }
        public static async void GetStatisticsFromXML(List<StatsInfo> statistics, Action<List<StatsInfo>> callback)
        {
            string fileContent = await GetTextFromFile(FileTypeEnum.Statistics);
            if (fileContent != string.Empty)
            {
                XDocument xDoc = XDocument.Parse(fileContent);
                var result = from c in xDoc.Descendants("StatInfo")
                             select new
                             {
                                 TheDate = c.Element("TheDate").Value,
                                 NumPomodorosRipe = c.Element("NumPomodorosRipe").Value,
                                 NumPomodorosUnripe = c.Element("NumPomodorosUnripe").Value
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

            callback?.Invoke(statistics);
        }
        public static async void UpdateStatistics(Pomodoro pomodoro)
        {
            List<StatsInfo> statistics = new List<StatsInfo>();

            string fileContent = await GetTextFromFile(FileTypeEnum.Statistics);
            if (fileContent != string.Empty)
            {
                XDocument xDoc = XDocument.Parse(fileContent);
                var result = from c in xDoc.Descendants("StatInfo")
                             select new
                             {
                                 TheDate = c.Element("TheDate").Value,
                                 NumPomodorosRipe = c.Element("NumPomodorosRipe").Value,
                                 NumPomodorosUnripe = c.Element("NumPomodorosUnripe").Value
                             };
                foreach (var el in result)
                {
                    DateTime theDate = DTC.GetDateFromStringYYYYMMDD(el.TheDate);
                    int numPomodorosRipe = Convert.ToInt16(el.NumPomodorosRipe);
                    int numPomodorosUnripe = Convert.ToInt16(el.NumPomodorosUnripe);

                    statistics.Add(new StatsInfo(theDate, numPomodorosRipe, numPomodorosUnripe));
                }
            }

            StatsInfo statsToday = statistics.Find(i => i.TheDate == pomodoro.TheDay);
            if (statsToday != null)
            {
                statsToday.SetStatistics(pomodoro.NumPomodorosToday, 0);
            }
            else
            {
                statsToday = new StatsInfo(pomodoro.TheDay, pomodoro.NumPomodorosToday, 0);
                statistics.Add(statsToday);
            }

            statistics = statistics.OrderByDescending(i => i.TheDate).ToList();

            await WriteStatistics(statistics);
        }
        #endregion

        #region Private Methods
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
        private static async Task<string> GetTextFromFile(FileTypeEnum fileType)
        {
            string text = string.Empty;
            string fileName = string.Empty;
            if (fileType == FileTypeEnum.Pomodoro)
                fileName = fileNamePomodoro;
            else if (fileType == FileTypeEnum.Statistics)
                fileName = fileNameStatistics;
            //Windows.Storage.ApplicationData.Current.LocalFolder.TryGetItemAsync(filename).ConfigureAwait(false);

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var sampleFile = await storageFolder.TryGetItemAsync(fileName);

            if (sampleFile != null)
            {
                Windows.Storage.StorageFile theFile = await storageFolder.GetFileAsync(fileName);

                //Windows.Storage.StorageFile theFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileName);
                text = await Windows.Storage.FileIO.ReadTextAsync(theFile);
            }
            else
            {
                await storageFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                if (fileType == FileTypeEnum.Pomodoro)
                    await UpdatePomodoro(new Pomodoro()).ConfigureAwait(false);
                else if (fileType == FileTypeEnum.Statistics)
                    await WriteStatistics(new List<StatsInfo>()).ConfigureAwait(false);

                Windows.Storage.StorageFile theFile = await storageFolder.GetFileAsync(fileName);

                text = await Windows.Storage.FileIO.ReadTextAsync(theFile);
            }

            return text;
        }
        private static async Task<bool> CreateNewPomodoroFile()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile newFile = await storageFolder.CreateFileAsync(fileNamePomodoro, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            return true;
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

            xmlWriter.WriteStartElement("TheDay");
            xmlWriter.WriteString(DTC.GetDateStringYYYYMMDD(pomodoro.TheDay));
            xmlWriter.WriteEndElement();

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

            foreach (StatsInfo stat in statistics)
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
        private static async Task<bool> WriteStatistics(List<StatsInfo> statistics)
        {
            StringBuilder sb = CreateStatisticsXML(statistics);

            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync(fileNameStatistics);
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, sb.ToString());
            }
            catch (Exception e)
            {

            }

            return true;
        } 
        #endregion
    }
}
