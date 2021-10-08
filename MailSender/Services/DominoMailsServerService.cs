using Domino;
using MailSender.Models;
using MailSender.Services.Interfaces;
using MailSender.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MailSender.Services
{
    public class DominoMailsServerService : IMailsServerService
    {
        private ILogger logger;
        private IMailServiceConfig config;

        private NotesDatabase db;

        public DominoMailsServerService(ILogger _logger, IMailServiceConfig _config)
        {
            logger = _logger;
            config = _config;

              
            Connect(config.ReadSettings());
          
        }

        public void Connect(MAILSSERVICESETTINGS settings)
        {
            try
            {
              
                logger.Info(String.Format("Opening database (Server: {0} , Name={1})", ((DOMINOSETTINGS)settings).SERVER, ((DOMINOSETTINGS)settings).DBNAME));
                Open((DOMINOSETTINGS)settings);
                logger.Info("Open database " + db.FileName + " success!");
            }
             
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public object GetServer()
        {
            if (db == null)
                Connect(config.ReadSettings());

               return db;
        }

        public bool isConnected()
        {
            if (db != null)
                return db.IsOpen;

            return false;
        }

       

       private void Open(DOMINOSETTINGS settings)
        {

            if (settings != null && settings.PASSWORD != null )
            {
                
                   
                    NotesSession session = new NotesSession();
                    //Initializing Lotus Notes Session
                    session.Initialize(settings.PASSWORD);
                    //session.ConvertMime = false;
                    //Opening Lotus Notes DataBase Object

                     db = session.GetDatabase(settings.SERVER, settings.DBNAME, false);

                    if (db == null)
                    {
                         throw new Exception("Open database failed!");

                    }

            }
            else
            {
                throw new Exception("Could not read mail service settings!");

            }
        }
       
    }
}
