using Caliburn.Micro;
using System;
using System.Windows.Input;
using Sru.Wpf.Input;
using Sru.Wpf.Extensions;
using Sru.Wpf.Infrastructure;

namespace Sru.Wpf.ViewModels
{
    public class HotKeysPreferencesViewModel : Screen
    {

        SerializableHotKey _ejectHotKey;
        SerializableHotKey _optionsHotKey;

        public HotKeysPreferencesViewModel()
        {
            DisplayName = "Hotkeys";
            if (Properties.Settings.Default.EjectHotKey == String.Empty)
            {
                _ejectHotKey = new SerializableHotKey(ModifierKeys.Control|ModifierKeys.Alt, Key.Z);
                _optionsHotKey = new SerializableHotKey(ModifierKeys.Control | ModifierKeys.Alt, Key.O);
            }
            else
            {
                var s = (string)Utilities.Settings["EjectHotKey"];
                _ejectHotKey = s.FromBase64String<SerializableHotKey>();
                s = (string)Utilities.Settings["OptionsHotKey"];
                _optionsHotKey = s.FromBase64String<SerializableHotKey>();
            }
        }

        public SerializableHotKey EjectHotKey
        {
            get
            {
                return _ejectHotKey;
            }

            set
            {
                _ejectHotKey = value;
                Utilities.Settings["EjectHotKey"] = _ejectHotKey.ToBase64String();
                NotifyOfPropertyChange(()=> EjectHotKey);
            }
        }

        public SerializableHotKey OptionsHotKey
        {
            get
            {
                return _optionsHotKey;
            }

            set
            {
                _optionsHotKey = value;
                Utilities.Settings["OptionsHotKey"] = _optionsHotKey.ToBase64String(); // ToBase64String() is an extension of objects
                NotifyOfPropertyChange(() => OptionsHotKey);
            }
        }
    }
}
