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

        Point location;
        Bitmap bmpVista;
        Color borderColor;
        XaramaButtonInfo cmdClose;
        private bool mouseDown;
        private Point lastLocation;
        public const int W = 600;
        public const int H = 200;

        public FrmImageEnlarged(Point location, Bitmap bmpVista, Color borderColor)
        {
            this.location = location;
            this.bmpVista = bmpVista;
            this.borderColor = borderColor;

            InitializeComponent();
        }

        public void SetLocation(Point loc)
        {
            this.location = loc;
            this.Location = loc;
        }

        #region Form Events
        private void FrmImageEnlarged_Load(object sender, EventArgs e)
        {
            Size = new Size(W, H);
            Location = location;

            cmdClose = XaramaButtonEngine.YellowGreenButton(XaramaButtonInfo.ContextEnum.Start, "X", new Point(Size.Width - 20 - 2, 2), new Size(20, 20));
            cmdClose.OnClicked += cmdClose_OnClicked;
            cmdClose.Hide();
        }
        private void FrmImageEnlarged_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
            cmdClose.MouseDown(e.Location);
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

                isUpdateNeeded = isUpdateNeeded | cmdClose.MouseMove(e.Location);
                if (isUpdateNeeded)
                    Invalidate();
            }
        }
        private void FrmImageEnlarged_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            cmdClose.MouseUp(e.Location);
        }
        private void FrmImageEnlarged_MouseEnter(object sender, EventArgs e)
        {
            cmdClose.Show();
        }
        private void FrmImageEnlarged_MouseLeave(object sender, EventArgs e)
        {
            cmdClose.Hide();
        }
        private void cmdClose_OnClicked(object sender, XaramaButtonInfo.ClickedArgs e)
        {
            this.Close();
        }
        #endregion

        #region Overridden Form Events
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            dc.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height));

            dc.DrawImage(bmpVista, new Rectangle(0, 0, Size.Width, Size.Height));

            Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            dc.DrawRectangle(new Pen(borderColor, 1), rectBorder);

            cmdClose.Draw(dc);
        }
        protected override void OnPaintBackground(PaintEventArgs e) { }

        #endregion
    }
}
