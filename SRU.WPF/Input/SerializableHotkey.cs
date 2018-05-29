using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sru.Wpf.Input
{
    [Serializable]
    public class SerializableHotkey
    {
        public Key Key { get; }

        public ModifierKeys Modifiers { get; }

        public SerializableHotkey(ModifierKeys modifiers,Key key)
        {
            Key = key;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Constructor that thake string and parses the hotkey
        /// </summary>
        /// <param name="hotkey">The string value of the hotkey</param>
        public SerializableHotkey(String hotkey)
        {
            var key = hotkey.Split('+');
        } 

        /// <summary>
        /// Converts to string format that can be persisted
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }
    }
}
