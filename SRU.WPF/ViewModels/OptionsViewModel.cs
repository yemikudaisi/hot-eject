using Caliburn.Micro;
using Sru.Core;
using Sru.Core.Usb;
using Sru.Wpf.Infrastructure;
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
        private IList<UsbDevice> _devices;
        private readonly PreferencesViewModel _preferencesViewModel;

        public OptionsViewModel()
        {            
            _preferencesViewModel = new PreferencesViewModel();
            ResetDeviceList();
        }

        private void ResetDeviceList()
        {
            Devices = DeviceManager.ListUsbDevices();
        }

        public void  Eject(UsbDevice usbDevice)
        {
            DeviceManager.EjectDrive(usbDevice);
            ToastNotification.Toast($"{usbDevice.Caption} {Properties.Resources.Removed.ToLower()}");
                
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            ResetDeviceList();
            NotifyOfPropertyChange(() => CanShowPreferences);
        }

        public IList<UsbDevice> Devices
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
