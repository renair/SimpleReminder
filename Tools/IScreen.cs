using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleReminder.Tools
{
    internal interface IScreen
    {
        void NavigatedTo();
        void NavigatedFrom();
    }
}
