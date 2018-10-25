using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindingTreeNet.Model
{
    public class AccountResponse
    {
        public string accountId { get; set; }
        public string accessKey { get; set; }       
    }

    public class HotelResponse
    {
        public string address { get; set; }
    }
}
