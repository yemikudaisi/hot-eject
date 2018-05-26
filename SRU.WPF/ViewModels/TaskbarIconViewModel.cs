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

namespace Sru.Wpf
{
    [Export(typeof(ITaskbarIconShell))]
    public class TaskbarIconViewModel : Screen, ITaskbarIconShell
    {
        public TaskbarIconViewModel()
        {
            InitializeHotKeys();
            _optionsViewModel = new OptionsViewModel();
            _windowManager = IoC.Get<IWindowManager>();
        }
        
        private void InitializeHotKeys()
        {
            var interopHelper = new WindowInteropHelper(new Window());
            var _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.Z, interopHelper);
            _hotkey.HotKeyPressed += (h) =>
            {
                Console.Beep();
                var ejected = VolumeManager.EjectRemovableDevices();
                var toastMessage = "";
                if (ejected.Count < 1)
                {
                    toastMessage = Properties.Resources.NoDriveRemoved;
                }
                else if( ejected.Count == 1) {
                    toastMessage = $"{ejected[0].Description} {ejected[0].LogicalDrive} {Properties.Resources.Removed}";
                }else
                {
                    toastMessage = $"{Properties.Resources.Removed.ToLower()} ";
                    for (var i = 0; i < ejected.Count; i++)
                    {
                        var d = ejected[i];
                        if (i == 0)
                            toastMessage = $"{d.LogicalDrive}";
                        else if(i == ejected.Count - 1 && i != 0)
                            toastMessage = $" {Properties.Resources.And} {d.LogicalDrive}";
                        else
                            toastMessage = $", {d.LogicalDrive}";
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
