using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MailSender.Models;
using MailSender.Services.Interfaces;
using MailSender.Utils;

namespace MailSender.Services
{
    public class StaticMailsService : IMailsService
    {
        private List<MAIL> mails;
        private ISenderService sender;
        private IRepository repository;

        public StaticMailsService(ISenderService _sender,IRepository _repository)
        {
            sender = _sender;
            repository = _repository;  
        
            mails = new List<MAIL>()
            {
                new MAIL {

                    MESSAGEID ="<OF0A58CF24.0BE24CE5-ON43258049.005CB353-43258049.005CB357@LocalDomain>",
                    FROM = "CN=Кулагина А.С. 101-1177/OU=101/O=PFR/C=RU",
                    SENDTO = "all0801-Всем начальникам Управлений информационных технологий;CN=Гоцуцов С.Ю. 101-0105/OU=101/O=PFR/C=RU@101;CN=Алонцев В.В. 100-1571/OU=100/O=PFR/C=RU@100;CN=Кузнецова А.Н 101-0838/OU=101/O=PFR/C=RU@101;CN=Виноградов Н.С. 101-0855/OU=101/O=PFR/C=RU@101;CN=Федосеев В.А. 101-0864/OU=101/O=PFR/C=RU@101;Svetlana.Kalyuzhnova@redsys.ru;A.Kuznetsov@redsys.ru;057-1002@057.pfr.ru;CN=Мамин Р.А. 057-1001/OU=057/O=PFR/C=RU@057;CN=Ратимова Н.Г. 101-1001/OU=101/O=PFR/C=RU@101;CN=Коленчук Т.А. 101-1167/OU=10",
                    DATE = DateTime.Parse("2018-01-23 16:03:46.124203"),
                    SUBJECT = "ВКС по ЕГИССО",
                    BODY = "<font face=\"Default Sans Serif,Verdana,Arial,Helvetica,sans-serif\" size=\"2\"><div style=\"\"><font face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\" size=\"2\">Уведомляем Вас, что&nbsp;<font> 10.10 2016 с 13:00 до 16:00 будут проводиться технологические &nbsp; работы по актуализации БД, в связи с чем будет приостановлена обработка запросов на сервере ПТК СПУ центрального уровня(на C1100000 будет остановлено задание LSTNRCENT).</font><br><font> О возобновлении обработки будет сообщено дополнительно.</font></font></div><div style = \"\" ><font face = \"Times New Roman\" size = \"4\" ><br></font><font face = \"Sans Serif, Verdana, Arial, Helvetica, sans-serif\" size = \"2\" > С уважением,<br> Служба технической поддержки АИС ПФР<br>+7(499) 972 - 93 - 37</font></div><div></div></font>",
                    ISSENT=false
                },

                new MAIL {

                    MESSAGEID ="<OF2444FA1D.894419C9-ON4325804B.004E7EB0-4325804B.004F9C4A@LocalDomain>",
                    FROM = "CN=Лебедева Л.С 101-1174/OU=101/O=PFR/C=RU",
                    SENDTO = "all0801-Всем начальникам Управлений информационных технологий;CN=Гоцуцов С.Ю. 101-0105/OU=101/O=PFR/C=RU@101;CN=Кузнецова А.Н 101-0838/OU=101/O=PFR/C=RU@101;CN=Виноградов Н.С. 101-0855/OU=101/O=PFR/C=RU@101;CN=Федосеев В.А. 101-0864/OU=101/O=PFR/C=RU@101;CN=Ратимова Н.Г. 101-1001/OU=101/O=PFR/C=RU@101;CN=Никитин Р.П. 101-0853/OU=101/O=PFR/C=RU@101;CN=Башкатов А.С 100-1511/OU=100/O=PFR/C=RU@100;CN=Сорокина Е.В. 100-1578/OU=100/O=PFR/C=RU@100;CN=Хлебников О.В. 019000-0803003/OU=019/O=PFR/C=RU@019",
                    DATE = DateTime.Parse("2016-10-13 17:29:33.0"),
                    SUBJECT = "УВЕДОМЛЕНИЕ по процессам СЗВ-М и регистрации СНИЛС",
                    BODY = "<font face=\"Default Sans Serif,Verdana,Arial,Helvetica,sans-serif\" size=\"2\"><div><font size=\"2\" face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\"><font>Уведомляем Вас, что 13.10.2016 с 18:00 будет остановлена среда производственной эксплуатации (подсистемы ВИО, СПУ, НСИ) для установки обновления. Ориентировочное время недоступности - 2 часа.</font><br><font>О восстановлении работоспособности будет сообщено дополнительно.</font><br><br></font></div><div><font size=\"2\" face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\"><br></font></div><div><font size=\"2\" face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\">С уважением,<br>Служба технической поддержки АИС ПФР<br>+7(499) 972-93-37</font></div><div><font size=\"2\" face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\"><br></font></div><div><font size=\"2\" face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\"><br></font></div><div><font size=\"2\" face=\"Sans Serif, Verdana, Arial, Helvetica, sans-serif\"><br></font></div><div><br></div><div></div></font>",
                    ISSENT=false
                },

                  new MAIL {

                    MESSAGEID ="<OF3233F9E4.E6BD0F6D-ON43258064.003FC038-43258064.00403FE9@LocalDomain>",
                    FROM = "CN=Астрейко В.П. 101-1164/OU=101/O=PFR/C=RU",
                    SENDTO = "all0801@101;all1001@101",
                    DATE = DateTime.Parse("2016-11-07 14:41:46.0"),
                    SUBJECT = "Технологические работы на сервере ПТК СПУ",
                    BODY = "Технологические работы на сервере ПТК СПУ завершены.  С 14:30 LSTNRCENT на С1100000 доступен.С уважением, Служба технической поддержки АИС ПФР тел.: 8 (499) 972-93-37.",
                    ISSENT =false,
                    
                } };
        }

