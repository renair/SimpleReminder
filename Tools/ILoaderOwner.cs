﻿using System.ComponentModel;
using System.Windows;

namespace Tools
{
    public interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool ContentEnabled { get; set; }
    }
}
