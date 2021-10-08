using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Models
{
    public class REGULATIONTARGET
    {
        public long ID { get; set; }
        public long REGULATIONID { get; set; }
        public long TARGETID { get; set; }

        public virtual REGULATION REGULATION { get; set; }

    }
}
