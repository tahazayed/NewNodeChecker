using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNodeChecker.Models
{
    public class CommonBusinessEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVesion { get; set; }

        [Required]
        public DateTime EventDateTime { get; set; }

        public string Exception { get; set; }


        public CommonBusinessEntity()
        {
            EventDateTime = DateTime.Now;
        }
    }
}
