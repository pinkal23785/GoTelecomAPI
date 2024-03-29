using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Go.Web.Invoices.Models
{
    public class ReCaptcha
    {
        private readonly HttpClient captchaClient;

        public ReCaptcha(HttpClient captchaClient)
        {
            this.captchaClient = captchaClient;
        }

        public async Task<bool> IsValid(string captcha,string secret)
        {
            try
            {
                var client = new System.Net.WebClient();
                var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, captcha));
                //var postTask = await captchaClient.GetStringAsync($"?secret=" + secret + "&response={captcha}");
                   // .PostAsync($"?secret="+ secret + "&response={captcha}", new StringContent(""));
                //var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(googleReply);
                dynamic success = resultObject["success"];
                return (bool)success;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
