using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmoothLanding
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmFocus frm = new FrmFocus();
            Screen myScreen = Screen.FromControl(frm);
            System.Drawing.Rectangle area = myScreen.WorkingArea;
            frm.Left = area.Right - frm.Width - 10;
            frm.Top = 10;
            frm.TopMost = true;
            frm.Opacity = 0.9;
            Application.Run(frm);
        }
    }
}
