namespace IHK.ResultsNotifier.Windows
{
    partial class MainWindow
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
            this.panelMain = new System.Windows.Forms.Custom.CustomTableLayoutPanel();
            this.lbxLogs = new System.Windows.Forms.ListBox();
            this.panelBody = new System.Windows.Forms.Custom.CustomTableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.customTableLayoutPanel1 = new System.Windows.Forms.Custom.CustomTableLayoutPanel();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblCheckEveryXMin = new IHK.ResultsNotifier.Controls.CustomLabel();
            this.tbxMinutes = new System.Windows.Forms.TextBox();
            this.dashboard = new IHK.ResultsNotifier.Controls.Dashboard();
            this.pbxIHK = new System.Windows.Forms.PictureBox();
            this.controlBox = new System.Windows.Forms.Custom.CustomControlBox();
            this.panelMain.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.customTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIHK)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Transparent;
            this.panelMain.Color1 = System.Drawing.Color.Transparent;
            this.panelMain.Color2 = System.Drawing.Color.Black;
            this.panelMain.ColumnCount = 3;
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.panelMain.Controls.Add(this.lbxLogs, 1, 2);
            this.panelMain.Controls.Add(this.panelBody, 1, 1);
            this.panelMain.Controls.Add(this.pbxIHK, 1, 0);
            this.panelMain.CustomCursor = false;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.DraggableForm = false;
            this.panelMain.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelMain.Location = new System.Drawing.Point(6, 35);
            this.panelMain.Name = "panelMain";
            this.panelMain.RowCount = 4;
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.97283F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.70383F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.32333F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.panelMain.Size = new System.Drawing.Size(981, 540);
            this.panelMain.TabIndex = 2;
            // 
            // lbxLogs
            // 
            this.lbxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLogs.FormattingEnabled = true;
            this.lbxLogs.Location = new System.Drawing.Point(15, 370);
            this.lbxLogs.Name = "lbxLogs";
            this.lbxLogs.Size = new System.Drawing.Size(951, 154);
            this.lbxLogs.TabIndex = 2;
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.Transparent;
            this.panelBody.Color1 = System.Drawing.Color.Transparent;
            this.panelBody.Color2 = System.Drawing.Color.Transparent;
            this.panelBody.ColumnCount = 3;
            this.panelBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.panelBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.9885F));
            this.panelBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.011494F));
            this.panelBody.Controls.Add(this.label8, 0, 1);
            this.panelBody.Controls.Add(this.customTableLayoutPanel1, 1, 1);
            this.panelBody.Controls.Add(this.dashboard, 1, 0);
            this.panelBody.CustomCursor = false;
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.DraggableForm = false;
            this.panelBody.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelBody.Location = new System.Drawing.Point(15, 71);
            this.panelBody.Name = "panelBody";
            this.panelBody.RowCount = 2;
            this.panelBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.49056F));
            this.panelBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.50943F));
            this.panelBody.Size = new System.Drawing.Size(951, 293);
            this.panelBody.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.Location = new System.Drawing.Point(0, 267);
            this.label8.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 26);
            this.label8.TabIndex = 2;
            this.label8.Text = "Logs:";
            // 
            // customTableLayoutPanel1
            // 
            this.customTableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customTableLayoutPanel1.Color1 = System.Drawing.Color.Transparent;
            this.customTableLayoutPanel1.Color2 = System.Drawing.Color.Black;
            this.customTableLayoutPanel1.ColumnCount = 3;
            this.customTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.16518F));
            this.customTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.83481F));
            this.customTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.customTableLayoutPanel1.Controls.Add(this.btnStartStop, 2, 0);
            this.customTableLayoutPanel1.Controls.Add(this.lblCheckEveryXMin, 0, 0);
            this.customTableLayoutPanel1.Controls.Add(this.tbxMinutes, 1, 0);
            this.customTableLayoutPanel1.CustomCursor = false;
            this.customTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTableLayoutPanel1.DraggableForm = false;
            this.customTableLayoutPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.customTableLayoutPanel1.Location = new System.Drawing.Point(71, 232);
            this.customTableLayoutPanel1.Name = "customTableLayoutPanel1";
            this.customTableLayoutPanel1.RowCount = 1;
            this.customTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.customTableLayoutPanel1.Size = new System.Drawing.Size(859, 58);
            this.customTableLayoutPanel1.TabIndex = 4;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnStartStop.FlatAppearance.BorderSize = 0;
            this.btnStartStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(219)))), ((int)(((byte)(112)))));
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnStartStop.Location = new System.Drawing.Point(747, 9);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(109, 46);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "Start Listening";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblCheckEveryXMin
            // 
            this.lblCheckEveryXMin.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCheckEveryXMin.AutoSize = true;
            this.lblCheckEveryXMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckEveryXMin.ForeColor = System.Drawing.Color.FloralWhite;
            this.lblCheckEveryXMin.Location = new System.Drawing.Point(475, 19);
            this.lblCheckEveryXMin.Name = "lblCheckEveryXMin";
            this.lblCheckEveryXMin.Size = new System.Drawing.Size(176, 20);
            this.lblCheckEveryXMin.TabIndex = 2;
            this.lblCheckEveryXMin.Text = "Check Every X Minutes:";
            // 
            // tbxMinutes
            // 
            this.tbxMinutes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbxMinutes.Location = new System.Drawing.Point(657, 19);
            this.tbxMinutes.Name = "tbxMinutes";
            this.tbxMinutes.Size = new System.Drawing.Size(45, 20);
            this.tbxMinutes.TabIndex = 3;
            this.tbxMinutes.Text = "120";
            // 
            // dashboard
            // 
            this.dashboard.BackColor = System.Drawing.Color.Transparent;
            this.dashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboard.Location = new System.Drawing.Point(71, 3);
            this.dashboard.MinimumSize = new System.Drawing.Size(625, 200);
            this.dashboard.Name = "dashboard";
            this.dashboard.Size = new System.Drawing.Size(859, 223);
            this.dashboard.TabIndex = 5;
            // 
            // pbxIHK
            // 
            this.pbxIHK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbxIHK.BackColor = System.Drawing.Color.Transparent;
            this.pbxIHK.Image = global::IHK.ResultsNotifier.Properties.Resources.LogoIHK;
            this.pbxIHK.Location = new System.Drawing.Point(383, 3);
            this.pbxIHK.Name = "pbxIHK";
            this.pbxIHK.Size = new System.Drawing.Size(215, 56);
            this.pbxIHK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxIHK.TabIndex = 1;
            this.pbxIHK.TabStop = false;
            // 
            // controlBox
            // 
            this.controlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBox.BackColor = System.Drawing.Color.Transparent;
            this.controlBox.BackColorBtns = System.Drawing.Color.DimGray;
            this.controlBox.ColorMouseDownExit = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseDownMaximize = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseDownMinimize = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseHoverExit = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseHoverMaximize = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseHoverMinimize = System.Drawing.Color.Empty;
            this.controlBox.ForeColorBtns = System.Drawing.Color.FloralWhite;
            this.controlBox.Location = new System.Drawing.Point(916, 8);
            this.controlBox.Margin = new System.Windows.Forms.Padding(0);
            this.controlBox.MinimumSize = new System.Drawing.Size(60, 25);
            this.controlBox.Name = "controlBox";
            this.controlBox.Size = new System.Drawing.Size(71, 25);
            this.controlBox.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.AppTitle.Icon = null;
            this.AppTitle.IconLocation = new System.Drawing.Point(0, 0);
            this.AppTitle.IconSize = new System.Drawing.Size(0, 0);
            this.AppTitle.ShowIcon = false;
            this.AppTitle.ShowTextTitle = false;
            this.AppTitle.TextColor = System.Drawing.Color.Empty;
            this.AppTitle.TextFont = null;
            this.AppTitle.TextLocation = new System.Drawing.Point(0, 0);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 581);
            this.Controls.Add(this.controlBox);
            this.Controls.Add(this.panelMain);
            this.FormBackColor.GradientColor1 = System.Drawing.Color.SlateBlue;
            this.FormBackColor.GradientColor2 = System.Drawing.Color.DarkBlue;
            this.FormBackColor.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.FormBorders.Color = System.Drawing.Color.LightSteelBlue;
            this.FormBorders.DrawBorders = true;
            this.FormBorders.Width = 5;
            this.Icon = global::IHK.ResultsNotifier.Properties.Resources.AppIcon;
            this.MinimumSize = new System.Drawing.Size(720, 525);
            this.Name = "MainWindow";
            this.Padding = new System.Windows.Forms.Padding(6, 35, 5, 6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panelMain.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.customTableLayoutPanel1.ResumeLayout(false);
            this.customTableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIHK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxIHK;
        private System.Windows.Forms.Custom.CustomTableLayoutPanel panelMain;
        private System.Windows.Forms.ListBox lbxLogs;
        private System.Windows.Forms.Custom.CustomControlBox controlBox;
        private System.Windows.Forms.Custom.CustomTableLayoutPanel panelBody;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Custom.CustomTableLayoutPanel customTableLayoutPanel1;
        private System.Windows.Forms.Button btnStartStop;
        private Controls.CustomLabel lblCheckEveryXMin;
        private System.Windows.Forms.TextBox tbxMinutes;
        private Controls.Dashboard dashboard;
    }
}

