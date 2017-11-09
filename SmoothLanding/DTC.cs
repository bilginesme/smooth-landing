using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace SmoothLanding
{
    public class DTC
    {
        public static bool IsNumeric(object input)
        {
            if (input == null) return false;

            string text = input.ToString();
            bool result = false;

            if (text.Length > 0)
            {
                char[] acceptedChars = "0123456789,.-'".ToCharArray();
                result = true; // innocent until proven guilty

                // look for the first non-numeric character in the input string
                for (int i = 0; i < text.Length; i++)
                {
                    // if the character is NOT in the list of valid characters, it is not a number
                    if (text.LastIndexOfAny(acceptedChars, i, 1) < 0)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public static Font GetFont(int size, FontStyle fontStyle)
        {
            return new Font(new FontFamily("Tahoma"), size, fontStyle);
        }
        public static Font GetFont(string fontName, int size, FontStyle fontStyle)
        {
            return new Font(new FontFamily(fontName), size, fontStyle);
        }
        public static Font GetFont(string strFontName, string strFontSize, string strFontStyle)
        {
            Font font;

            if (DTC.IsNumeric(strFontSize))
            {
                int fontSize = Convert.ToInt16(strFontSize);
                FontStyle fontStyle = FontStyle.Regular;
                if (strFontStyle == "Bold") fontStyle = FontStyle.Bold;
                else if (strFontStyle == "Italic") fontStyle = FontStyle.Italic;

                font = DTC.GetFont(strFontName, fontSize, fontStyle);
            }
            else
            {
                font = DTC.GetFont(11, FontStyle.Bold);
            }

            return font;
        }

        public class Drawing
        {
            public static void DrawRedRect(Graphics dc, Rectangle rect)
            {
                dc.DrawRectangle(new Pen(new SolidBrush(Color.Red)), rect);
            }
            public static Bitmap DrawTransparentImage(Bitmap bmpCanvas, Bitmap theImage, Point p)
            {
                if (theImage != null)
                {
                    // Get the color of a background pixel as the Pixel 0,0 
                    Color TranpColor = theImage.GetPixel(0, 0);

                    // Set the Attributes for the Transparent color
                    ImageAttributes m_mattr;
                    m_mattr = new ImageAttributes();
                    m_mattr.SetColorKey(TranpColor, TranpColor);

                    Graphics.FromImage(bmpCanvas).DrawImage(theImage, new Rectangle(p.X, p.Y, theImage.Width, theImage.Height),
                         0, 0, theImage.Width, theImage.Height,
                         System.Drawing.GraphicsUnit.Pixel, m_mattr);
                }

                return bmpCanvas;
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
                DrawImage(Graphics.FromImage(bmpCanvas), theImage, p);
            }
            public static void DrawImage(Graphics dc, Bitmap theImage, Point p)
            {
                dc.DrawImage(theImage, new Rectangle(p.X, p.Y, theImage.Width, theImage.Height),
                     0, 0, theImage.Width, theImage.Height, GraphicsUnit.Pixel, new ImageAttributes());
            }
            public static void DrawRoundedRect(Bitmap bmpCanvas, Rectangle rect, Color insideColor, Color borderColor)
            {
                if (bmpCanvas == null)
                {
                    bmpCanvas = new Bitmap(rect.Width, rect.Height);
                    Graphics.FromImage(bmpCanvas).FillRectangle(new SolidBrush(Color.White), rect);
                }

                DrawRoundedRect(Graphics.FromImage(bmpCanvas), rect, insideColor, borderColor);
            }
            public static void DrawRoundedRect(Graphics dc, Rectangle rect, Color insideColor, Color borderColor)
            {
                Rectangle insideRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);

                dc.FillRectangle(new SolidBrush(insideColor), insideRect);

                Pen pen = new Pen(borderColor);
                dc.DrawLine(pen,
                    new Point(rect.X + 1, rect.Y),
                    new Point(rect.X + rect.Width - 2, rect.Y));
                dc.DrawLine(pen,
                    new Point(rect.X + rect.Width - 1, rect.Y + 1),
                    new Point(rect.X + rect.Width - 1, rect.Y + rect.Height - 2));
                dc.DrawLine(pen,
                    new Point(rect.X + rect.Width - 2, rect.Y + rect.Height - 1),
                    new Point(rect.X + 1, rect.Y + rect.Height - 1));
                dc.DrawLine(pen,
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
                dc.FillRectangle(new SolidBrush(insideColor), insideRect);

                Pen pen = new Pen(borderColor);
                dc.DrawLine(pen, new Point(rect.X + 1, rect.Y), new Point(rect.X + rect.Width - 2, rect.Y));
                dc.DrawLine(pen, new Point(rect.X + rect.Width - 1, rect.Y + 1), new Point(rect.X + rect.Width - 1, rect.Y + rect.Height - 2));
                dc.DrawLine(pen, new Point(rect.X + rect.Width - 2, rect.Y + rect.Height - 1), new Point(rect.X + 1, rect.Y + rect.Height - 1));
                dc.DrawLine(pen, new Point(rect.X, rect.Y + rect.Height - 2), new Point(rect.X, rect.Y + 1));
            }

            public static void DrawMultilineString(string text, Rectangle rectMain, Bitmap bmpCanvas, Font font, SolidBrush brush, StringFormat sf, int maxLines)
            {
                List<string> lines = new List<string>();
                string buffer = text;
                System.Drawing.SizeF size;
                int index1;
                int index2;
                char[] whitespace = new char[] { ' ', '\t', '\r', '\n' };

                try
                {
                    while (buffer.Length > 0)
                    {
                        size = Graphics.FromImage(bmpCanvas).MeasureString(buffer, font);

                        if (size.Width > rectMain.Width)
                        {
                            // Find the wrapping point of the line based on the width
                            for (index1 = buffer.Length - 1; index1 >= 0; index1--)
                            {
                                size = Graphics.FromImage(bmpCanvas).MeasureString(buffer.Substring(0, index1),
                                font);

                                if (size.Width <= rectMain.Width)
                                {
                                    // We found the wrapping point now let's look for the first
                                    // whitespace character - if there is one
                                    index2 = buffer.LastIndexOfAny(whitespace, index1);

                                    if (index2 >= 0)
                                    {
                                        // Whitespace character found
                                        lines.Add(buffer.Substring(0, index2));
                                        buffer = buffer.Substring(index2);
                                        break;
                                    }
                                    else
                                    {
                                        // Whitespace was not found
                                        lines.Add(buffer.Substring(0, index1));
                                        buffer = buffer.Substring(index1);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // This line completely fits so add it to the buffer unaltered
                            lines.Add(buffer);
                            buffer = "";
                        }
                    }

                    int h = rectMain.Height / lines.Count;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (i < maxLines)
                        {
                            Rectangle rectLine = new Rectangle(rectMain.X, rectMain.Y + i * h, rectMain.Width, h);
                            Graphics.FromImage(bmpCanvas).DrawString(lines[i], font, brush, rectLine, sf);
                        }
                    }

                }
                catch
                {
                    return;
                }
                finally
                {
                    lines = null;
                }
            }
        }
    }
}
