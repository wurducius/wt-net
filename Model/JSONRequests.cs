using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindingTreeNet.Model
{
    public class Cipherparams
    {
        public string iv { get; set; }
    }

    public class Kdfparams
    {
        public int dklen { get; set; }
        public int n { get; set; }
        public int p { get; set; }
        public int r { get; set; }
        public string salt { get; set; }
    }
    
    public class Crypto
    {
        [JsonProperty]
        public string cipher { get; set; }

        [JsonProperty]
        public string ciphertext { get; set; }

        [JsonProperty]
        public Cipherparams cipherparams { get; set; }
        public string kdf { get; set; }
        public Kdfparams kdfparams { get; set; }
        public string mac { get; set; }
    }

    public class RootObject
    {
        public Crypto crypto { get; set; }
    }

    public class Swarm
    {
    }

    public class Root
    {
        public Swarm swarm { get; set; }
    }

    public class Uploaders
    {
        public Root root { get; set; }
    }
}
