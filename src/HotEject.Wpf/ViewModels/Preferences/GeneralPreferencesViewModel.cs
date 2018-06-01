using Caliburn.Micro;
using HotEject.Wpf.Infrastructure;
using HotEject.Wpf.Localization;
using HotEject.Wpf.Extensions;
using System;
using System.Collections.Generic;

namespace HotEject.Wpf.ViewModels
{
    public class GeneralPreferencesViewModel : Screen
    {
        Language _selectedLanguage;

        IList<Language> _supportedLanguages;
        public GeneralPreferencesViewModel()
        {
            DisplayName = Properties.Resources.Basic;
            if ((String)Utilities.Settings["PreferredLanguage"] == "")
            {
                SelectedLanguage = new Language(); // note that this defaults to english
            }else
            {
                var lang = (String)Utilities.Settings["PreferredLanguage"];
                SelectedLanguage = lang.FromBase64String<Language>();
            }

            SupportedLanguages = LocaleManager.SupportedLanguages;
        }

        public Language SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }

            set
            {
                _selectedLanguage = value;
                Utilities.Settings["PreferredLanguage"] = value.ToBase64String();
                LocaleManager.SetLocale(value.Lcid);
                NotifyOfPropertyChange(() => SelectedLanguage);
            }
        }

        public IList<Language> SupportedLanguages
        {
            get
            {
                return _supportedLanguages;
            }

            set
            {
                _supportedLanguages = value;
                NotifyOfPropertyChange(()=> SupportedLanguages);
            }
        }

        public bool EnableRunOnStartup
        {
            get { return (bool)Utilities.Settings["EnableRunOnStartup"]; }

            set
            {
                Utilities.Settings["EnableRunOnStartup"] = value;
                if (value)
                {
                    ApplicationsConstants.RUN_REGISTRY_KEY.SetValue(ApplicationsConstants.APP_NAME, System.Reflection.Assembly.GetExecutingAssembly().Location);
                }else
                {
                    ApplicationsConstants.RUN_REGISTRY_KEY.DeleteValue(ApplicationsConstants.APP_NAME, false);
                }
                NotifyOfPropertyChange(() => EnableRunOnStartup);
            }
        }
    }
}
