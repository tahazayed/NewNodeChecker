namespace NewNodeChecker
{
   internal static class Constants
   {
       public const string InstalledApps32BitRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
       public const string InstalledApps64BitRegKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
       public const string WindowsUpdatesRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Packages";
    }
}
