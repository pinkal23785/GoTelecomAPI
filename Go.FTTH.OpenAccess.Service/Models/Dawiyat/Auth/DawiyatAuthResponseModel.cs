﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat
{
    public class DawiyatAuthResponseModel
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }

        [JsonPropertyName("not-before-policy")]
        public string not_before_policy { get; set; }
        public string session_state { get; set; }
        public string scope { get; set; }
    }
}
