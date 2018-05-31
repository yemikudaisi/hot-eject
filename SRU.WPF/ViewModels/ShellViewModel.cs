using Caliburn.Micro;
using Sru.Wpf.Controls;
using Sru.Wpf.Input;
using Sru.Wpf.ViewModels;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Interop;
using Hardcodet.Wpf.TaskbarNotification;
using Sru.Core;
using Sru.Wpf.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel;
using Sru.Wpf.Extensions;
using Sru.Wpf.Events;

namespace Sru.Wpf
{
    [Export(typeof(ITaskbarIconShell))]
    public class ShellViewModel : Screen, ITaskbarIconShell, IHandle<PreferenceChangeEvent>
    {
        SerializableHotKey _ejectHotKey;
        SerializableHotKey _optionsHotKey;

        /// <summary>
        /// Public default constructor for ShelLViewModel
        /// </summary>
        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            var _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _ejectHotKey = new SerializableHotKey(ApplicationsConstants.DEFAULT_HOT_KEY_MODIFIERS, ApplicationsConstants.DEFAULT_EJECT_KEY);
            _optionsHotKey = new SerializableHotKey(ApplicationsConstants.DEFAULT_HOT_KEY_MODIFIERS, ApplicationsConstants.DEFAULT_OPTIONS_KEY);
            RegisterHotKeys();
            _optionsViewModel = new OptionsViewModel();
            _windowManager = IoC.Get<IWindowManager>();
        }
        
        #region Event handlers
        /// <summary>
        /// Options hot key pressed event handler
        /// </summary>
        /// <param name="obj">The hotkey that was pressed</param>
        private void OptionsHotKeyPressed(SerializableHotKey obj)
        {
            NotifyHotKeyAction();

            if (CanShowOptions)
                ShowOptions();
            else if(CanHideOptions)
                HideOptions();
        }

        /// <summary>
        /// Eject device hot key event handler
        /// </summary>
        /// <param name="obj">The hotkey that was pressed</param>
        private void EjectHotKeyPressed(SerializableHotKey obj)
        {
            NotifyHotKeyAction();
            var ejected = new List<String>();
            foreach (var device in DeviceManager.ListUsbDevices())
            {
                if (device.IsMounted)
                {
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
            HandleEjectToast(ejected);
        }

        /// <summary>
        /// Screen activate event handler
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();

            NotifyOfPropertyChange(() => CanShowOptions);
            NotifyOfPropertyChange(() => CanHideOptions);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show the notification tray icon options menu
        /// </summary>
        public void ShowOptions()
        {
            _optionsViewModel = new OptionsViewModel();
            _windowManager.ShowWindow(_optionsViewModel);

            NotifyOfPropertyChange(() => CanShowOptions);
            NotifyOfPropertyChange(() => CanHideOptions);
        }

        /// <summary>
        /// Hide the notification tray icon options menu
        /// </summary>
        public void HideOptions()
        {
            _optionsViewModel.TryClose();

            NotifyOfPropertyChange(() => CanShowOptions);
            NotifyOfPropertyChange(() => CanHideOptions);
        }

        /// <summary>
        /// Terminates the application execution
        /// </summary>
        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles hot key (re)assignment event
        /// </summary>
        /// <param name="message">The event that occured</param>
        public void Handle(PreferenceChangeEvent message)
        {
            RegisterHotKeys();
        }
        #endregion

        #region Enabling flags

        /// <summary>
        /// Show option action guard
        /// </summary>
        public bool CanShowOptions
        {
            get
            {
                return (!_optionsViewModel.IsActive);
            }
        }

        /// <summary>
        /// Hide option action guard
        /// </summary>
        public bool CanHideOptions
        {
            get
            {
                return (_optionsViewModel.IsActive);
            }
        }
        #endregion

        #region Private Helpers
        
        /// <summary>
        /// Initialize the pre configured hot keys or assign a default
        /// </summary>
        private void RegisterHotKeys()
        {
            // Required for the handle arg of the native call to register hot keys
            var interopHelper = new WindowInteropHelper(new Window());

            var base64String = Properties.Settings.Default.EjectHotKey;
            // if a previous eject hot key value has been saved in the setting assign it to _ejectHotKey
            // and if the saved preference is not the same as the current value
            // The latter, is necessary to avoid re-registering same hotkey
            if (base64String.CanFromBase64String<SerializableHotKey>() && 
                base64String.FromBase64String<SerializableHotKey>() != _ejectHotKey)
            {
                _ejectHotKey = base64String.FromBase64String<SerializableHotKey>();
            }

            base64String = Properties.Settings.Default.OptionsHotKey;
            // if a previous options hot key value has been saved in the setting assign it to _optionsHotKey
            if (base64String.CanFromBase64String<SerializableHotKey>() &&
                base64String.FromBase64String<SerializableHotKey>() != _optionsHotKey)
            {
                _optionsHotKey = base64String.FromBase64String<SerializableHotKey>();
            }


            _optionsHotKey.Handle = interopHelper.Handle;
            _ejectHotKey.Handle = interopHelper.Handle;
            _optionsHotKey.Pressed += OptionsHotKeyPressed;
            _ejectHotKey.Pressed += EjectHotKeyPressed;
        }

        /// <summary>
        /// Show the respective toast depending on the amount of device ejected
        /// </summary>
        /// <param name="ejected">The labels of devices that were ejected</param>
        private void HandleEjectToast(List<String> ejected)
        {
            var toastMessage = "";
            if (ejected.Count < 1)
            {
                toastMessage = Properties.Resources.NoDriveRemoved;
            }
            else if (ejected.Count == 1)
            {
                toastMessage = $"{ejected[0]} {Properties.Resources.WasRemoved}";
            }
            else
            {
                toastMessage = $"{Properties.Resources.Removed.ToLower()} ";
                for (var i = 0; i < ejected.Count; i++)
                {
                    if (i == 0)
                        toastMessage = $"{ejected[i]}";
                    else if (i == ejected.Count - 1 && i != 0)
                        toastMessage = $" {Properties.Resources.And} {ejected[i]}";
                    else
                        toastMessage = $", {ejected[i]}";
                }
            }

            ToastNotification.Toast(toastMessage);
        }

        /// <summary>
        /// Alert user before a hot key action is carried out
        /// </summary>
        private void NotifyHotKeyAction()
        {
            Console.Beep();
        }
        #endregion

        /// <summary>
        /// Sets or gets the taskbar notification icon
        /// </summary>
        public TaskbarIcon TaskbarIcon
        {
            get
            {
                return _taskbarIcon;
            }

            set
            {
                _taskbarIcon = value;
                
                
                _taskbarIcon.MouseUp += (s,e) => {
                    ShowOptions();
                };
                _taskbarIcon.TrayMouseDoubleClick += (s, e) => {
                    ShowOptions();
                 };
                NotifyOfPropertyChange(() => TaskbarIcon);
            }
        }


        private TaskbarIcon _taskbarIcon;
        private readonly IWindowManager _windowManager;
        private OptionsViewModel _optionsViewModel;
    }
}
