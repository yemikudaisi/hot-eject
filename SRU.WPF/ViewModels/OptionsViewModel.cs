using Caliburn.Micro;
using Sru.Core;
using Sru.Core.Usb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sru.Wpf.ViewModels
{
    public class OptionsViewModel : Screen
    {
        IList<Device> _devices;
        PreferencesViewModel _preferencesViewModel;

        public OptionsViewModel()
        {
            Devices = VolumeManager.GetDeviceClass().Devices;
            _preferencesViewModel = new PreferencesViewModel();
            ResetDeviceList();
        }

        private void ResetDeviceList()
        {
            Devices = VolumeManager.GetDeviceClass().Devices;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ResetDeviceList();
            NotifyOfPropertyChange(() => CanShowPreferences);
        }

        public IList<Device> Devices
        {
            get
            {
                return _devices;
            }

            set
            {
                _devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }

        public void ShowPreferences()
        {
            IoC.Get<IWindowManager>().ShowDialog(_preferencesViewModel);
            NotifyOfPropertyChange(() => CanShowPreferences);
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public bool CanShowPreferences
        {
            get
            {
                return (!_preferencesViewModel.IsActive);
            }
        }
    }
}
