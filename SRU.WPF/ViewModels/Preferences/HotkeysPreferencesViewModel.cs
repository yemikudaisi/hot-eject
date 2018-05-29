using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using Sru.Wpf.Input;
using Sru.Wpf.Extension;

namespace Sru.Wpf.ViewModels
{
    public class HotkeysPreferencesViewModel : Screen
    {

        SerializableHotkey _ejectHotKey;
        SerializableHotkey _optionsHotkey;

        public HotkeysPreferencesViewModel()
        {
            DisplayName = "Hotkeys";
            if (Properties.Settings.Default.EjectHotKey == String.Empty)
            {
                EjectHotKey = new SerializableHotkey(ModifierKeys.Control|ModifierKeys.Alt, Key.Z);
                OptionsHotKey = new SerializableHotkey(ModifierKeys.Control | ModifierKeys.Alt, Key.O);
            }
        }

        public SerializableHotkey EjectHotKey
        {
            get
            {
                return _ejectHotKey;
            }

            set
            {
                _ejectHotKey = value;
                Properties.Settings.Default.EjectHotKey = _ejectHotKey.ToBase64String();
                NotifyOfPropertyChange(()=> EjectHotKey);
            }
        }

        public SerializableHotkey OptionsHotKey
        {
            get
            {
                return _optionsHotkey;
            }

            set
            {
                _optionsHotkey = value;
                Properties.Settings.Default.OptionsHotKey= _optionsHotkey.ToBase64String(); // .ToBase64String() is an extension of objects
                NotifyOfPropertyChange(() => OptionsHotKey);
            }
        }
    }
}
