using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotEject.Wpf.ViewModels
{
    public abstract class AppContextMenuBaseViewModel : Screen
    {

        private readonly PreferencesViewModel _preferencesViewModel;

        protected AppContextMenuBaseViewModel()
        {
            _preferencesViewModel = new PreferencesViewModel();
        }

        public void ShowAbout()
        {
            System.Diagnostics.Process.Start(ApplicationsConstants.APP_WEBSITE);
        }

        public void ShowHelp()
        {
            System.Diagnostics.Process.Start(ApplicationsConstants.APP_WEBSITE_HELP);
        }

        public void ShowPreferences()
        {
            IoC.Get<IWindowManager>().ShowDialog(_preferencesViewModel);
            NotifyOfPropertyChange(() => CanShowPreferences);
        }

        /// <summary>
        /// Terminates the application execution
        /// </summary>
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

        protected override void OnActivate()
        {
            NotifyOfPropertyChange(() => CanShowPreferences);
            base.OnActivate();
        }
    }
}
