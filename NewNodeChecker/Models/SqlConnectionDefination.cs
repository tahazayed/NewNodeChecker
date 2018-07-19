using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewNodeChecker.Models
{
    public class SqlConnectionDefination : DefinationBase
    {

        [Required]
        public string SqlConnection { get; set; }

        [Required]
        public string SqlStatment { get; set; }

        public virtual List<SqlTransResultLog> SqlTransResultLogs { get; set; }
    }
}
