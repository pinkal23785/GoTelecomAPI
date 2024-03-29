using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data
{
    public class SMSRequestModel
    {
        private readonly IConfiguration _configuration;
        public SMSRequestModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string AppSid { get { return _configuration.GetValue<string>("AppSid"); } }
        public string SenderID { get { return _configuration.GetValue<string>("SenderID"); } }
        public string Recipient { get; set; }
        public string Body { get; set; }
    }
}
