using System.Windows;
using SimpleReminder.Tools;
using SimpleReminder.ViewModels;

namespace SimpleReminder.Managers
{
    class LoaderManager
    {
        private static  ILoaderOwner _viewModel;

        private static ILoaderOwner ViewModel
        {
            get
            {
                return _viewModel ?? (_viewModel = new LoaderViewModel());
            }
            set { _viewModel = value; }
        }

        internal static void Initialize(ILoaderOwner owner)
        {
            ViewModel = owner;
        }

        internal static void ShowLoader()
        {
            ViewModel.ContentEnabled = false;
            ViewModel.LoaderVisibility = Visibility.Visible;
        }

        internal static void HideLoader()
        {
            ViewModel.ContentEnabled = true;
            ViewModel.LoaderVisibility = Visibility.Collapsed;
        }
    }
}
