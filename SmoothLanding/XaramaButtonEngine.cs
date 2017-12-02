using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SmoothLanding
{

    public class XaramaButtonEngine
    {
        public static XaramaButtonInfo Minimalist(int id, string txtButton, Rectangle rect, StringFormat sf, int fontSize, FontStyle fontStyle, Color foreColor)
        {
            return Minimalist(id, txtButton, rect.Location, rect.Size, sf, fontSize, fontStyle, foreColor);
        }
        public static XaramaButtonInfo Minimalist(int id, string txtButton, Point loc, Size size)
        {
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;

            return Minimalist(id, txtButton, loc, size, sfCenter, 7, FontStyle.Bold, Color.Black);
        }
        public static XaramaButtonInfo Minimalist(int id, string txtButton, Point loc, Size size, StringFormat sf, int fontSize, FontStyle fontStyle, Color foreColor)
        {
            XaramaButtonInfo b = new XaramaButtonInfo();

            b.ID = id;
            b.Rect = new Rectangle(loc, size);

            b.ImgNormal = new Bitmap(b.Rect.Width, b.Rect.Height);
            b.ImgHovered = new Bitmap(b.Rect.Width, b.Rect.Height);
            b.ImgDisabled = new Bitmap(b.Rect.Width, b.Rect.Height);

            Graphics.FromImage(b.ImgNormal).FillRectangle(new SolidBrush(Color.White), b.GetZeroRect());
            Graphics.FromImage(b.ImgHovered).FillRectangle(new SolidBrush(Color.White), b.GetZeroRect());
            Graphics.FromImage(b.ImgDisabled).FillRectangle(new SolidBrush(Color.White), b.GetZeroRect());

            DTC.Drawing.DrawRoundedRect(b.ImgHovered, b.GetZeroRect(), Color.LightGreen, Color.LightGray);

            Font font = DTC.GetFont(fontSize, fontStyle);


            Graphics.FromImage(b.ImgNormal).DrawString(txtButton, font, new SolidBrush(foreColor), b.GetZeroRect(), sf);
            Graphics.FromImage(b.ImgHovered).DrawString(txtButton, font, new SolidBrush(foreColor), b.GetZeroRect(), sf);
            Graphics.FromImage(b.ImgDisabled).DrawString(txtButton, font, new SolidBrush(Color.Gray), b.GetZeroRect(), sf);

            b.Enable();
        
            return b;
        }
        public static XaramaButtonInfo YellowGreenButton(XaramaButtonInfo.ContextEnum context, string txtButton, Point loc, Size size)
        {
            XaramaButtonInfo button = new XaramaButtonInfo();
            button.Context = context;
            button.ContextID = 1;
            button.Rect = new Rectangle(loc, size);

            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;

            button.ImgNormal = new Bitmap(button.GetZeroRect().Width, button.GetZeroRect().Height);
            DTC.Drawing.DrawRoundedRect(button.ImgNormal, button.GetZeroRect(), Color.LightYellow, Color.LightGray);
            Graphics.FromImage(button.ImgNormal).DrawString(txtButton, DTC.GetFont(9, FontStyle.Bold), new SolidBrush(Color.FromArgb(80, 80, 80)), button.GetZeroRect(), sfCenter);

            button.ImgHovered = new Bitmap(button.GetZeroRect().Width, button.GetZeroRect().Height);
            DTC.Drawing.DrawRoundedRect(button.ImgHovered, button.GetZeroRect(), Color.LightGreen, Color.LightGray);
            Graphics.FromImage(button.ImgHovered).DrawString(txtButton, DTC.GetFont(9, FontStyle.Bold), new SolidBrush(Color.FromArgb(90, 90, 90)), button.GetZeroRect(), sfCenter);

            button.ImgDisabled = new Bitmap(button.GetZeroRect().Width, button.GetZeroRect().Height);
            DTC.Drawing.DrawRoundedRect(button.ImgDisabled, button.GetZeroRect(), Color.Gray, Color.LightGray);
            Graphics.FromImage(button.ImgDisabled).DrawString(txtButton, DTC.GetFont(9, FontStyle.Bold), new SolidBrush(Color.FromArgb(80, 80, 80)), button.GetZeroRect(), sfCenter);

            return button;
        }
    }
}
