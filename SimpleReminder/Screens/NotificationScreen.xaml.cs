namespace SimpleReminder.Screens
{
    public partial class NotificationScreen
    {
        public NotificationScreen()
        {
            InitializeComponent();
            InitializeTimePickers();
        }

        // This method initialize ComboBox with hours and minutes
        private void InitializeTimePickers()
        {
            for (int i = 0; i < 60; ++i)
            {
                string s = i.ToString();
                if (i < 24)
                {
                    HoursPicker.Items.Add(s.PadLeft(2, '0'));
                }
                MinutesPicker.Items.Add(s.PadLeft(2, '0'));
            }
        }
    }
}
