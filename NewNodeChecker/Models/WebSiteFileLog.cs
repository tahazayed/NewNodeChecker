using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class WebSiteFileLog : CommonBusinessEntity
    {
        [Required]
        public int WebSiteLogId { get; set; }

        public string PhysicalPath { get; set; }

        [Required]
        public string FileName { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }
        public string BuildNo { get; set; }

        public DateTime? LastModificationDate { get; set; }


        [ForeignKey("WebSiteLogId")]
        public virtual WebSiteLog WebSiteLog { get; set; }
    }
}
