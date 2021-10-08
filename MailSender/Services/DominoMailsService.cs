using Domino;
using MailSender.Extensions;
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
    public class DominoMailsService : IMailsService
    {
        
        private ISenderService sender;
        private IRepository repository;
        private IMailsServerService server;
    

        public DominoMailsService(IMailsServerService _server, ISenderService _sender, IRepository _repository)
        {
           
            sender = _sender;
            repository= _repository;
            server = _server;

        }


        public IQueryable<MAIL> GetMails(DateTime datebegin, DateTime dateend)
        {
            IList<MAIL> result = new List<MAIL>();
            //NotesDatabase db = Open();
            var db = server.GetServer() as NotesDatabase;

            if (db != null)
            {
                NotesView viewSend = db.GetView("$Sent");

                for (int i = viewSend.AllEntries.Count; i >= 1; i--)
                {
                    NotesDocument notesmail = viewSend.GetNthDocument(i);
                    
                    DateTime date = DateTime.Parse(notesmail.GetFirstItem("PostedDate").Text);

                    if (date >= datebegin && date <= dateend)
                    {
                        if (!result.Any(mail => mail.MESSAGEID == notesmail.GetFirstItem("$MessageID").Text))
                        {
                            try
                            {
                                MAIL mail = NotesToModel(notesmail);

                                result.Add(mail);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        else
                        {
                            //Delete double
                            notesmail.Remove(false);

                        }
                    }
                }
            }

            return result.AsQueryable();

        }

        public IQueryable<MAIL> GetInbox(DateTime datebegin, DateTime dateend) 
        {
            datebegin = new DateTime(datebegin.Year, datebegin.Month, datebegin.Day, 0, 0, 0);
            dateend = new DateTime(dateend.Year, dateend.Month, dateend.Day, 23, 59, 59);

            return GetMails(datebegin, dateend).Parse(GetRegulations(), GetDefaultRegulation());
            
        }

        public void MarkSent(MAIL mail)
        {
            NotesDocument notes = GetNotesDocument(mail);

            if (notes != null)
            {
                 notes.ReplaceItemValue("isSent",mail.ISSENT.ToString());
                

                notes.Save(false, false, true);
            }
        }


        private List<MAILSENT> GetJournal(NotesDocument notes)
        {
            List<MAILSENT> result = new List<MAILSENT>();

            if (!notes.HasItem("Journal"))
            {
                notes.AppendItemValue("Journal", null);
            }

            var values = (object[])notes.GetItemValue("Journal");

            if (values[0] != null)
            {
                for (int i = 0; i <= (values).Length - 1; i++)
                {
                    var text= (string)values[i];
                    result.Add(new MAILSENT {  TARGETID =long.Parse(text.Split('|')[0]), SENTDATE= DateTime.Parse(text.Split('|')[1]) });
                }

                return result;
            }

            return result;
        }


        private bool IsSent(NotesDocument notes)
        {
            if (!notes.HasItem("isSent"))
            {
                notes.AppendItemValue("isSent", "false");
            }

            return Boolean.Parse(notes.GetFirstItem("isSent").Text);
        }


        private NotesDocument GetNotesDocument(MAIL mail)
        {

            var db = server.GetServer() as NotesDatabase;

            if (db != null)
            {
                NotesView viewSend = db.GetView("$Sent");
                for (int i = viewSend.AllEntries.Count; i >= 1; i--)
                {
                    NotesDocument notesmail = viewSend.GetNthDocument(i);
                    if (notesmail.GetFirstItem("$MessageID").Text == mail.MESSAGEID)
                    {
                        return notesmail;
                    }
                }
            }

            return null;
        }


     


        private string ConvertAddresses(string text)
        {
            string[] addresses = text.Split(';');
            for (int i = 0; i < addresses.Length; i++)
            {
                string[] sections = addresses[i].Split('/');

                for (int j=0; j< sections.Length;j++)
                {
                    if (sections[j].IndexOf("=") > -1)
                    {
                        sections[j] = sections[j].Substring(sections[j].IndexOf("=")+1);
                    }

                }

                addresses[i] = String.Join("/", sections); 
            }

            return String.Join(";", addresses);
        }

        private MAIL NotesToModel(NotesDocument notesmail)
        {
            MAIL mail = new MAIL()
            {
                MESSAGEID = notesmail.GetFirstItem("$MessageID").Text,
                FROM =ConvertAddresses(notesmail.GetFirstItem("From").Text),
                SENDTO = ConvertAddresses(notesmail.GetFirstItem("SendTo").Text),
                DATE = DateTime.Parse(notesmail.GetFirstItem("PostedDate").Text),
                SUBJECT = String.IsNullOrEmpty(notesmail.GetFirstItem("Subject").Text) ? "" : notesmail.GetFirstItem("Subject").Text,
                BODY = String.IsNullOrEmpty(notesmail.GetFirstItem("Body").Text) ? "" : notesmail.GetFirstItem("Body").Text,
                ISSENT = IsSent(notesmail)
            };

            return mail;

        }

        public MAIL GetMailByID(string messageID)
        {
            messageID = "<" + messageID + ">";

            var db = server.GetServer() as NotesDatabase;

            if (db != null)
            {
                NotesView viewSend = db.GetView("$Sent");
                for (int i = viewSend.AllEntries.Count; i >= 1; i--)
                {
                    NotesDocument notesmail = viewSend.GetNthDocument(i);
                    if (notesmail.GetFirstItem("$MessageID").Text == messageID)
                    {
                        MAIL mail = NotesToModel(notesmail);

                        mail.JOURNAL = GetJournal(notesmail);
                        mail = new[] { mail }.AsQueryable<MAIL>().Parse(GetRegulations(), GetDefaultRegulation()).FirstOrDefault();

                        return mail;
                    }
                }
            }

            return null;
        }

        public void Delete(string messageID)
        {
            messageID = "<" + messageID + ">";

            var db = server.GetServer() as NotesDatabase;

            if (db != null)
            {
                NotesView viewSend = db.GetView("$Sent");
                for (int i = viewSend.AllEntries.Count; i >= 1; i--)
                {
                    NotesDocument notesmail = viewSend.GetNthDocument(i);
                    if (notesmail.GetFirstItem("$MessageID").Text == messageID)
                    {
                        //Delete double
                        notesmail.Remove(false);
                    }
                }
            }
        }



        private string ConvertHTMLText(string text)
        {
            HtmlToText http = new HtmlToText();

            var plainText = http.ConvertHtml(text);

            return plainText;
        }

        private string StyleText(string text)
        {
        //    string[] bodies = text.Split(Environment.NewLine.ToCharArray());

        //    for (var i = 0; i < bodies.Length; i++) 
        //    {
        //        bodies[i] = bodies[i].PadLeft(20);
        //    }

        //    return String.Join(" \r\n", bodies);

           return text.Replace("\r\n", " \r\n");
        }


        public void SendOut(MAIL mail)
        {
            StringBuilder messageText = new StringBuilder();
            messageText.Append(" \r\n");
            messageText.AppendFormat(">> Дата письма: {0: dd.MM.yyyy HH:mm}", mail.DATE.ToString());
            messageText.Append(" \r\n");
            messageText.AppendFormat(">> Тема письма: {0}", mail.SUBJECT != null ? mail.SUBJECT : "");
            messageText.Append(" \r\n");
            messageText.AppendFormat(">> Категория : {0}", String.Join(",", mail.REGULATIONS.Select(r=>r.NAME)));
            messageText.AppendFormat(" \r\n \r\n");
            messageText.AppendFormat("{0}",StyleText(ConvertHTMLText(mail.BODY)));
            messageText.Append(" \r\n");


            //string messageText = "\r\nДата письма: " + mail.DATE.ToString("dd.MM.yyyy HH:mm")+ "\r\nТема письма: " + mail.SUBJECT != null ? mail.SUBJECT : "" +  "\r\nОпубликовано: Автоматическая сортировка (" + comment.DATE + ")" + "\r\n\r\n" + htmlAsText;

            sender.SendMessage(mail.JOURNAL.Select(s => s.TARGETID), messageText.ToString());

            NotesDocument notes = GetNotesDocument(mail);

            if (notes != null)
            {
                var journal = GetJournal(notes);
                //FullJournal
                notes.RemoveItem("Journal");
                 
                notes.AppendItemValue("Journal",journal.Concat(mail.JOURNAL).Select(s=> s.TARGETID.ToString() +"|" + s.SENTDATE.ToString()).ToArray<string>());

                //If not parse regulation  isSetn= false
                if(!mail.REGULATIONS.Contains(GetDefaultRegulation()))
                         mail.ISSENT = true;

                MarkSent(mail);
            }
        }

        public IQueryable<REGULATION> GetRegulations()
        {
            return repository.GetRegulations();
        }


        public REGULATION GetDefaultRegulation()
        {
            return repository.GetDefaultRegulation();
        }

       
    }
}
