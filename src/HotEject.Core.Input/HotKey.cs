// Copyright (c) 2018 Yemi Kudaisi for the SRU
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace HotEject.Core.Input
{
    /// <summary>
    /// Implementation of windows hot keys using native win32 api
    /// </summary>
    [Serializable]
    public class SerializableHotKey : IEquatable<SerializableHotKey> , IDisposable
    {
        public event Action<SerializableHotKey> Pressed;

        private bool _isKeyRegistered;
        [NonSerialized]
        private IntPtr _handle;

        #region Public Properties
        /// <summary>
        /// Gets or sets the key assigned to the hot key
        /// </summary>
        public virtual Key Key { get; protected set; }

        /// <summary>
        /// Gets or sets the modifier keys for the hot key
        /// </summary>
        public virtual ModifierKeys Modifiers { get; protected set; }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }

            set
            {
                UnregisterHotKey();
                _handle = value;
                RegisterHotKey();
                ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
            }
        }
        #endregion

        public SerializableHotKey(ModifierKeys modifierKeys, Key key)
        {
            Key = key;
            Modifiers = modifierKeys;
        }

        public SerializableHotKey(ModifierKeys modifierKeys, Key key, Window window)
            : this(modifierKeys, key, new WindowInteropHelper(window))
        {
            Contract.Requires(window != null);
        }

        public SerializableHotKey(ModifierKeys modifierKeys, Key key, WindowInteropHelper window)
            : this(modifierKeys, key, window.Handle)
        {
            Contract.Requires(window != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifierKeys"></param>
        /// <param name="key"></param>
        /// <param name="windowHandle"></param>
        public SerializableHotKey(ModifierKeys modifierKeys, Key key, IntPtr windowHandle)
        {
            Contract.Requires(modifierKeys != ModifierKeys.None || key != Key.None);
            Contract.Requires(windowHandle != IntPtr.Zero);

            Key = key;
            Modifiers = modifierKeys;
            Handle = windowHandle;
            RegisterHotKey();
            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
        }

        /// <summary>
        /// Class destructor
        /// </summary>
        ~SerializableHotKey()
        {
            Dispose();
        }

        public void RegisterHotKey()
        {
            if (Key == Key.None)
                return;
            if (_isKeyRegistered)
                UnregisterHotKey();
            int i = KeyInterop.VirtualKeyFromKey(Key);
            _isKeyRegistered = HotKeyWin32Helper.RegisterHotKey(Handle, GetHashCode(), Modifiers, i);
            if (!_isKeyRegistered)
                throw new HotKeyRegisteredException();
        }

        public void UnregisterHotKey()
        {
            if(_isKeyRegistered)
            _isKeyRegistered = !HotKeyWin32Helper.UnregisterHotKey(Handle, GetHashCode());
        }

        public void Dispose()
        {
            ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessageMethod;
            UnregisterHotKey();
        }

        private void ThreadPreprocessMessageMethod(ref MSG msg, ref bool handled)
        {
            if (!handled)
            {
                if (msg.message == HotKeyWin32Helper.WmHotKey
                    && (int)(msg.wParam) == GetHashCode())
                {
                    OnHotKeyPressed();
                    handled = true;
                }
            }
        }

        private void OnHotKeyPressed()
        {
            if (Pressed != null)
                Pressed(this);
        }

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

        public override bool Equals(object obj)
        {
            return Equals(obj as SerializableHotKey);
        }

        public bool Equals(SerializableHotKey other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return (this.Modifiers == other.Modifiers && this.Key == other.Key);
        }

        public static bool operator == (SerializableHotKey a, SerializableHotKey b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SerializableHotKey a, SerializableHotKey b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
