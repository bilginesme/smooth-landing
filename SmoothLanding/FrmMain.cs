﻿using System;
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
    public partial class FrmMain : Form
    {
        enum FormStateEnum { Compact, CompactWithImage }

        #region Private Members
        Pomodoro pomodoro;

        Dictionary<FormStateEnum, Size> formSizes;
        FormStateEnum formState;
        List<Bitmap> bmpVistas;
        int numVista;

        private bool mouseDown;
        private Point lastLocation;
        string strMainFolderPath;
        string strPathSound;
        System.Media.SoundPlayer playerWorkJustCompleted, playerRestJustCompleted, playerAlert, playerClick;
        List<XaramaButtonInfo> buttons;

        Font fontTimer;
        StringFormat sfCenter;
        Rectangle rectTimer;
        Brush brushTimer;

        Font fontNormal, fontSmall, fontTiny; 
        StringFormat sfLeft;
        Brush brushNormal;

        Bitmap bmpPomodoro;

        Color borderColor = Color.FromArgb(60, 63, 153, 41);

        Color insideColorWork = Color.FromArgb(255, 255, 69, 0);
        Color insideColorWorkDisabled = Color.FromArgb(30, 255, 69, 0);
        Color insideColorRest = Color.FromArgb(255, 255, 191, 135);
        Color insideColorRestDisabled = Color.FromArgb(30, 255, 191, 135);

        Color borderColorWork = Color.FromArgb(255, 255, 165, 0);
        Color borderColorWorkDisabled = Color.FromArgb(30, 255, 165, 0);
        Color borderColorRest = Color.FromArgb(255, 255, 204, 100);
        Color borderColorRestDisabled = Color.FromArgb(30, 255, 204, 100);

        const int hProgress = 6;
        const int wProgressWork = 50;
        const int wProgressRest = 10;
        const int wProgressRestLong = 30;
        const int yProgress = 40;
        const int xStartProgress = 10;
        #endregion

        #region Constructors
        public FrmMain()
        {
            //strMainFolderPath = System.IO.Path.GetDirectoryName(Application.);
            strMainFolderPath = new System.IO.DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            strPathSound = strMainFolderPath + @"\sound\";


            formState = FormStateEnum.Compact;
            formSizes = new Dictionary<FormStateEnum, Size>();
            formSizes.Add(FormStateEnum.Compact, new Size(283, 48));
            formSizes.Add(FormStateEnum.CompactWithImage, new Size(283, 145));

            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void DisplayBaloon()
        {
            string strMessage = string.Empty;
            if (pomodoro.State == Pomodoro.StateEnum.WorkCompleted)
            {
                if(pomodoro.SliceNow == 4)
                    strMessage = "Now you can have a larger break of 15 minutes";
                else
                    strMessage = "Time to rest for 5 minutes";
            }
            else if (pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted)
            {
                strMessage = "OK! Rest is over, back to work.";
            }
            else if (pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted)
            {
                strMessage = "Rest is over.\nIf you have enough time to handle another pomodoro, go for it!";
            }

            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = "Smooth Landing", 
                Icon = System.Drawing.SystemIcons.Application,
                // optional - BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                // optional - BalloonTipTitle = "My Title",
                BalloonTipText = strMessage,
            };

            // Display for 5 seconds.
            notification.ShowBalloonTip(5000);

            // This will let the balloon close after it's 5 second timeout
            // for demonstration purposes. Comment this out to see what happens
            // when dispose is called while a balloon is still visible.
            //Thread.Sleep(10000);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            //notification.Dispose();
        }
        private void Pomodoro_OnPaused(object sender, Pomodoro.PausedArgs e)
        {
            if (buttons != null)
            {
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Pause).Hide();
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Start).Show();
            }
        }
        private void Pomodoro_OnStarted(object sender, Pomodoro.StartedArgs e)
        {
            if (buttons != null)
            {
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Start).Hide();
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Pause).Show();
            }
        }
        private void Pomodoro_OnWorkJustCompleted(object sender, Pomodoro.WorkJustCompletedArgs e)
        {
            DisplayBaloon();
            playerWorkJustCompleted.Play();
        }
        private void Pomodoro_OnRestJustCompleted(object sender, Pomodoro.RestJustCompletedArgs e)
        {
            DisplayBaloon();
            playerRestJustCompleted.Play();
        }
        private void DrawPomodorosCompleted(Graphics dc)
        {
            if (pomodoro.PomodorosToday > 0)
            {
                for (int i = 0; i < pomodoro.PomodorosToday; i++)
                    dc.DrawImage(bmpPomodoro, 150 + 25 * i, 5);
            }
        }
        private void DrawProgressBars(Graphics dc)
        {
            int buffer = 0;

            for (int i = 0; i < Pomodoro.NumSlicesForPomodoro; i++)
            {
                int slice = i + 1;
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
                if (percentageWork == 0)
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
        private void GetVistaImages()
        {
            bmpVistas = new List<Bitmap>();
            numVista = 0;

            foreach(string strFile in System.IO.Directory.EnumerateFiles(@"C:\Users\besme\Desktop\SmoothLanding\SmoothLanding\images\vista"))
            {
                Bitmap bmpVista = (Bitmap)Bitmap.FromFile(strFile);
                bmpVistas.Add(bmpVista);
            }
            
        }
        private void InitPomodoro()
        {
            pomodoro = new Pomodoro();
            pomodoro.OnWorkJustCompleted += Pomodoro_OnWorkJustCompleted;
            pomodoro.OnRestJustCompleted += Pomodoro_OnRestJustCompleted;
            pomodoro.OnStarted += Pomodoro_OnStarted;
            pomodoro.OnPaused += Pomodoro_OnPaused;
            pomodoro.Pause();
        }
        #endregion

        #region Form Events
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //pomodoro = new Pomodoro();
            InitPomodoro();
            pomodoro = XMLEngine.ReadFromXML(pomodoro);
            pomodoro.Pause();
            
            playerWorkJustCompleted = new System.Media.SoundPlayer();
            playerWorkJustCompleted.SoundLocation = strPathSound + "work-just-completed.wav";
            playerWorkJustCompleted.Load();

            playerRestJustCompleted = new System.Media.SoundPlayer();
            playerRestJustCompleted.SoundLocation = strPathSound + "rest-just-completed.wav";
            playerRestJustCompleted.Load();

            playerAlert = new System.Media.SoundPlayer();
            playerAlert.SoundLocation = strPathSound + "reminder.wav";
            playerAlert.Load();

            playerClick = new System.Media.SoundPlayer();
            playerClick.SoundLocation = strPathSound + "GlossyClick.wav";

            playerAlert.Load();
            buttons = new List<XaramaButtonInfo>();

            XaramaButtonInfo cmdStart = XaramaButtonEngine.YellowGreenButton(XaramaButtonInfo.ContextEnum.Start, ">", new Point(247, 4), new Size(30, 30));
            cmdStart.OnClicked += cmdStart_OnClicked;
            cmdStart.Hide();

            XaramaButtonInfo cmdPause = XaramaButtonEngine.YellowGreenButton(XaramaButtonInfo.ContextEnum.Pause, "||", new Point(247, 4), new Size(30, 30));
            cmdPause.OnClicked += cmdPause_OnClicked;
            cmdPause.Hide();

            buttons.Add(cmdStart);
            buttons.Add(cmdPause);

            pomodoro.Pause();

            fontTimer = DTC.GetFont(24, FontStyle.Bold);
            rectTimer = new Rectangle(0, 10, 110, 30);
            sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;
            brushTimer = new SolidBrush(Color.FromArgb(40, 40, 40));

            fontNormal = DTC.GetFont(10, FontStyle.Regular);
            fontSmall = DTC.GetFont(7, FontStyle.Regular);
            fontTiny = DTC.GetFont(6, FontStyle.Regular);
            sfLeft = new StringFormat();
            sfLeft.Alignment = StringAlignment.Near;
            sfLeft.LineAlignment = StringAlignment.Near;
            brushNormal = new SolidBrush(Color.FromArgb(90, 90, 90));

            bmpPomodoro = (Bitmap)Bitmap.FromFile(@"C:\Users\besme\Desktop\SmoothLanding\SmoothLanding\images\tomato.png");

            GetVistaImages();
        }
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
            foreach (XaramaButtonInfo b in buttons)
                b.MouseDown(e.Location);
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            foreach (XaramaButtonInfo b in buttons)
                b.MouseUp(e.Location);

            if (e.Button == MouseButtons.Right)
                ctxGeneral.Show(this, e.X, e.Y);
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
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

                if (isUpdateNeeded)
                    Invalidate();
            }
        }
        private void FrmMain_DoubleClick(object sender, EventArgs e)
        {
            if (formState == FormStateEnum.Compact)
            {
                formState = FormStateEnum.CompactWithImage;
                numVista++;
                if (numVista >= bmpVistas.Count)
                    numVista = 0;
            }
            else
            {
                formState = FormStateEnum.Compact;
            }

            this.Size = formSizes[formState];
        }
        private void cmdPause_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            playerClick.Play();
            pomodoro.Pause();
        }
        private void cmdStart_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            playerClick.Play();
            pomodoro.Start();
        }
        private void timer60seconds_Tick(object sender, EventArgs e)
        {
            pomodoro.AddOneSecond();
            XMLEngine.WritePomodoro(pomodoro);
            Invalidate();
        }

       

        private void timerAlert_Tick(object sender, EventArgs e)
        {
            if (pomodoro.State == Pomodoro.StateEnum.WorkCompleted || pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted || pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted)
                playerAlert.Play();
        }
        private void tsInitialize_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm initialize", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                InitPomodoro();
                XMLEngine.WritePomodoro(pomodoro);
            }
        }
        private void tsClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsSkipSession_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to skip this session?", "Confirm initialize", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (!pomodoro.SkipSession())
                    MessageBox.Show("Cannot skip working sessions for now.");
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
            //dc.DrawString(pomodoro.Status.ToString(), fontSmall, brushNormal, new Rectangle(140, 27, 50, 16), sfLeft);
            //dc.DrawString(pomodoro.State.ToString(), fontSmall, brushNormal, new Rectangle(200, 27, 110, 16), sfLeft);
            //dc.DrawString(pomodoro.SliceNow.ToString(), fontSmall, brushNormal, new Rectangle(120, 27, 50, 16), sfLeft);
            if(pomodoro.State == Pomodoro.StateEnum.WorkCompleted || pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted || pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted)
                dc.DrawString("COMPLETED", fontTiny, brushNormal, new Rectangle(30, 2, 50, 10), sfLeft);

            DrawProgressBars(dc);

            if(formState == FormStateEnum.CompactWithImage)
            {
                Rectangle rectVista = new Rectangle(2, formSizes[FormStateEnum.Compact].Height + 2, 
                    formSizes[FormStateEnum.CompactWithImage].Width - 4, 
                    formSizes[FormStateEnum.CompactWithImage].Height - formSizes[FormStateEnum.Compact].Height - 4);
                dc.DrawImage(bmpVistas[numVista], rectVista);
            }

            Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height-1);
            dc.DrawRectangle(new Pen(borderColor, 1), rectBorder);
        }
        protected override void OnPaintBackground(PaintEventArgs e) { }
        #endregion  
    }
}
