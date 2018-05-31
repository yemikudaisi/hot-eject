using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.Infrastructure
{
    public class Settings
    {
        /// <summary>
        /// Index for fetching settings with string key(property names)
        /// </summary>
        /// <param name="index">The string key for settings property</param>
        /// <returns>The value of the property</returns>
        public object this[string index]
        {
            get {
                try
                {

                    Type type = Properties.Settings.Default.GetType();
                    PropertyInfo info = type.GetProperty(index);
                    if (info == null)
                    {
                        return null;
                    }
                    var obj = info.GetValue(Properties.Settings.Default);
                    return obj;
                }
                catch (System.ComponentModel.Composition.CompositionException e)
                {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException("Property", index, "The property does not exist");
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                }
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
