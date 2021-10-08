using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MailSender.Models
{
    public class REGULATION
    {
        
        public long ID { get; set; }
        public string NAME { get; set; }
        public string FILTER { get; set; }
        public bool DISABLED { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long? PARENTID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public short? ISEDIT { get; set; }

        public virtual ICollection<REGULATIONTARGET> REGULATIONTARGETS { get; set; }
    }
}
