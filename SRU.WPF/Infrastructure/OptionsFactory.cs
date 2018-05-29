using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf.Infrastructure
{
    public class OptionsFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The string key for settings property</param>
        /// <returns></returns>
        public object this[string index]
        {
            get {
                Type type = Properties.Settings.Default.GetType();
                PropertyInfo info = type.GetProperty(index);
                if (info == null) {
                    return null;
                }

                var obj = info.GetValue(Properties.Settings.Default);
                return obj;
            }

            set
            {
                Type type = Properties.Settings.Default.GetType();
                PropertyInfo info = type.GetProperty(index);
                if (info == null)
                {
                    return;
                }

                info.SetValue(Properties.Settings.Default, value);
            }
        }        
    }
}
