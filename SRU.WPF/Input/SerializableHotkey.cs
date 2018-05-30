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

namespace Sru.Wpf.Input
{
    /// <summary>
    /// Implementation of windows hot keys using native win32 api
    /// </summary>
    [Serializable]
    public sealed class SerializableHotkey : IDisposable
    {
        public event Action<SerializableHotkey> HotKeyPressed;

        private readonly int _id;
        private bool _isKeyRegistered;
        readonly IntPtr _handle;

        #region Public Properties
        /// <summary>
        /// Gets or sets the key assigned to the hot key
        /// </summary>
        public Key Key { get; private set; }

        /// <summary>
        /// Gets or sets the modifier keys for the hot key
        /// </summary>
        public ModifierKeys Modifiers { get; private set; }
        #endregion

        public SerializableHotkey(ModifierKeys modifierKeys, Key key)
        {
            Key = key;
            Modifiers = modifierKeys;
        }

        public SerializableHotkey(ModifierKeys modifierKeys, Key key, Window window)
            : this(modifierKeys, key, new WindowInteropHelper(window))
        {
            Contract.Requires(window != null);
        }

        public SerializableHotkey(ModifierKeys modifierKeys, Key key, WindowInteropHelper window)
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
        public SerializableHotkey(ModifierKeys modifierKeys, Key key, IntPtr windowHandle)
        {
            Contract.Requires(modifierKeys != ModifierKeys.None || key != Key.None);
            Contract.Requires(windowHandle != IntPtr.Zero);

            Key = key;
            Modifiers = modifierKeys;
            _id = GetHashCode();
            _handle = windowHandle;
            RegisterHotKey();
            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
        }

        /// <summary>
        /// Class destructor
        /// </summary>
        ~SerializableHotkey()
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
            _isKeyRegistered = HotKeyWin32Helper.RegisterHotKey(_handle, _id, Modifiers, i);
            if (!_isKeyRegistered)
                throw new ApplicationException("HotKey already in use");
        }

        public void UnregisterHotKey()
        {
            _isKeyRegistered = !HotKeyWin32Helper.UnregisterHotKey(_handle, _id);
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
                    && (int)(msg.wParam) == _id)
                {
                    OnHotKeyPressed();
                    handled = true;
                }
            }
        }

        private void OnHotKeyPressed()
        {
            if (HotKeyPressed != null)
                HotKeyPressed(this);
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
    }
}
