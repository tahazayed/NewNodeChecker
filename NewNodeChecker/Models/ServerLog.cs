using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class ServerLog : CommonBusinessEntity
    {
        [Required]
        [MaxLength(500)]
        public string MachineName { get; set; }
        public string Ip { get; set; }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public virtual List<WebSiteLog> WebSiteLogs { get; set; }
        public virtual List<HostsFileLog> HostsFileLogs { get; set; }
        public virtual List<InstalledAppLog> InstalledAppLogs { get; set; }
        public virtual List<SqlTransResultLog> SqlTransResultLogs { get; set; }
    }
}
