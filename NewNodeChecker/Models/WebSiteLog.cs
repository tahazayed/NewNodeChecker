using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class WebSiteLog : LogBase
    {
        [Required]
        public string AppPhysicalPath { get; set; }

        public string SiteName { get; set; }
        public string VirtualDirectoryName { get; set; }
        public virtual List<WebSiteFileLog> WebSiteFileLogs { get; set; }
    }
}
