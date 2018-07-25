using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class LogBase : CommonBusinessEntity
    {
        [Required]
        [Index]
        public int ServerLogId { get; set; }

        [ForeignKey("ServerLogId")]
        public virtual ServerLog ServerLog { get; set; }
    }
}
