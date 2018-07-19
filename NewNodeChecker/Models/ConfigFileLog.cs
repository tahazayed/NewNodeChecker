using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
namespace NewNodeChecker.Models
{
    public class ConfigFileLog : CommonBusinessEntity
    {
        public int WebSiteLogId { get; set; }

        public string Type { get; set; }

        [Required]
        public string ConfigFileName { get; set; }
        [Column(TypeName = "xml")]
        public string ConfigFileContent { get; set; }
        [NotMapped]
        public XElement ConfigFileNameWrapper
        {
            get { return XElement.Parse(ConfigFileName); }
            set { ConfigFileName = value.ToString(); }
        }
       

        //public XmlAttribute ConfigFileContent { get; set; }

        public DateTime? LastModificationDate { get; set; }


        [ForeignKey("WebSiteLogId")]
        public virtual WebSiteLog WebSiteLog { get; set; }
    }
}
