using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using MailSender.Services.Interfaces;

namespace MailSender.Services
{
    public class APISender : ISenderService
    {
        //Path to web api
        private string url;

        public APISender(string _url)
        {
            url = _url;
        }

        public void SendMessage(IEnumerable<long> targets, string message)
        {
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                UriBuilder builder = new UriBuilder(url);

                var parameters = new
                {
                    content = message,
                    id = 0,
                    sendto = targets,
                    title = "MailSender",
                    author = new  { id = 3512, fullname = "" },
                    priority = new  { id = 1 }


                };

                var json_parameters = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json_parameters, Encoding.UTF8, "application/json");
               
                    var response = client.PostAsync(builder.Uri, content).Result;
                    var responseString = response.Content.ReadAsStringAsync().Result;
                try
                {
                    dynamic obj = JsonConvert.DeserializeObject(responseString);
                    if (obj.message == "errors")
                    {
                        throw new Exception(obj.errors);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(responseString);
                }

            }
        }
    }
}
