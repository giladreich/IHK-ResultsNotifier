using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using IHK.ResultsNotifier.Misc;
using IHK.ResultsNotifier.Utils;


namespace IHK.ResultsNotifier.Windows
{
    public partial class LoginWindow : CustomForm
    {
        private const int MIN_USER_CHARS = 7;
        private readonly string DEFAULT_USER;
        private readonly string DEFAULT_PASS;

        private readonly Configuration config;
        private NetworkClient networkClient;
        private User currentUser;

        public LoginWindow()
        {
            InitializeComponent();
            DEFAULT_USER = tbxUser.TextSearch;
            DEFAULT_PASS = tbxPassword.TextSearch;

            config = new Configuration();
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            if (!Configuration.KeyExist) return;

            ConfigData data = config.GetConfigurations();
            cbxRemember.Checked = data.IsChecked;
            if (data.IsChecked)
            {
                tbxUser.Text     = data.User.Username;
                tbxPassword.Text = data.User.Password;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValidCredentials())
                return;

            loader.Show();
            await Utility.SimulateWork(TimeSpan.FromSeconds(2));

            User user = new User(tbxUser.Text, tbxPassword.Text);

            if (currentUser?.GetHashCode() != user.GetHashCode())
                ResetNetworkClient();

            await ValidateNetworkClient(user);

            if (!networkClient.IsAuthenticated)
            {
                ResetNetworkClient();
                loader.Hide();

                this.InvokeSafe(() => 
                    MessageBox.Show("Failed to login. Please try again.", 
                        "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information));

                return;
            }

            if (cbxRemember.Checked)
                config.SetConfigurations(new ConfigData(cbxRemember.Checked, user));

            currentUser = new User(user);
            this.InvokeSafe(() => new MainWindow(networkClient).Show(this));
            loader.Hide();
            this.Visible(false);
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

        private async Task ValidateNetworkClient(User user)
        {
            try
            {
                if (networkClient == null)
                {
                    networkClient = new NetworkClient();
                    await networkClient.AuthenticateUser(user);
                }
                else
                {
                    await networkClient.ValidateAuthentication();
                }
            }
            catch (Exception ex)
            {
                loader.Hide();
                ResetNetworkClient();
                this.InvokeSafe(() => 
                    MessageBox.Show("Exception thrown while validating network client -> " + ex.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        private void ResetNetworkClient()
        {
            networkClient?.Dispose();
            networkClient = null;
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

        private void LoginWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetNetworkClient();
        }
    }
}
