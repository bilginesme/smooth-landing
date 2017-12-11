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
    public partial class FrmImageEnlarged : Form
    {
        public event EventHandler<LocChangedArgs> OnLocChanged;
        public class LocChangedArgs : EventArgs
        {
            public Point Loc { get; set; }
            public LocChangedArgs(Point loc) { Loc = loc; }
        }

        public event EventHandler<ImageChangedArgs> OnImageChanged;
        public class ImageChangedArgs : EventArgs
        {
            public int MinusOrPlus { get; set; }
            public ImageChangedArgs(int minusOrPlus) { MinusOrPlus = minusOrPlus; }
        }
        
        #region Private Members
        Point location;
        Bitmap bmpVista;
        Color borderColor;
        XaramaButtonInfo cmdClose, cmdNext, cmdPrevious;
        private bool mouseDown;
        private Point lastLocation;
        public const int W = 600;
        public const int H = 200;
        #endregion

        #region Constructors
        public FrmImageEnlarged(Point location, Bitmap bmpVista, Color borderColor)
        {
            this.location = location;
            this.bmpVista = bmpVista;
            this.borderColor = borderColor;

            InitializeComponent();
        }
        #endregion

        #region Form Events
        private void FrmImageEnlarged_Load(object sender, EventArgs e)
        {
            Size = new Size(W, H);
            Location = location;

            string strIconsFolder = @"C:\Users\besme\Desktop\SmoothLanding\SmoothLanding\images\";

            Bitmap bmpCloseNormal = (Bitmap)Bitmap.FromFile(strIconsFolder + "close-normal.png");
            Bitmap bmpCloseHovered = (Bitmap)Bitmap.FromFile(strIconsFolder + "close-hovered.png");
            cmdClose = new XaramaButtonInfo(bmpCloseNormal, bmpCloseHovered, bmpCloseNormal,
                new Rectangle(new Point(Size.Width - 20 - 9, 9), new Size(20, 20)),
                "Close", XaramaButtonInfo.ContextEnum.EnlargeImage);
            cmdClose.OnClicked += cmdClose_OnClicked;
            cmdClose.Hide();

            Bitmap bmpNextNormal = (Bitmap)Bitmap.FromFile(strIconsFolder + "next-normal.png");
            Bitmap bmpNextHovered = (Bitmap)Bitmap.FromFile(strIconsFolder + "next-hovered.png");
            cmdNext = new XaramaButtonInfo(bmpNextNormal, bmpNextHovered, bmpNextNormal,
                new Rectangle(new Point(Size.Width - 20 - 9, Size.Height - 20 - 9), new Size(20, 20)),
                "Next", XaramaButtonInfo.ContextEnum.NextImage);
            cmdNext.OnClicked += cmdNext_OnClicked;
            cmdNext.Hide();

            Bitmap bmpPreviousNormal = (Bitmap)Bitmap.FromFile(strIconsFolder + "previous-normal.png");
            Bitmap bmpPreviousHovered = (Bitmap)Bitmap.FromFile(strIconsFolder + "previous-hovered.png");
            cmdPrevious = new XaramaButtonInfo(bmpPreviousNormal, bmpPreviousHovered, bmpPreviousNormal,
                new Rectangle(new Point(Size.Width - 20 - 9 - 20 - 4, Size.Height - 20 - 9), new Size(20, 20)),
                "Previous", XaramaButtonInfo.ContextEnum.PreviousImage);
            cmdPrevious.OnClicked += cmdPrevious_OnClicked;
            cmdPrevious.Hide();
        }
        private void FrmImageEnlarged_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
            cmdClose.MouseDown(e.Location);
            cmdNext.MouseDown(e.Location);
            cmdPrevious.MouseDown(e.Location);
        }
        private void FrmImageEnlarged_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
                if (this.OnLocChanged != null)
                    OnLocChanged(this, new LocChangedArgs(Location));
            }
            else
            {
                bool isUpdateNeeded = false;

                isUpdateNeeded = isUpdateNeeded 
                    | cmdClose.MouseMove(e.Location)
                    | cmdNext.MouseMove(e.Location)
                    | cmdPrevious.MouseMove(e.Location);

                if (isUpdateNeeded)
                    Invalidate();
            }
        }
        private void FrmImageEnlarged_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            cmdClose.MouseUp(e.Location);
            cmdNext.MouseUp(e.Location);
            cmdPrevious.MouseUp(e.Location);
        }
        private void FrmImageEnlarged_MouseEnter(object sender, EventArgs e)
        {
            ShowButtons();
        }
        private void FrmImageEnlarged_MouseLeave(object sender, EventArgs e)
        {
            HideButtons();
        }
        private void cmdClose_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            this.Close();
        }
        private void cmdNext_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            if (this.OnImageChanged != null)
                OnImageChanged(this, new ImageChangedArgs(1));
        }
        private void cmdPrevious_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            if (this.OnImageChanged != null)
                OnImageChanged(this, new ImageChangedArgs(-1));
        }
        #endregion

        #region Private Methods
        private void HideButtons()
        {
            cmdClose.Hide();
            cmdNext.Hide();
            cmdPrevious.Hide();
            Invalidate();
        }
        private void ShowButtons()
        {
            cmdClose.Show();
            cmdNext.Show();
            cmdPrevious.Show();
            Invalidate();
        }
        #endregion

        #region Public Methods
        public void SetLocation(Point loc)
        {
            this.location = loc;
            this.Location = loc;
        }
        public void ChangeVista(Bitmap bmpVista)
        {
            this.bmpVista = bmpVista;
            Invalidate();
        } 
        #endregion

        #region Overridden Form Events
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            dc.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height));

            dc.DrawImage(bmpVista, new Rectangle(0, 0, Size.Width, Size.Height));

            Rectangle rectBorder = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            dc.DrawRectangle(new Pen(borderColor, 3), rectBorder);

            cmdClose.Draw(dc);
            cmdNext.Draw(dc);
            cmdPrevious.Draw(dc);
        }
        protected override void OnPaintBackground(PaintEventArgs e) { }
        #endregion
    }
}
