﻿namespace SmoothLanding
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.timer60seconds = new System.Windows.Forms.Timer(this.components);
            this.timerAlert = new System.Windows.Forms.Timer(this.components);
            this.ctxGeneral = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsDate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsInitialize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSkipSession = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.tsAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsClose = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxGeneral.SuspendLayout();
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
            this.timerAlert.Interval = 197000;
            this.timerAlert.Tick += new System.EventHandler(this.timerAlert_Tick);
            // 
            // ctxGeneral
            // 
            this.ctxGeneral.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDate,
            this.tsInitialize,
            this.tsSkipSession,
            this.toolStripMenuItem1,
            this.tsSettings,
            this.tsStatistics,
            this.tsAbout,
            this.toolStripMenuItem2,
            this.tsClose});
            this.ctxGeneral.Name = "ctxPragmas";
            this.ctxGeneral.Size = new System.Drawing.Size(171, 192);
            this.ctxGeneral.Opening += new System.ComponentModel.CancelEventHandler(this.ctxGeneral_Opening);
            // 
            // tsDate
            // 
            this.tsDate.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsDate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tsDate.Name = "tsDate";
            this.tsDate.Size = new System.Drawing.Size(170, 22);
            this.tsDate.Text = "10 December 2018";
            this.tsDate.Click += new System.EventHandler(this.tsDate_Click);
            // 
            // tsInitialize
            // 
            this.tsInitialize.Name = "tsInitialize";
            this.tsInitialize.Size = new System.Drawing.Size(170, 22);
            this.tsInitialize.Text = "Initialize";
            this.tsInitialize.Click += new System.EventHandler(this.tsInitialize_Click);
            // 
            // tsSkipSession
            // 
            this.tsSkipSession.Name = "tsSkipSession";
            this.tsSkipSession.Size = new System.Drawing.Size(170, 22);
            this.tsSkipSession.Text = "Skip Session";
            this.tsSkipSession.Click += new System.EventHandler(this.tsSkipSession_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(167, 6);
            // 
            // tsSettings
            // 
            this.tsSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsSettings.Image")));
            this.tsSettings.Name = "tsSettings";
            this.tsSettings.Size = new System.Drawing.Size(170, 22);
            this.tsSettings.Text = "Settings";
            this.tsSettings.Click += new System.EventHandler(this.tsSettings_Click);
            // 
            // tsStatistics
            // 
            this.tsStatistics.Image = ((System.Drawing.Image)(resources.GetObject("tsStatistics.Image")));
            this.tsStatistics.Name = "tsStatistics";
            this.tsStatistics.Size = new System.Drawing.Size(170, 22);
            this.tsStatistics.Text = "Statistics";
            this.tsStatistics.Click += new System.EventHandler(this.tsStatistics_Click);
            // 
            // tsAbout
            // 
            this.tsAbout.Name = "tsAbout";
            this.tsAbout.Size = new System.Drawing.Size(170, 22);
            this.tsAbout.Text = "About";
            this.tsAbout.Click += new System.EventHandler(this.tsAbout_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(167, 6);
            // 
            // tsClose
            // 
            this.tsClose.Name = "tsClose";
            this.tsClose.Size = new System.Drawing.Size(170, 22);
            this.tsClose.Text = "Close";
            this.tsClose.Click += new System.EventHandler(this.tsClose_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 48);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "Smooth Landing";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.DoubleClick += new System.EventHandler(this.FrmMain_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseDown);
            this.MouseEnter += new System.EventHandler(this.FrmMain_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.FrmMain_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseUp);
            this.ctxGeneral.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer60seconds;
        private System.Windows.Forms.Timer timerAlert;
        private System.Windows.Forms.ContextMenuStrip ctxGeneral;
        private System.Windows.Forms.ToolStripMenuItem tsInitialize;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsClose;
        private System.Windows.Forms.ToolStripMenuItem tsSkipSession;
        private System.Windows.Forms.ToolStripMenuItem tsSettings;
        private System.Windows.Forms.ToolStripMenuItem tsAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsStatistics;
        private System.Windows.Forms.ToolStripMenuItem tsDate;
    }
}