using Nethereum.KeyStore;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WindingTreeNet.Model
{
    public class Wallet
    {
        public string address;
        public string id;
        public int version = 3;
        public Crypto crypto;

        // Private key
        [JsonIgnore]
        public EthECKey key;

        // Wallet password
        [JsonIgnore]
        public string password;

        [JsonIgnore]
        public string GetPrivateKey {
            get { return key.GetPrivateKey(); }
        }

        public Wallet(EthECKey ethKey, string pass = "")
        {
            key = ethKey;
            password = pass;

            InitWallet();
        }

        public Wallet(string privateKey, string pass = "")
        {
            key = new EthECKey(privateKey);
            password = pass;

            InitWallet();
        }

        private void InitWallet()
        {
            address = key.GetPublicAddress().Substring(2);
            id = Guid.NewGuid().ToString();

            var keyStoreService = new KeyStoreService();
            string cryptoString = keyStoreService.EncryptAndGenerateDefaultKeyStoreAsJson(this.password, this.key.GetPrivateKeyAsBytes(), key.GetPublicAddress());

            try
            {
                crypto = JsonConvert.DeserializeObject<RootObject>(cryptoString).crypto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
