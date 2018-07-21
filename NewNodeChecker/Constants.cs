namespace NewNodeChecker
{
    internal static class Constants
    {
        //Registry Keys
        public const string InstalledApps32BitRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        public const string InstalledApps64BitRegKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        public const string WindowsUpdatesRegKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Packages";
        public const string DisplayNamePropertyName = "DisplayName";
        public const string DisplayVersionPropertyName = "DisplayVersion";
        public const string InstallDatePropertyName = "InstallDate";
        public const string InstallSourcePropertyName = "InstallSource";
        public const string InstallDateFormate = "yyyyMMdd";

        //File paths
        public const string HostsFilePath = @"drivers\etc\hosts";

        //File Extensions
        public const string DllFileExtension = ".dll";
        public const string ExeFileExtension = ".exe";
        public const string AllConfigFilesExtension = "*.config";

        //IIS
        public const string IisBaseUrl = "IIS://localhost/W3SVC";
        public const string IisSchemaClassName = "IIsWebServer";
        public const string IisServerCommentPropertyName = "ServerComment";
        public const string IisServerStatePropertyName = "ServerState";
        public const string IisForwardSlash = "/";
        public const string IisExecutableFolder = "bin";



    }
}
