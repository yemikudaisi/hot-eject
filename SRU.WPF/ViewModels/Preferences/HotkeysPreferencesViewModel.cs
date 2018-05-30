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

        SerializableHotkey _ejectHotKey;
        SerializableHotkey _optionsHotKey;

        public HotKeysPreferencesViewModel()
        {
            DisplayName = "Hotkeys";
            if (Properties.Settings.Default.EjectHotKey == String.Empty)
            {
                _ejectHotKey = new SerializableHotkey(ModifierKeys.Control|ModifierKeys.Alt, Key.Z);
                _optionsHotKey = new SerializableHotkey(ModifierKeys.Control | ModifierKeys.Alt, Key.O);
            }
            else
            {
                var s = (string)Utilities.Settings["EjectHotKey"];
                _ejectHotKey = s.FromBase64String<SerializableHotkey>();
                s = (string)Utilities.Settings["OptionsHotKey"];
                _optionsHotKey = s.FromBase64String<SerializableHotkey>();
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
                Utilities.Settings["EjectHotKey"] = _ejectHotKey.ToBase64String();
                NotifyOfPropertyChange(()=> EjectHotKey);
            }
        }

        public SerializableHotkey OptionsHotKey
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
