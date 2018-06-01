using System;

namespace HotEject.Wpf
{
    public static class ApplicationsConstants
    {
        public const string APP_NAME = "Hot Eject";
        public const string APP_WEBSITE = "https://yemikudaisi.github.io/hot-eject/";
        public const string APP_WEBSITE_HELP = ApplicationsConstants.APP_WEBSITE+"help";

        public const System.Windows.Input.ModifierKeys DEFAULT_HOT_KEY_MODIFIERS = System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt ;
        public const System.Windows.Input.Key DEFAULT_EJECT_KEY = System.Windows.Input.Key.Z;
        public const System.Windows.Input.Key DEFAULT_OPTIONS_KEY = System.Windows.Input.Key.O;

        public static readonly Microsoft.Win32.RegistryKey RUN_REGISTRY_KEY = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
    }
}
