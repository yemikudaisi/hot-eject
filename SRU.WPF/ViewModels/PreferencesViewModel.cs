using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf.ViewModels
{
    public class PreferencesViewModel : Conductor<object>
    {
        readonly IList<Screen> _screenCollection;
        Screen _selectedScreen;

        public PreferencesViewModel()
        {
            _screenCollection = new List<Screen>();
            _screenCollection.Add(new BasicPreferencesViewModel());
            _screenCollection.Add(new HotkeysPreferencesViewModel());
            SelectedScreen = _screenCollection[0];
            NotifyOfPropertyChange(() => ScreenCollection );
            Deactivated += (s, e) =>
            {
                Properties.Settings.Default.Save();
            };
        }

        public IList<Screen> ScreenCollection
        {
            get
            {
                return _screenCollection;
            }
        }

        public Screen SelectedScreen
        {
            get
            {
                return _selectedScreen;
            }

            set
            {
                _selectedScreen = value;
                ActivateItem(_selectedScreen);
                NotifyOfPropertyChange(() => SelectedScreen);
            }
        }
 
    }
}
