using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf.Localization
{
    public class LocaleManager
    {

        public void SetLocale(String locale)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU" + locale);
        }

        public String this[String i]
        {
            get
            {
                return " locale tezt";
            }
        }
    }
}
