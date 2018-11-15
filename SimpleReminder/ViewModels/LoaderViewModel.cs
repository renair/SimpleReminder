using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SimpleReminder.Tools;

namespace SimpleReminder.ViewModels
{
    internal class LoaderViewModel : ILoaderOwner
    {
        private Visibility _loaderVisibility = Visibility.Collapsed;
        private bool _contentEnabled = true;

        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool ContentEnabled
        {
            get { return _contentEnabled; }
            set
            {
                _contentEnabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
