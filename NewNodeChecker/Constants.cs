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
        public const string IisRegKey = @"Software\Microsoft\InetStp";
        public const string IisMajorVersionPropertyName = "MajorVersion";
        public const string IisMinorVersionPropertyName = "MinorVersion";

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
        public const string IisWebVirtualDirSchemaClassName = "IIsWebVirtualDir";
        public const string IisWebVirtualDirPathPropertyName = "Path";

        //Regex
        public const string RegexIp = @"key=""([^\\""]*)""\s*value=""(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})""";
        public const string RegexUrl = @"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?";
        public const string RegexConnectionString1St = @"connectionString=""([^""]*)""";
        public const string RegexConnectionString2Nd = "data source=([^;]*);initial catalog=([^;]*);user id=([^;]*);password=([^;]*);";

        //Misc
        public const string Succeed = "Succeed";
        public const string Failed = "Failed";
        public const string Entity = "Entity";
        public const string ConnectionStringFormat = "server={0};DataBase={1};UID={2};PWD={3};";
        public const string ProgramPathFormat1St = "\"{0}\",\"{1}\",\"{2}\",\"{3}\"";
        public const string ProgramPathFormat2Nd = "\"\",\"\",\"\",\"\"";
        public const string SqlSimpleStatement = "select getdate()";

    }
}
