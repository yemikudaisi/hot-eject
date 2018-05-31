using Sru.Wpf.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Sru.Wpf.Converter
{
    /// <summary>
    /// Converts Hotkey to string
    /// </summary>
    [ValueConversion(typeof(SerializableHotKey), typeof(string))]
    public class HotkeyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "< not set >";

            var hotkey = (SerializableHotKey)value;
            var key = (Key)hotkey.Key;
            var modifiers = (ModifierKeys)hotkey.Modifiers;

            var str = new StringBuilder();
            if (modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");
            str.Append(key);
            return str.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
