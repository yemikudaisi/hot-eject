using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sru.Wpf.Infrastructure;
using System;

namespace Sru.Test
{
    [TestClass]
    public class OptionsFactoryTest
    {
        [TestMethod]
        public void GetIndex_WhenTheValue_ShouldBeEqual()
        {
            OptionsFactory factory = new OptionsFactory();
            var expected = "en-us";
            var actual = (string)factory["Language"];
            Assert.AreEqual(expected, actual,"Settings not equal");
        }

        [TestMethod]
        public void GetIndex_WhenTheType_ShouldBeEqual()
        {
            OptionsFactory factory = new OptionsFactory();
            var expected = typeof(bool);
            Assert.AreEqual(expected, factory["PreferEjectAll"].GetType(), "settings not same type");
        }
    }
}
