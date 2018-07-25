using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class InstalledAppLog : LogBase
    {
        [Index]
        [MaxLength(500)]
        public string DisplayName { get; set; }
        [Index]
        [MaxLength(100)]
        public string DisplayVersion { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InstallSource { get; set; }
    }
}