        public IQueryable<REGULATION> GetRegulations()
        {
            return repository.GetRegulations();
        }


        public REGULATION GetDefaultRegulation()
        {
            return repository.GetDefaultRegulation();
        }

        public IQueryable<MAIL> GetInbox()
        {
            return  mails.AsQueryable();
        }

        public IQueryable<MAIL> GetInbox(DateTime datebegin, DateTime dateend)
        {
            datebegin = new DateTime(datebegin.Year, datebegin.Month, datebegin.Day, 0, 0, 0);
            dateend = new DateTime(dateend.Year, dateend.Month, dateend.Day, 23, 59, 59);

            return mails.Where(m=>m.DATE>=datebegin && m.DATE<=dateend).AsQueryable();
        }


        public MAIL GetMailByID(string messageID)
        {
            return mails.FirstOrDefault(m=>m.MESSAGEID == messageID);
        }

        public void Delete(string messageID)
        {
            var mail= mails.FirstOrDefault(m => m.MESSAGEID == messageID);
            if (mail != null)
            {
                mails.Remove(mail);
            }
        }

        private string ConvertHTMLText(string text)
        {
            HtmlToText http = new HtmlToText();
            var plainText = http.ConvertHtml(text);

            return plainText;
        }

        public void MarkSent(MAIL _mail)
        {
            MAIL mail = mails.FirstOrDefault(m => m.MESSAGEID == _mail.MESSAGEID);

            if (mail != null)
            {
                mail.ISSENT = _mail.ISSENT;
            }
        }

    

        public void SendOut(MAIL mail)
        {
            StringBuilder messageText = new StringBuilder("\r\n");
            messageText.AppendFormat(">> Дата письма: {0: dd.MM.yyyy HH:mm}", mail.DATE.ToString());
            messageText.Append("\r\n");
            messageText.AppendFormat(">> Тема письма: {0}", mail.SUBJECT != null ? mail.SUBJECT : "");
            messageText.Append("\r\n");
            messageText.AppendFormat(">> Категория : {0}", String.Join(",", mail.REGULATIONS.Select(r => r.NAME)));
            messageText.AppendFormat("\r\n\r\n");
            messageText.AppendFormat("{0}", ConvertHTMLText(mail.BODY));
            messageText.Append("\r\n");


            //string messageText = "\r\nДата письма: " + mail.DATE.ToString("dd.MM.yyyy HH:mm")+ "\r\nТема письма: " + mail.SUBJECT != null ? mail.SUBJECT : "" +  "\r\nОпубликовано: Автоматическая сортировка (" + comment.DATE + ")" + "\r\n\r\n" + htmlAsText;

            sender.SendMessage(mail.JOURNAL.Select(s => s.TARGETID), messageText.ToString());

            MarkSent(mail);

        }
    }
}
