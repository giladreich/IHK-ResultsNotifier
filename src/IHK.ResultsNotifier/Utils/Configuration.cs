using System;
using Microsoft.Win32;


namespace IHK.ResultsNotifier.Utils
{
    public class Configuration
    {
        private const string REG_KEY = "Software\\IHK-ResultsNotifier";

        private const string REG_CHECKED = "Checked";
        private const string REG_VALUE1 = "Value1";
        private const string REG_VALUE2 = "Value2";
        private const string REG_VALUE3 = "Value3";

        public static bool KeyExist => Registry.CurrentUser.OpenSubKey(REG_KEY) != null;

        public ConfigurationData GetConfigurations()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(REG_KEY);
            if (reg == null) return new ConfigurationData(false, null);

            bool isChecked = Convert.ToBoolean(reg.GetValue(REG_CHECKED));
            string key = reg.GetValue(REG_VALUE1).ToString();
            string user = Encryption.Decrypt(reg.GetValue(REG_VALUE2).ToString(), key);
            string pass = Encryption.Decrypt(reg.GetValue(REG_VALUE3).ToString(), key);
            reg.Dispose();

            return new ConfigurationData(isChecked, new User(user, pass));
        }

        public void SetConfigurations(ConfigurationData data)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(REG_KEY, true);
            if (reg == null) return;

            string key = Encryption.GenerateKey(10);
            string encryptedUser = Encryption.Encrypt(data.User.Username, key);
            string encryptedPass = Encryption.Encrypt(data.User.Password, key);

            reg.SetValue(REG_CHECKED, data.IsChecked, RegistryValueKind.DWord);
            reg.SetValue(REG_VALUE1, key);
            reg.SetValue(REG_VALUE2, encryptedUser);
            reg.SetValue(REG_VALUE3, encryptedPass);
            reg.Dispose();
        }

        public void RememberMe(bool remember)
        {
            if (remember)
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey(REG_KEY, true);
                if (reg == null) return;

                reg.SetValue(REG_CHECKED, remember, RegistryValueKind.DWord);
                reg.Dispose();
            }
            else
            {
                Registry.CurrentUser.DeleteSubKeyTree(REG_KEY);
            }
        }
    }
}
