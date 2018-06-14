﻿using System;
using System.Windows.Forms;
using System.Windows.Forms.Custom;

namespace IHK.ResultsNotifier.Windows
{
    public partial class LoginWindow : CustomForm
    {
        private const int MIN_USER_CHARS = 7;
        private readonly string DEFAULT_USER;
        private readonly string DEFAULT_PASS;


        public LoginWindow()
        {
            InitializeComponent();
            DEFAULT_USER = tbxUser.TextSearch;
            DEFAULT_PASS = tbxPassword.TextSearch;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValidCredintials())
                return;

            string username = tbxUser.Text;
            string password = tbxPassword.Text;

            new MainWindow().Show(this);
            Hide();
        }

        private bool IsValidCredintials()
        {
            bool isNotEmptyAndRulesMatch = tbxUser.TextSearch.Length >= MIN_USER_CHARS
                                        && !String.IsNullOrEmpty(tbxPassword.Text);

            // Need to fix that text box control at some point, but this will do the quick workaround for now ;)
            bool isNotDefault = !String.Equals(tbxUser.Text, DEFAULT_USER)
                             && !String.Equals(tbxPassword.Text, DEFAULT_PASS);

            return isNotEmptyAndRulesMatch && isNotDefault;
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
