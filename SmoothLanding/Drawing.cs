using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SmoothLanding
{
    public class Drawing
    {
        public static Font GetFont(int size, FontStyle fontStyle)
        {
            return new Font(new FontFamily("Tahoma"), size, fontStyle);
        }
        public static void DrawTransparentImage(Bitmap bmpCanvas, Bitmap theImage, Point p)
        {
            Graphics dc = Graphics.FromImage(bmpCanvas);
            DrawTransparentImage(dc, theImage, p);
        }
        public static void DrawTransparentImage(Graphics g, Bitmap img, Point p)
        {
            // Get the color of a background pixel as the Pixel 0,0 
            Color TranpColor = img.GetPixel(0, 0);

            // Set the Attributes for the Transparent color
            ImageAttributes m_mattr;
            m_mattr = new ImageAttributes();
            m_mattr.SetColorKey(TranpColor, TranpColor);
            

            g.DrawImage((Image)img, new Rectangle(p.X, p.Y, img.Width, img.Height),
                 0, 0, img.Width, img.Height,
                 System.Drawing.GraphicsUnit.Pixel, m_mattr);
        }
        public static void DrawImage(Bitmap bmpCanvas, Bitmap theImage, Point p)
        {
            Graphics dc = Graphics.FromImage(bmpCanvas);
            DrawImage(dc, theImage, p);
        }
        public static void DrawImage(Graphics dc, Bitmap theImage, Point p)
        {
            dc.DrawImage(theImage, new Rectangle(p.X, p.Y, theImage.Width, theImage.Height),
                 0, 0, theImage.Width, theImage.Height, GraphicsUnit.Pixel, new ImageAttributes());

        }
        public static void DrawRoundedRect(Bitmap bmpCanvas, Rectangle rect, Color insideColor, Color borderColor)
        {
            Rectangle insideRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);

            Graphics.FromImage(bmpCanvas).FillRectangle(new SolidBrush(insideColor), insideRect);

            Pen pen = new Pen(borderColor);
            Graphics.FromImage(bmpCanvas).DrawLine(pen,
                new Point(rect.X + 1, rect.Y),
                new Point(rect.X + rect.Width - 2, rect.Y));
            Graphics.FromImage(bmpCanvas).DrawLine(pen,
                new Point(rect.X + rect.Width - 1, rect.Y + 1),
                new Point(rect.X + rect.Width - 1, rect.Y + rect.Height - 2));
            Graphics.FromImage(bmpCanvas).DrawLine(pen,
                new Point(rect.X + rect.Width - 2, rect.Y + rect.Height - 1),
                new Point(rect.X + 1, rect.Y + rect.Height - 1));
            Graphics.FromImage(bmpCanvas).DrawLine(pen,
                new Point(rect.X, rect.Y + rect.Height - 2),
                new Point(rect.X, rect.Y + 1));
        }
        public static void DrawRoundedProgress(Bitmap bmpCanvas, int percentage, Rectangle rect, Color insideColor, Color borderColor)
        {
            DrawRoundedProgress(Graphics.FromImage(bmpCanvas), percentage, rect, insideColor, borderColor);
        }
        public static void DrawRoundedProgress(Graphics dc, int percentage, Rectangle rect, Color insideColor, Color borderColor)
        {
            if (percentage > 100) percentage = 100;
            if (percentage < 0) percentage = 0;
            
            Rectangle bgRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
            dc.FillRectangle(new SolidBrush(Color.White), bgRect);

            Rectangle insideRect = new Rectangle(rect.X + 1, rect.Y + 1, percentage * rect.Width / 100 - 2, rect.Height - 2);
            if (insideRect.Width > 0 && insideRect.Height > 0)
                dc.FillRectangle(new SolidBrush(insideColor), insideRect);

            Pen pen = new Pen(borderColor);
            dc.DrawLine(pen, new Point(rect.X + 1, rect.Y), new Point(rect.X + rect.Width - 2, rect.Y));
            dc.DrawLine(pen, new Point(rect.X + rect.Width - 1, rect.Y + 1), new Point(rect.X + rect.Width - 1, rect.Y + rect.Height - 2));
            dc.DrawLine(pen, new Point(rect.X + rect.Width - 2, rect.Y + rect.Height - 1), new Point(rect.X + 1, rect.Y + rect.Height - 1));
            dc.DrawLine(pen, new Point(rect.X, rect.Y + rect.Height - 2), new Point(rect.X, rect.Y + 1));

            if (rect.Height == 3)
            {
                dc.DrawLine(pen, new Point(rect.X, rect.Y + 1), new Point(rect.X + 1, rect.Y + 1));
                dc.DrawLine(pen, new Point(rect.X + rect.Width - 2, rect.Y + 1), new Point(rect.X + rect.Width - 1, rect.Y + 1 ));
            }
        }
    }
}
