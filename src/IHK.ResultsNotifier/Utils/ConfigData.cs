namespace IHK.ResultsNotifier.Utils
{
    public struct ConfigData
    {
        public bool IsChecked { get; }
        public User User { get; }

        public ConfigData(bool isChecked, User user)
        {
            IsChecked = isChecked;
            User = user;
        }
    }
}