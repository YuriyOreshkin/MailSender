using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Models
{
    public class MAIL
    {
        public MAIL()
        {
            JOURNAL = new HashSet<MAILSENT>();
            REGULATIONS = new HashSet<REGULATION>();
        }
        public string FROM { get; set; }
        public string SENDTO { get; set; }
        public string SUBJECT { get; set; }
        public string BODY { get; set; }
        public DateTime DATE { get; set; }
        public string MESSAGEID { get; set; }
        public bool ISSENT { get; set; }
        public bool ISPARSE { get; set; } = true;
        public ICollection<REGULATION> REGULATIONS { get; set; }
        public ICollection<MAILSENT> JOURNAL { get; set; }
    }
}
