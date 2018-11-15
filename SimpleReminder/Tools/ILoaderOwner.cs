using System.ComponentModel;
using System.Windows;

namespace SimpleReminder.Tools
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool ContentEnabled { get; set; }
    }
}
