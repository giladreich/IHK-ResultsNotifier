namespace IHK.ResultsNotifier.Utils
{
    public struct ConfigurationData
    {
        public bool IsChecked { get; }
        public User User { get; }

        public ConfigurationData(bool isChecked, User user)
        {
            IsChecked = isChecked;
            User = user;
        }
    }
}