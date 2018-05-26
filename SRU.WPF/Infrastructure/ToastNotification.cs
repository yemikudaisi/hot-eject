using Caliburn.Micro;
using Sru.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf.Infrastructure
{
    public class ToastNotification
    {
        public static void Toast(String message)
        {
            var toastViewModel = new ToastViewModel(message);
            IoC.Get<IWindowManager>().ShowDialog(toastViewModel);
        }
    }
}
