using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.ViewModels
{
    public class ToastViewModel : Screen
    {
        string _message;
        public ToastViewModel() : this("")
        {
            
        }

        public ToastViewModel(String message)
        {
            Message = message;
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
    }
}
