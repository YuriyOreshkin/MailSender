using Domino;
using MailSender.Models;
using MailSender.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MailSender.Services.Scheduler
{
    public class XMLSchedulerServiceConfig : ISchedulerServiceConfig
    {
        private string filename;

        
        public XMLSchedulerServiceConfig(string _filename)
        {
          
            filename = _filename;
        }

        public void SaveSettings(SCHEDULERSETTINGS settings)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(SCHEDULERSETTINGS));
            TextWriter writer = new StreamWriter(filename, false, Encoding.GetEncoding(1251));
            formatter.Serialize(writer, settings);
            writer.Close();

        }

        public SCHEDULERSETTINGS ReadSettings()
        {
            if (File.Exists(filename))
            {

                XmlSerializer formatter = new XmlSerializer(typeof(SCHEDULERSETTINGS));

                using (StreamReader fs = new StreamReader(filename, Encoding.GetEncoding(1251), false))
                {
                    SCHEDULERSETTINGS settings = (SCHEDULERSETTINGS)formatter.Deserialize(fs);
                    fs.Close();
                    return settings;
                }
            }
            else
            {
                SCHEDULERSETTINGS settings = new SCHEDULERSETTINGS
                {
                    ENABLE = false,
                    INTERVALINMINUTES = 5,
                    STARTINGDAILYAT = new TimeSpan(0, 0, 0).ToString(),
                    ENDINGDAILYAT = new TimeSpan(23, 59, 59).ToString(),
                    ONDAYSOFTHEWEEK ="1|2|3|4|5|6|7"
                   
                };

                SaveSettings(settings);

                return settings;
            }

        }
       
    }
}
