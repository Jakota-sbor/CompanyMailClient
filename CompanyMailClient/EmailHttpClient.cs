using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CompanyMailClient
{
    static class EmailHttpClient
    {
        static HttpClient client;

        public static async Task<HttpStatusCode> SendEmail(Email email)
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55500/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            HttpResponseMessage response = await client.PutAsJsonAsync("Emails/Add", email);
            return response.StatusCode;
        }

        public static async Task<IEnumerable<EmailModel>> GetEmails(string emailAddress)
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55500/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            HttpResponseMessage response = await client.GetAsync("Emails/GetEmails?emailAddress=" + emailAddress);
            var emails = (await response.Content.ReadAsAsync<IEnumerable<Email>>()).Select(e=> new EmailModel(e));
            return emails;
        }
    }
}
