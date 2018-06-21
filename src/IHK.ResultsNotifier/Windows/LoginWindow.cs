using System;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using IHK.ResultsNotifier.Utils;


namespace IHK.ResultsNotifier.Windows
{
    public partial class LoginWindow : CustomForm
    {
        private const int MIN_USER_CHARS = 7;
        private readonly string DEFAULT_USER;
        private readonly string DEFAULT_PASS;

        private readonly Configuration config;
        private WebClientIHK webClient;
        
        public LoginWindow()
        {
            InitializeComponent();            
            DEFAULT_USER = tbxUser.TextSearch;
            DEFAULT_PASS = tbxPassword.TextSearch;

            config = new Configuration();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            if (!Configuration.KeyExist) return;

            ConfigData data = config.GetConfigurations();
            cbxRemember.Checked = data.IsChecked;
            if (data.IsChecked)
            {
                tbxUser.Text     = data.Username;
                tbxPassword.Text = data.Password;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValidCredentials())
                return;

            string username = tbxUser.Text;
            string password = tbxPassword.Text;

            if (cbxRemember.Checked)
                config.SetConfigurations(new ConfigData(cbxRemember.Checked, username, password));

            webClient = new WebClientIHK();

            if (!webClient.AuthenticateUser(username, password))
            {
                MessageBox.Show("Failed to login. " +
                                "Check your internet connection or username/password.");
                return;
            }

            new MainWindow(webClient).Show(this);
            Hide();
        }

        private bool IsValidCredentials()
        {
            bool isNotEmptyAndRulesMatch = tbxUser.TextSearch.Length >= MIN_USER_CHARS
                                        && !String.IsNullOrEmpty(tbxPassword.Text);

            // Need to fix that text box control at some point, but this will do the quick workaround for now ;)
            bool isNotDefault = !String.Equals(tbxUser.Text, DEFAULT_USER)
                             && !String.Equals(tbxPassword.Text, DEFAULT_PASS);

            return isNotEmptyAndRulesMatch && isNotDefault;
        }

        private void cbxRemember_CheckedChanged(object sender, EventArgs e)
        {
            config.RememberMe(cbxRemember.Checked);
        }

        private void LoginWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char) Keys.Return:
                    btnLogin.PerformClick();
                    break;
                case (char) Keys.Escape:
                    panelBtns.Focus();
                    break;
            }
        }

    }
}
