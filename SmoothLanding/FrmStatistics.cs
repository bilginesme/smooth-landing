using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmoothLanding
{
    public partial class FrmStatistics : Form
    {
        #region Private Members
        Font fontNormal, fontBold, fontSmall, fontTiny;
        StringFormat sfCenter, sfLeft;
        Brush brushNormal;
        List<StatsInfo> statistics = new List<StatsInfo>();
        Bitmap bmpPomodoroRipe, bmpPomodoroUnripe;
        Pomodoro pomodoro;
        Rectangle rectFooter;
        #endregion

        #region Constructors
        public FrmStatistics(Pomodoro pomodoro)
        {
            this.pomodoro = pomodoro;
            statistics = XMLEngine.ReadStatisticsFromXML();
            bmpPomodoroRipe = (Bitmap)Bitmap.FromFile(@"C:\Users\besme\Desktop\SmoothLanding\SmoothLanding\images\tomato-normal.png");
            bmpPomodoroUnripe = (Bitmap)Bitmap.FromFile(@"C:\Users\besme\Desktop\SmoothLanding\SmoothLanding\images\tomato-unripe.png");

            InitializeComponent();
        }
        #endregion

        #region Form Events
        private void FrmStatistics_Load(object sender, EventArgs e)
        {
            fontNormal = DTC.GetFont(10, FontStyle.Regular);
            fontBold = DTC.GetFont(10, FontStyle.Bold);
            fontSmall = DTC.GetFont(7, FontStyle.Regular);
            fontTiny = DTC.GetFont(6, FontStyle.Regular);

            sfLeft = new StringFormat();
            sfLeft.Alignment = StringAlignment.Near;
            sfLeft.LineAlignment = StringAlignment.Near;

            sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;

            brushNormal = new SolidBrush(Color.FromArgb(90, 90, 90));

            rectFooter = new Rectangle(0, ClientSize.Height - 30, ClientSize.Width, 30);
        }
        private void radioRangeLast7Days_CheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
        private void radioRangeWeekly_CheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
        #endregion

        #region Private Methods
        private void DrawDays(Graphics dc)
        {
            List<DateTime> days = new List<DateTime>();
            DateTime startDay = DateTime.MinValue;

            if (radioRangeLast7Days.Checked)
                startDay = DateTime.Today.AddDays(-6);
            else if (radioRangeWeekly.Checked)
                startDay = GetStartOfWeek();

            for (int i = 0; i < 7; i++)
                days.Add(startDay.AddDays(i));

            int heightRow = 30;
            int numOverallPomodoros = 0;
            for(int d=0;d<days.Count;d++)
            {
                DateTime theDay = days[d];
                string strDay = DTC.GetSmartDate(theDay, false);
                Rectangle rect = new Rectangle(10, 50 + heightRow * d, 100, heightRow);

                if(theDay == pomodoro.TheDate)
                    dc.FillRectangle(Brushes.FloralWhite, new Rectangle(0, rect.Top-2, this.Width, heightRow - 4));

                dc.DrawString(strDay, fontBold, brushNormal, rect, sfLeft);

                StatsInfo stat = statistics.Find(i=>i.TheDate == theDay);
                if (stat != null)
                {
                    int numPomodoros = stat.NumPomodorosRipe + stat.NumPomodorosUnripe;
                    numOverallPomodoros += numPomodoros;

                    for (int i=0;i<numPomodoros;i++)
                    {
                        int posX, posY;
                        posX = rect.Right + 10 + i * (bmpPomodoroRipe.Width + 5);
                        posY = rect.Top - 5;

                        if (i <= stat.NumPomodorosRipe-1)
                            dc.DrawImage(bmpPomodoroRipe, posX, posY);
                        else
                            dc.DrawImage(bmpPomodoroUnripe, posX, posY);
                    }
                }
            }
            
            dc.FillRectangle(Brushes.Tomato, rectFooter);
            dc.DrawString("Totally " + numOverallPomodoros + " pomodoros", fontBold, Brushes.White, rectFooter, sfCenter);
        }
        private void CreateDummyData(object sender, EventArgs e)
        {
            List<StatsInfo> statistics = new List<StatsInfo>();

            statistics.Add(new StatsInfo(new DateTime(2017, 12, 28), 2, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 27), 1, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 26), 2, 1));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 25), 0, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 24), 0, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 23), 2, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 22), 3, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 21), 2, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 20), 2, 0));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 19), 2, 1));
            statistics.Add(new StatsInfo(new DateTime(2017, 12, 18), 1, 0));

            XMLEngine.WriteStatistics(statistics);
        }
        private DateTime GetStartOfWeek()
        {
            DateTime dt = DateTime.Today;
            bool isOK = false;

            while(!isOK)
            {
                if(dt.DayOfWeek == DayOfWeek.Monday)
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
        #endregion

        #region Overridden Form Events
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            //dc.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height));

            DrawDays(dc);

            
                
        }
        #endregion
    }
}
