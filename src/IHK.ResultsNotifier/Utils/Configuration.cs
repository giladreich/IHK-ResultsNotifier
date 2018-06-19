using System;
using System.IO;
using System.Linq;
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

        public ConfigData GetConfigurations()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(REG_KEY);
            if (reg == null) return new ConfigData(false, null, null);

            bool isChecked = Convert.ToBoolean(reg.GetValue(REG_CHECKED));
            string key = reg.GetValue(REG_VALUE1).ToString();
            string user = Encryption.Decrypt(reg.GetValue(REG_VALUE2).ToString(), key);
            string pass = Encryption.Decrypt(reg.GetValue(REG_VALUE3).ToString(), key);
            reg.Dispose();

            return new ConfigData(isChecked, user, pass);
        }

        public void SetConfigurations(ConfigData data)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(REG_KEY, true);
            if (reg == null) return;

            string key = Encryption.GenerateKey(10);
            string encryptedUser = Encryption.Encrypt(data.Username, key);
            string encryptedPass = Encryption.Encrypt(data.Password, key);

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


    public struct ConfigData
    {
        public bool IsChecked { get; }
        public string Username { get; }
        public string Password { get; }

        public ConfigData(bool isChecked, string username, string password)
        {
            IsChecked = isChecked;
            Username = username;
            Password = password;
        }
    }
}
