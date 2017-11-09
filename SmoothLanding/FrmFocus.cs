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
    public partial class FrmFocus : Form
    {
        #region Private Members
        Pomodoro pomodoro;
        private bool mouseDown;
        private Point lastLocation;
        string strMainFolderPath;
        string strPathSound;
        System.Media.SoundPlayer playerWorkJustCompleted;
        System.Media.SoundPlayer playerRestJustCompleted;
        System.Media.SoundPlayer playerAlert;
        List<XaramaButtonInfo> buttons;

        Font fontTimer;
        StringFormat sfCenter;
        Rectangle rectTimer;
        Brush brushTimer;

        Font fontNormal, fontSmall;
        StringFormat sfLeft;
        Brush brushNormal;

        Bitmap bmpPomodoro;

        Color insideColorWork = Color.FromArgb(255, 255, 69, 0);
        Color insideColorWorkDisabled = Color.FromArgb(30, 255, 69, 0);
        Color insideColorRest = Color.FromArgb(255, 255, 191, 135);
        Color insideColorRestDisabled = Color.FromArgb(30, 255, 191, 135);

        Color borderColorWork = Color.FromArgb(255, 255, 165, 0);
        Color borderColorWorkDisabled = Color.FromArgb(30, 255, 165, 0);
        Color borderColorRest = Color.FromArgb(255, 255, 204, 100);
        Color borderColorRestDisabled = Color.FromArgb(30, 255, 204, 100);
        #endregion

        #region Constructors
        public FrmFocus()
        {
            //strMainFolderPath = System.IO.Path.GetDirectoryName(Application.);
            strMainFolderPath = new System.IO.DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            strPathSound = strMainFolderPath + @"\sound\"; 
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void Pomodoro_OnWorkJustCompleted(object sender, Pomodoro.WorkJustCompletedArgs e)
        {
            playerWorkJustCompleted.Play();
        } 
        private void DrawPomodoros()
        {

        }
        #endregion

        #region Form Events
        private void FrmFocus_Load(object sender, EventArgs e)
        {
            pomodoro = new Pomodoro();
            pomodoro.OnWorkJustCompleted += Pomodoro_OnWorkJustCompleted;
            pomodoro.OnRestJustCompleted += Pomodoro_OnRestJustCompleted;
            pomodoro.Start();
            
            playerWorkJustCompleted = new System.Media.SoundPlayer();
            playerWorkJustCompleted.SoundLocation = strPathSound + "work-just-completed.wav";
            playerWorkJustCompleted.Load();

            playerRestJustCompleted = new System.Media.SoundPlayer();
            playerRestJustCompleted.SoundLocation = strPathSound + "rest-just-completed.wav";
            playerRestJustCompleted.Load();

            playerAlert = new System.Media.SoundPlayer();
            playerAlert.SoundLocation = strPathSound + "reminder.wav";
            playerAlert.Load();

            buttons = new List<XaramaButtonInfo>();

            XaramaButtonInfo cmdStart = XaramaButtonEngine.YellowGreenButton(XaramaButtonInfo.ContextEnum.Idea, ">", new Point(257, 10), new Size(20, 20));
            cmdStart.OnClicked += cmdStart_OnClicked;
            cmdStart.Show();

            XaramaButtonInfo cmdPause = XaramaButtonEngine.YellowGreenButton(XaramaButtonInfo.ContextEnum.Idea, "||", new Point(235, 10), new Size(20, 20));
            cmdPause.OnClicked += cmdPause_OnClicked;
            cmdPause.Show();

            buttons.Add(cmdStart);
            buttons.Add(cmdPause);

            fontTimer = DTC.GetFont(24, FontStyle.Bold);
            rectTimer = new Rectangle(0, 10, 110, 30);
            sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;
            brushTimer = new SolidBrush(Color.FromArgb(40, 40, 40));

            fontNormal = DTC.GetFont(10, FontStyle.Regular);
            fontSmall = DTC.GetFont(7, FontStyle.Regular);
            sfLeft = new StringFormat();
            sfLeft.Alignment = StringAlignment.Near;
            sfLeft.LineAlignment = StringAlignment.Near;
            brushNormal = new SolidBrush(Color.FromArgb(90, 90, 90));

            bmpPomodoro = (Bitmap)Bitmap.FromFile(@"C:\Users\besme\Desktop\SmoothLanding\SmoothLanding\images\tomato.png");
        }

        private void cmdPause_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            pomodoro.Pause();
        }
        private void cmdStart_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            pomodoro.Start();
        }
        private void Pomodoro_OnRestJustCompleted(object sender, Pomodoro.RestJustCompletedArgs e)
        {
            playerRestJustCompleted.Play();
        }

        private void timer60seconds_Tick(object sender, EventArgs e)
        {
            pomodoro.AddOneSecond();
            XMLEngine.WritePomodoro(pomodoro);
            Invalidate();
        }
      
        private void FrmFocus_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
            foreach(XaramaButtonInfo b in buttons)
                b.MouseDown(e.Location);
        }
        private void FrmFocus_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            foreach (XaramaButtonInfo b in buttons)
                b.MouseUp(e.Location);
        }
        private void FrmFocus_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
            else
            {
                bool isUpdateNeeded = false;

                foreach (XaramaButtonInfo b in buttons)
                    isUpdateNeeded = isUpdateNeeded | b.MouseMove(e.Location);
                
                if(isUpdateNeeded)
                    Invalidate();
            }
        }
        private void timerAlert_Tick(object sender, EventArgs e)
        {
            if (pomodoro.State == Pomodoro.StateEnum.WorkCompleted || pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted || pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted)
                playerAlert.Play();
        }
        private void DrawPomodorosCompleted(Graphics dc)
        {
            if(pomodoro.PomodorosToday > 0)
            {
                for(int i=0;i<pomodoro.PomodorosToday;i++)
                    dc.DrawImage(bmpPomodoro, 150 + 25 * i, 5);
            }
        }

        const int hProgress = 6;
        const int wProgressWork = 50;
        const int wProgressRest = 10;
        const int wProgressRestLong = 30;
        const int yProgress = 40;
        const int xStartProgress = 10;
        private void DrawProgressBars(Graphics dc)
        {
            int buffer = 0;

            for(int i=0;i<Pomodoro.NumSlicesForPomodoro;i++)
            {
                int slice = i+1;
                int percentageWork = 0;
                int percentageRest = 0;

                if (slice < pomodoro.SliceNow)
                {
                    percentageWork = 100;
                }
                else if (slice == pomodoro.SliceNow)
                {
                    if (pomodoro.State == Pomodoro.StateEnum.Working)
                        percentageWork = pomodoro.GetPercentage();
                    else
                        percentageWork = 100;
                }
                else
                {
                    percentageWork = 0;
                }

                if (slice < pomodoro.SliceNow)
                {
                    percentageRest = 100;
                }
                else if (slice == pomodoro.SliceNow)
                {
                    if (pomodoro.State == Pomodoro.StateEnum.RestingLong || pomodoro.State == Pomodoro.StateEnum.RestingShort)
                        percentageRest = pomodoro.GetPercentage();
                    else if (pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted || pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted)
                        percentageRest = 100;
                    else
                        percentageRest = 0;
                }
                else
                {
                    percentageRest = 0;
                }

                int xPosWork = xStartProgress + (wProgressWork + wProgressRest) * i + buffer;
                Color colBorder, colInside;
                if(percentageWork == 0)
                {
                    colBorder = borderColorWorkDisabled;
                    colInside = insideColorWorkDisabled;
                }
                else
                {
                    colBorder = borderColorWork;
                    colInside = insideColorWork;
                }
                DTC.Drawing.DrawRoundedProgress(dc, percentageWork, new Rectangle(xPosWork, yProgress, wProgressWork, hProgress), colInside, colBorder);
                buffer++;

                if (percentageRest == 0)
                {
                    colBorder = borderColorRestDisabled;
                    colInside = insideColorRestDisabled;
                }
                else
                {
                    colBorder = borderColorRest;
                    colInside = insideColorRest;
                }
                int xPosRest = xStartProgress + wProgressWork * (i + 1) + 10 * i + buffer;
                int w = wProgressRest;
                if (slice == Pomodoro.NumSlicesForPomodoro)
                    w = wProgressRestLong;
                DTC.Drawing.DrawRoundedProgress(dc, percentageRest, new Rectangle(xPosRest, yProgress, w, hProgress), colInside, colBorder);
                buffer++;
            }
            
        }
        #endregion

        #region Overridden Form Events
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            dc.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height));
            DrawPomodorosCompleted(dc);

            foreach (XaramaButtonInfo b in buttons)
                b.Draw(dc);

            dc.DrawString(pomodoro.GetSmartDisplay(), fontTimer, brushTimer, rectTimer, sfCenter);
            dc.DrawString(pomodoro.Status.ToString(), fontSmall, brushNormal, new Rectangle(140, 27, 50, 16), sfLeft);
            dc.DrawString(pomodoro.State.ToString(), fontSmall, brushNormal, new Rectangle(200, 27, 110, 16), sfLeft);
            dc.DrawString(pomodoro.SliceNow.ToString(), fontSmall, brushNormal, new Rectangle(120, 27, 50, 16), sfLeft);

            DrawProgressBars(dc);

        }
        protected override void OnPaintBackground(PaintEventArgs e) { }
        #endregion  
    }
}

