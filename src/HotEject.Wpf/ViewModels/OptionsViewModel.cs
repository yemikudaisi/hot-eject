using Caliburn.Micro;
using HotEject.Core;
using HotEject.Core.Usb;
using HotEject.Wpf.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotEject.Wpf.ViewModels
{
    public class OptionsViewModel : AppContextMenuBaseViewModel
    {
        private IList<UsbDevice> _devices;
        private bool _windowIsVisible;

        public OptionsViewModel()
            : base()
        {
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
            ResetDeviceList();
            base.OnActivate();
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

        public bool WindowIsVisible
        {
            get
            {
                return _windowIsVisible;
            }

            set
            {
                _windowIsVisible = value;
                NotifyOfPropertyChange(() => WindowIsVisible);
            }
        }
    }
}
