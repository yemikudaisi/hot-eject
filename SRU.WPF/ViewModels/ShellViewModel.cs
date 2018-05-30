using Caliburn.Micro;
using Sru.Wpf.Controls;
using Sru.Wpf.Input;
using Sru.Wpf.ViewModels;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Hardcodet.Wpf.TaskbarNotification;
using Sru.Core;
using Sru.Wpf.Infrastructure;
using Sru.Core.Usb;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sru.Wpf
{
    [Export(typeof(ITaskbarIconShell))]
    public class ShellViewModel : Screen, ITaskbarIconShell
    {
        public ShellViewModel()
        {
            InitializeHotKeys();
            _optionsViewModel = new OptionsViewModel();
            _windowManager = IoC.Get<IWindowManager>();
        }
        
        private void InitializeHotKeys()
        {
            var interopHelper = new WindowInteropHelper(new Window());
            var _ejectHotKey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, CustomKeys.Z, interopHelper);
            if (Properties.Settings.Default.EjectHotKey == String.Empty)
            {
                //_ejectHotKey = new SerializableHotkey(ModifierKeys.Control | ModifierKeys.Alt, Key.Z);
                //_optionsHotKey = new SerializableHotkey(ModifierKeys.Control | ModifierKeys.Alt, Key.O);
            }
            _ejectHotKey.HotKeyPressed += (h) =>
            {
                Console.Beep();
                var ejected = new List<String>();
                foreach (var device in DeviceManager.ListUsbDevices())
                {
                    if (device.IsMounted)
                    {
                        //DeviceManager.EjectDrive(device);
                        try
                        {
                            DeviceManager.EjectVolumeDevice(device.DriveLetters[0]);
                            ejected.Add(device.Caption);
                        }
                        catch (Win32Exception e)
                        {
                            // possible cause, file being used by another device ?
                            ToastNotification.Toast(Properties.Resources.UnableRemove);
                        }
                        
                    }
                }
                var toastMessage = "";
                if (ejected.Count < 1)
                {
                    toastMessage = Properties.Resources.NoDriveRemoved;
                }
                else if( ejected.Count == 1) {
                    toastMessage = $"{ejected[0]} {Properties.Resources.WasRemoved}";
                }else
                {
                    toastMessage = $"{Properties.Resources.Removed.ToLower()} ";
                    for (var i = 0; i < ejected.Count; i++)
                    {
                        if (i == 0)
                            toastMessage = $"{ejected[i]}";
                        else if(i == ejected.Count - 1 && i != 0)
                            toastMessage = $" {Properties.Resources.And} {ejected[i]}";
                        else
                            toastMessage = $", {ejected[i]}";
                    }
                }

                ToastNotification.Toast(toastMessage);
            };
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            NotifyOfPropertyChange(() => CanShowWindow);
            NotifyOfPropertyChange(() => CanHideWindow);
        }

        public void ShowWindow()
        {
            _optionsViewModel = new OptionsViewModel();
            _windowManager.ShowWindow(_optionsViewModel);

            NotifyOfPropertyChange(() => CanShowWindow);
            NotifyOfPropertyChange(() => CanHideWindow);
        }

        public bool CanShowWindow
        {
            get
            {
                return (!_optionsViewModel.IsActive);
            }
        }

        public void HideWindow()
        {
            _optionsViewModel.TryClose();

            NotifyOfPropertyChange(() => CanShowWindow);
            NotifyOfPropertyChange(() => CanHideWindow);
        }

        public bool CanHideWindow
        {
            get
            {
                return (_optionsViewModel.IsActive);
            }
        }

        public TaskbarIcon TaskbarIcon
        {
            get
            {
                return _taskbarIcon;
            }

            set
            {
                _taskbarIcon = value;
                _taskbarIcon.MouseDown += (s,e) => {
                    ShowWindow();
                };
                _taskbarIcon.TrayMouseDoubleClick += (s, e) => {
                    ShowWindow();
                 };
                NotifyOfPropertyChange(() => TaskbarIcon);
            }
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private TaskbarIcon _taskbarIcon;
        private readonly IWindowManager _windowManager;
        private OptionsViewModel _optionsViewModel;
    }
}
