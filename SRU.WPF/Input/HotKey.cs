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
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Sru.Wpf.Input
{
    /// <summary>
    /// Implementation of windows hot keys using native win32 api
    /// </summary>
    public sealed class HotKey : IDisposable
    {
        public event Action<HotKey> HotKeyPressed;

        private readonly int _id;
        private bool _isKeyRegistered;
        readonly IntPtr _handle;

        public HotKey(ModifierKeys modifierKeys, Keys key, Window window)
            : this(modifierKeys, key, new WindowInteropHelper(window))
        {
            Contract.Requires(window != null);
        }

        public HotKey(ModifierKeys modifierKeys, Keys key, WindowInteropHelper window)
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
        public HotKey(ModifierKeys modifierKeys, Keys key, IntPtr windowHandle)
        {
            Contract.Requires(modifierKeys != ModifierKeys.None || key != Keys.None);
            Contract.Requires(windowHandle != IntPtr.Zero);

            Key = key;
            KeyModifier = modifierKeys;
            _id = GetHashCode();
            _handle = windowHandle;
            RegisterHotKey();
            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
        }

        /// <summary>
        /// Class destructor
        /// </summary>
        ~HotKey()
        {
            Dispose();
        }

        /// <summary>
        /// Gets or sets the key assigned to the hot key
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Gets or sets the modifier keys for the hot key
        /// </summary>
        public ModifierKeys KeyModifier { get; private set; }

        public void RegisterHotKey()
        {
            if (Key == Keys.None)
                return;
            if (_isKeyRegistered)
                UnregisterHotKey();
            _isKeyRegistered = HotKeyNativeApi.RegisterHotKey(_handle, _id, KeyModifier, Key);
            if (!_isKeyRegistered)
                throw new ApplicationException("Hotkey already in use");
        }

        public void UnregisterHotKey()
        {
            _isKeyRegistered = !HotKeyNativeApi.UnregisterHotKey(_handle, _id);
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
                if (msg.message == HotKeyNativeApi.WmHotKey
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
    }
}
