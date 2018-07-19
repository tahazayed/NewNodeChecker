using System;

namespace NewNodeChecker.Models
{
    public class InstalledAppLog : LogBase
    {
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InstallSource { get; set; }
    }
}
