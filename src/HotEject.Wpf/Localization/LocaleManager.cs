// Copyright (c) 2018 Yemi Kudaisi for the SRU
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.Localization
{
    /// <summary>
    /// Sets the application locale and provide string values for assigned culture via an indexer property
    /// </summary>
    public class LocaleManager
    {
        /// <summary>
        /// Set the the application local to the specified value
        /// </summary>
        /// <param name="locale"></param>
        public void SetLocale(String locale)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(locale);
        }

        /// <summary>
        /// Indexer property for getting string values from resource file
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The value corresponding to the resource name supplied</returns>
        public String this[String name]
        {
            get
            {
                // TODO: implement this
                return " locale tezt";
            }
        }

        /// <summary>
        /// Get the list of languages supported
        /// </summary>
        public static IList<Language> SupportedLanguages
        {
            get
            {
                var result = new List<Language>();
                result.Add(new Language());
                result.Add(new Language() { Lcid = "yo-NG", Description = "Yoruba (Nigeria)" });
                return result;
            }
        }
    }
}
