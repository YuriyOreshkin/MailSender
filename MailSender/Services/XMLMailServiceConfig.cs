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

namespace MailSender.Services
{
    public class XMLMailServiceConfig : IMailServiceConfig
    {
        private string filename;

        
        public XMLMailServiceConfig(string _filename)
        {
          
            filename = _filename;
        }

        public void SaveSettings(MAILSSERVICESETTINGS settings)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(DOMINOSETTINGS));
            TextWriter writer = new StreamWriter(filename, false, Encoding.GetEncoding(1251));
            formatter.Serialize(writer, settings);
            writer.Close();

        }

        public MAILSSERVICESETTINGS ReadSettings()
        {
            if (File.Exists(filename))
            {

                XmlSerializer formatter = new XmlSerializer(typeof(DOMINOSETTINGS));

                using (StreamReader fs = new StreamReader(filename, Encoding.GetEncoding(1251), false))
                {
                    DOMINOSETTINGS settings = (DOMINOSETTINGS)formatter.Deserialize(fs);
                    fs.Close();
                    return settings;
                }
            }
            else
            {
                DOMINOSETTINGS settings = new DOMINOSETTINGS
                {
                    PASSWORD = "007-0822",
                    SERVER = @"s007/007/PFR/RU",
                    DBNAME = @"mail\007000-0800008.nsf"
                };

                SaveSettings(settings);

                return settings;
            }

        }
       
    }
}
