namespace IHK.ResultsNotifier.Utils
{
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