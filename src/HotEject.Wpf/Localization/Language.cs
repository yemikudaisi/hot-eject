using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.Localization
{
    [Serializable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Language
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public String Lcid { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Public no argument constructor, defaults to english (US)
        /// </summary>
        public Language()
          :  this("en-US", "English (United States)")
        {

        }

        public Language(String lcid, String description)
        {
            Lcid = lcid;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;

            var lang = (Language)obj;
            return (this.Lcid == lang.Lcid);
        }
    }
}
