﻿namespace SmoothLanding
{
    partial class FrmFocus
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
            this.components = new System.ComponentModel.Container();
            this.timer60seconds = new System.Windows.Forms.Timer(this.components);
            this.timerAlert = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer60seconds
            // 
            this.timer60seconds.Enabled = true;
            this.timer60seconds.Interval = 1000;
            this.timer60seconds.Tick += new System.EventHandler(this.timer60seconds_Tick);
            // 
            // timerAlert
            // 
            this.timerAlert.Enabled = true;
            this.timerAlert.Interval = 17000;
            this.timerAlert.Tick += new System.EventHandler(this.timerAlert_Tick);
            // 
            // FrmFocus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 48);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmFocus";
            this.Text = "FrmFocus";
            this.Load += new System.EventHandler(this.FrmFocus_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmFocus_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmFocus_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmFocus_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer60seconds;
        private System.Windows.Forms.Timer timerAlert;
    }
}