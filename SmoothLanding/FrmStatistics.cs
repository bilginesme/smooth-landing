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
        Font fontNormal, fontBold, fontSmall, fontTiny;
        StringFormat sfCenter, sfLeft;
        Brush brushNormal;

        #region Constructors
        public FrmStatistics()
        {
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
            DateTime today = DateTime.Today;

            if (radioRangeLast7Days.Checked)
                today = DateTime.Today;
            else if (radioRangeWeekly.Checked)
                today = DateTime.Today.StartOfWeek(DayOfWeek.Monday);

            int heightRow = 30;

            for(int d=0;d<7;d++)
            {
                DateTime theDay = today.AddDays(-d);
                string strDay = DTC.GetSmartDate(theDay, false);
                Rectangle rect = new Rectangle(10, 50 + heightRow * d, 100, heightRow);

                dc.DrawString(strDay, fontBold, brushNormal, rect, sfLeft);
            }
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
