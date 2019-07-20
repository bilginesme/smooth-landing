using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;


namespace SmoothLandingUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        #region Private Members
        List<StatsInfo> statistics = new List<StatsInfo>();
        Pomodoro pomodoro;
        #endregion

        public StatisticsPage()
        {
            this.InitializeComponent();

            Action<List<StatsInfo>> callback = o => { object result = o; DrawStatistics(); };
            XMLEngine.GetStatisticsFromXML(statistics, callback);
        }

        #region Private Methods
        private void DrawStatistics()
        {
            theCanvas.Children.Clear();

            List<DateTime> days = new List<DateTime>();
            DateTime startDay = DateTime.MinValue;

            Uri uriRipe = new Uri("ms-appx:///Assets/tomato-normal.png");
            Uri uriUnripe = new Uri("ms-appx:///Assets/tomato-unripe.png");

            startDay = DTC.GetStartOfWeek(DateTime.Today);


            if (radioRangeLast7Days.IsChecked.Value)
                startDay = DateTime.Today.AddDays(-6);
            else if (radioRangeWeekly.IsChecked.Value)
                startDay = DTC.GetStartOfWeek(DateTime.Today);


            for (int i = 0; i < 7; i++)
                days.Add(startDay.AddDays(i));

            int heightRow = 30;
            int numOverallPomodoros = 0;
            int startX = 130, startY = 0;

            for (int d = 0; d < days.Count; d++)
            {
                DateTime theDay = days[d];
                string strDay = DTC.GetSmartDate(theDay, false);

                var txtDay = new TextBlock();
                txtDay.Text = DTC.GetSmartDate(theDay, false);
                txtDay.Height = 20;
                if (theDay == pomodoro.TheDay)
                    txtDay.FontWeight = Windows.UI.Text.FontWeights.Bold;
                theCanvas.Children.Add(txtDay);
                Canvas.SetLeft(txtDay, 30);
                Canvas.SetTop(txtDay, startY + d * heightRow - 5);

                StatsInfo stat = statistics.Find(i => i.TheDate == theDay);
                if (stat != null)
                {
                    int numPomodoros = stat.NumPomodorosRipe + stat.NumPomodorosUnripe;
                    numOverallPomodoros += numPomodoros;

                    for (int i = 0; i < numPomodoros; i++)
                    {
                        int posX, posY;

                        Image img = new Image();
                        BitmapImage bitmapImage = new BitmapImage();
                        if (i <= stat.NumPomodorosRipe - 1)
                            bitmapImage.UriSource = uriRipe;
                        else
                            bitmapImage.UriSource = uriUnripe;

                        img.Source = bitmapImage;
                        img.Width = 24;
                        img.Height = 24;

                        posX = startX + i * ((int)img.Width + 5);
                        posY = startY + d * heightRow - 5;

                        Canvas.SetLeft(img, posX);
                        Canvas.SetTop(img, posY);

                        theCanvas.Children.Add(img);
                    }
                }
            }

            //dc.FillRectangle(Brushes.Tomato, rectFooter);
            //dc.DrawString("Totally " + numOverallPomodoros + " pomodoros", fontBold, Brushes.White, rectFooter, sfCenter);
        }
       
        #endregion

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void radioRangeWeekly_Checked(object sender, RoutedEventArgs e)
        {
            if (pomodoro != null)
                DrawStatistics();
        }

        private void radioRangeLast7Days_Checked(object sender, RoutedEventArgs e)
        {
            if (pomodoro != null)
                DrawStatistics();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            pomodoro = e.Parameter as Pomodoro;
        }
    }
}
