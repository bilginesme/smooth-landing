namespace SmoothLanding
{
    partial class FrmStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStatistics));
            this.radioRangeLast7Days = new System.Windows.Forms.RadioButton();
            this.radioRangeWeekly = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // radioRangeLast7Days
            // 
            this.radioRangeLast7Days.AutoSize = true;
            this.radioRangeLast7Days.Location = new System.Drawing.Point(13, 13);
            this.radioRangeLast7Days.Name = "radioRangeLast7Days";
            this.radioRangeLast7Days.Size = new System.Drawing.Size(81, 17);
            this.radioRangeLast7Days.TabIndex = 0;
            this.radioRangeLast7Days.TabStop = true;
            this.radioRangeLast7Days.Text = "Last 7 Days";
            this.radioRangeLast7Days.UseVisualStyleBackColor = true;
            this.radioRangeLast7Days.CheckedChanged += new System.EventHandler(this.radioRangeLast7Days_CheckedChanged);
            // 
            // radioRangeWeekly
            // 
            this.radioRangeWeekly.AutoSize = true;
            this.radioRangeWeekly.Location = new System.Drawing.Point(100, 13);
            this.radioRangeWeekly.Name = "radioRangeWeekly";
            this.radioRangeWeekly.Size = new System.Drawing.Size(61, 17);
            this.radioRangeWeekly.TabIndex = 1;
            this.radioRangeWeekly.TabStop = true;
            this.radioRangeWeekly.Text = "Weekly";
            this.radioRangeWeekly.UseVisualStyleBackColor = true;
            this.radioRangeWeekly.CheckedChanged += new System.EventHandler(this.radioRangeWeekly_CheckedChanged);
            // 
            // FrmStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 286);
            this.Controls.Add(this.radioRangeWeekly);
            this.Controls.Add(this.radioRangeLast7Days);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStatistics";
            this.Text = "Statistics";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmStatistics_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioRangeLast7Days;
        private System.Windows.Forms.RadioButton radioRangeWeekly;
    }
}