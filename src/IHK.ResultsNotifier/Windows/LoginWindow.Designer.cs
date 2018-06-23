namespace IHK.ResultsNotifier.Windows
{
    partial class LoginWindow
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
            this.pbxIHK = new System.Windows.Forms.PictureBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbxUser = new System.Windows.Forms.Custom.CustomTextBoxSearch();
            this.panelTbxs = new System.Windows.Forms.Custom.CustomTableLayoutPanel();
            this.tbxPassword = new System.Windows.Forms.Custom.CustomTextBoxSearch();
            this.cbxRemember = new System.Windows.Forms.CheckBox();
            this.panelMain = new System.Windows.Forms.Custom.CustomTableLayoutPanel();
            this.panelBtns = new System.Windows.Forms.Custom.CustomPanel();
            this.controlBox = new System.Windows.Forms.Custom.CustomControlBoxDialog();
            this.loader = new IHK.ResultsNotifier.Controls.Loader();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIHK)).BeginInit();
            this.panelTbxs.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxIHK
            // 
            this.pbxIHK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbxIHK.BackColor = System.Drawing.Color.Transparent;
            this.pbxIHK.Image = global::IHK.ResultsNotifier.Properties.Resources.LogoIHK;
            this.pbxIHK.Location = new System.Drawing.Point(24, 0);
            this.pbxIHK.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pbxIHK.Name = "pbxIHK";
            this.pbxIHK.Size = new System.Drawing.Size(139, 61);
            this.pbxIHK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxIHK.TabIndex = 1;
            this.pbxIHK.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(219)))), ((int)(((byte)(112)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnLogin.Location = new System.Drawing.Point(42, 16);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(74, 32);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbxUser
            // 
            this.tbxUser.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbxUser.Animated = true;
            this.tbxUser.AnimateLength = 10;
            this.tbxUser.AnimTimeInterval = 4;
            this.tbxUser.BackgroundColor = System.Drawing.Color.White;
            this.tbxUser.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.tbxUser.ForeColor = System.Drawing.Color.DimGray;
            this.tbxUser.Location = new System.Drawing.Point(27, 3);
            this.tbxUser.MinimumSize = new System.Drawing.Size(83, 27);
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.PasswordChar = '\0';
            this.tbxUser.Size = new System.Drawing.Size(114, 33);
            this.tbxUser.TabIndex = 0;
            this.tbxUser.TabStop = false;
            this.tbxUser.Text = "Azubinummer";
            this.tbxUser.TextSearch = "Azubinummer";
            // 
            // panelTbxs
            // 
            this.panelTbxs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTbxs.BackColor = System.Drawing.Color.Transparent;
            this.panelTbxs.Color1 = System.Drawing.Color.Transparent;
            this.panelTbxs.Color2 = System.Drawing.Color.Transparent;
            this.panelTbxs.ColumnCount = 1;
            this.panelTbxs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelTbxs.Controls.Add(this.tbxUser, 0, 0);
            this.panelTbxs.Controls.Add(this.tbxPassword, 0, 1);
            this.panelTbxs.Controls.Add(this.cbxRemember, 0, 2);
            this.panelTbxs.CustomCursor = false;
            this.panelTbxs.DraggableForm = false;
            this.panelTbxs.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelTbxs.Location = new System.Drawing.Point(10, 70);
            this.panelTbxs.Name = "panelTbxs";
            this.panelTbxs.RowCount = 3;
            this.panelTbxs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelTbxs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelTbxs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.panelTbxs.Size = new System.Drawing.Size(168, 108);
            this.panelTbxs.TabIndex = 1;
            // 
            // tbxPassword
            // 
            this.tbxPassword.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbxPassword.Animated = true;
            this.tbxPassword.AnimateLength = 10;
            this.tbxPassword.AnimTimeInterval = 4;
            this.tbxPassword.BackgroundColor = System.Drawing.Color.White;
            this.tbxPassword.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.tbxPassword.ForeColor = System.Drawing.Color.DimGray;
            this.tbxPassword.Location = new System.Drawing.Point(28, 45);
            this.tbxPassword.MinimumSize = new System.Drawing.Size(83, 27);
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.PasswordChar = '*';
            this.tbxPassword.Size = new System.Drawing.Size(111, 33);
            this.tbxPassword.TabIndex = 1;
            this.tbxPassword.TabStop = false;
            this.tbxPassword.Text = "PasswortPass";
            this.tbxPassword.TextSearch = "PasswortPass";
            // 
            // cbxRemember
            // 
            this.cbxRemember.AutoSize = true;
            this.cbxRemember.Checked = true;
            this.cbxRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxRemember.ForeColor = System.Drawing.Color.FloralWhite;
            this.cbxRemember.Location = new System.Drawing.Point(30, 87);
            this.cbxRemember.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.cbxRemember.Name = "cbxRemember";
            this.cbxRemember.Size = new System.Drawing.Size(94, 17);
            this.cbxRemember.TabIndex = 2;
            this.cbxRemember.Text = "Remember me";
            this.cbxRemember.UseVisualStyleBackColor = true;
            this.cbxRemember.CheckedChanged += new System.EventHandler(this.cbxRemember_CheckedChanged);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Transparent;
            this.panelMain.Color1 = System.Drawing.Color.Transparent;
            this.panelMain.Color2 = System.Drawing.Color.Transparent;
            this.panelMain.ColumnCount = 3;
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.166667F));
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.83334F));
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.panelMain.Controls.Add(this.panelTbxs, 1, 1);
            this.panelMain.Controls.Add(this.panelBtns, 1, 2);
            this.panelMain.Controls.Add(this.pbxIHK, 1, 0);
            this.panelMain.CustomCursor = false;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.DraggableForm = false;
            this.panelMain.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelMain.Location = new System.Drawing.Point(6, 35);
            this.panelMain.Name = "panelMain";
            this.panelMain.RowCount = 4;
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelMain.Size = new System.Drawing.Size(201, 247);
            this.panelMain.TabIndex = 5;
            // 
            // panelBtns
            // 
            this.panelBtns.BackColor = System.Drawing.Color.Transparent;
            this.panelBtns.Color1 = System.Drawing.Color.Transparent;
            this.panelBtns.Color2 = System.Drawing.Color.Transparent;
            this.panelBtns.Controls.Add(this.btnLogin);
            this.panelBtns.CustomCursor = false;
            this.panelBtns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBtns.DraggableForm = false;
            this.panelBtns.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelBtns.Location = new System.Drawing.Point(10, 184);
            this.panelBtns.Name = "panelBtns";
            this.panelBtns.Size = new System.Drawing.Size(168, 51);
            this.panelBtns.TabIndex = 0;
            // 
            // controlBox
            // 
            this.controlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBox.BackColor = System.Drawing.Color.Transparent;
            this.controlBox.BackColorBtns = System.Drawing.Color.DimGray;
            this.controlBox.ColorMouseDownExit = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseDownMinimize = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseHoverExit = System.Drawing.Color.Empty;
            this.controlBox.ColorMouseHoverMinimize = System.Drawing.Color.Empty;
            this.controlBox.ForeColorBtns = System.Drawing.Color.FloralWhite;
            this.controlBox.Location = new System.Drawing.Point(147, 7);
            this.controlBox.Margin = new System.Windows.Forms.Padding(0);
            this.controlBox.MinimumSize = new System.Drawing.Size(50, 25);
            this.controlBox.Name = "controlBox";
            this.controlBox.Size = new System.Drawing.Size(60, 25);
            this.controlBox.TabIndex = 6;
            this.controlBox.TabStop = false;
            // 
            // loader
            // 
            this.loader.BackColor = System.Drawing.Color.Transparent;
            this.loader.DisableControlsOnWork = true;
            this.loader.LoaderKind = IHK.ResultsNotifier.Controls.LoaderKind.CircleBall;
            this.loader.Location = new System.Drawing.Point(9, 7);
            this.loader.Name = "loader";
            this.loader.Size = new System.Drawing.Size(20, 20);
            this.loader.SizeLoading = new System.Drawing.Size(85, 85);
            this.loader.TabIndex = 7;
            this.loader.Visible = false;
            // 
            // LoginWindow
            // 
            this.AllowResize = false;
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
            this.ClientSize = new System.Drawing.Size(213, 288);
            this.Controls.Add(this.loader);
            this.Controls.Add(this.controlBox);
            this.Controls.Add(this.panelMain);
            this.FormBackColor.GradientColor1 = System.Drawing.Color.SlateBlue;
            this.FormBackColor.GradientColor2 = System.Drawing.Color.DarkBlue;
            this.FormBackColor.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.FormBorders.Color = System.Drawing.Color.LightSteelBlue;
            this.FormBorders.DrawBorders = true;
            this.FormBorders.Width = 5;
            this.Icon = global::IHK.ResultsNotifier.Properties.Resources.AppIcon;
            this.KeyPreview = true;
            this.Name = "LoginWindow";
            this.Padding = new System.Windows.Forms.Padding(6, 35, 6, 6);
            this.ResizeGrip = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.LoginWindow_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LoginWindow_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbxIHK)).EndInit();
            this.panelTbxs.ResumeLayout(false);
            this.panelTbxs.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxIHK;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Custom.CustomTextBoxSearch tbxUser;
        private System.Windows.Forms.Custom.CustomTableLayoutPanel panelTbxs;
        private System.Windows.Forms.Custom.CustomTextBoxSearch tbxPassword;
        private System.Windows.Forms.Custom.CustomTableLayoutPanel panelMain;
        private System.Windows.Forms.Custom.CustomControlBoxDialog controlBox;
        private System.Windows.Forms.CheckBox cbxRemember;
        private System.Windows.Forms.Custom.CustomPanel panelBtns;
        private Controls.Loader loader;
    }
}