namespace SmoothLanding
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
            this.tsInitialize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSkipSession = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
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
            this.tsInitialize,
            this.tsSkipSession,
            this.toolStripMenuItem1,
            this.tsClose});
            this.ctxGeneral.Name = "ctxPragmas";
            this.ctxGeneral.Size = new System.Drawing.Size(139, 76);
            // 
            // tsInitialize
            // 
            this.tsInitialize.Name = "tsInitialize";
            this.tsInitialize.Size = new System.Drawing.Size(138, 22);
            this.tsInitialize.Text = "Initialize";
            this.tsInitialize.Click += new System.EventHandler(this.tsInitialize_Click);
            // 
            // tsSkipSession
            // 
            this.tsSkipSession.Name = "tsSkipSession";
            this.tsSkipSession.Size = new System.Drawing.Size(138, 22);
            this.tsSkipSession.Text = "Skip Session";
            this.tsSkipSession.Click += new System.EventHandler(this.tsSkipSession_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(135, 6);
            // 
            // tsClose
            // 
            this.tsClose.Name = "tsClose";
            this.tsClose.Size = new System.Drawing.Size(138, 22);
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
    }
}