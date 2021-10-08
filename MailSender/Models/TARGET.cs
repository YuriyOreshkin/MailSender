using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Models
{
    public class TARGET
    {
        public long ID { get; set; }
        public string NAME { get; set; }
        public bool HASCHILDREN { get; set; }
        public long? PARENTID { get; set; }
    }
}
