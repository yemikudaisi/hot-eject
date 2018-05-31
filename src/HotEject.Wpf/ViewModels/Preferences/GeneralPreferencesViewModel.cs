using Caliburn.Micro;
using HotEject.Wpf.Infrastructure;
using HotEject.Wpf.Localization;
using HotEject.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.ViewModels
{
    public class GeneralPreferencesViewModel : Screen
    {
        Language _selectedLanguage;

        IList<Language> _supportedLanguages;
        public GeneralPreferencesViewModel()
        {
            DisplayName = "Basic";
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
                NotifyOfPropertyChange(() => EnableRunOnStartup);
            }
        }
    }
}
