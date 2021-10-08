using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MailSender.Models
{
    public class SCHEDULERSETTINGS 
    {
        [XmlAttribute]
        public bool ENABLE { get; set; }

        public int INTERVALINMINUTES { get; set; }
        public string STARTINGDAILYAT { get; set; }
        public string ENDINGDAILYAT { get; set; }
        public string ONDAYSOFTHEWEEK { get; set; }
    }
}
