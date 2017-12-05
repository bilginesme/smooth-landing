namespace SmoothLanding
{
    partial class FrmImageEnlarged
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmImageEnlarged
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 200);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImageEnlarged";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmImageEnlarged";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmImageEnlarged_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmImageEnlarged_MouseDown);
            this.MouseEnter += new System.EventHandler(this.FrmImageEnlarged_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.FrmImageEnlarged_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmImageEnlarged_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmImageEnlarged_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}