using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Web;

using Nethereum.Web3.Accounts;
using Nethereum.Signer;
using Newtonsoft.Json;

using WindingTreeNet.Model;
using WindingTreeNet.Utils;
using System.Timers;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.Hex.HexTypes;

namespace WindingTreeNet
{
    public class WindingTree
    {
        // Public Ethereum API
        private const string INFURA_API = "https://mainnet.infura.io/";

        // Public WTree write API
        private const string WRITE_API = "https://playground-write-api.windingtree.com";

        // Public WTree read API
        private const string READ_API = "https://playground-api.windingtree.com";

        // Public WTree API: create account
        private const string CREATE_ACCOUNT = WRITE_API + "/accounts";

        // Public WTree API: create hotel
        private const string WRITE_HOTEL = WRITE_API + "/hotels";

        // Public WTree API: create hotel
        private const string GET_HOTEL = READ_API + "/hotels";

        // Account & Wallet
        private AccountResponse account = null;
        public Wallet wallet = null;

        // Balance checker data
        //private string ethAddressToCheck = null;
        //private bool balanceCheckerLocked = false;
        //private Func<string> paymentReceivedCallback;

        //private Timer balanceCheckerTimer = null;

        // Main constructor
        // By default connects to the Ropsten network
        // You can modify the connection options now by directly editing the const values
        public WindingTree()
        {}

        public async void GetHotels()
        {
            await ReadHotels("");
        }

        public async Task TransferEther(string senderWalletPKey, string destinationAddress, decimal etherAmount)
        {
            Account account = new Account(senderWalletPKey);
   
            Web3 web3 = new Web3(account, INFURA_API);
            HexBigInteger weiAmount = new HexBigInteger(Web3.Convert.ToWei(etherAmount));
            
            try
            {
                var receiptFirstAmountSend = await web3.TransactionManager.SendTransactionAsync(account.Address, destinationAddress, weiAmount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        public async Task<decimal> GetEtherBalanceOf(string ethWalletAddress)
        {
            Web3 web3 = new Web3(INFURA_API);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(ethWalletAddress);

            decimal etherAmount = 0;
            if (balance != null)
                etherAmount = Web3.Convert.FromWei(balance.Value);

            return etherAmount;
        }

        public async void UpdateHotel(WTHotel hotel, string hotelID, string accountKey, string walletPass)
        {
            await UpdateHotel( accountKey, walletPass, hotelID, JsonConvert.SerializeObject(hotel));
        }

        public async void UpdateHotel(WTHotel hotel, string hotelID, AccountResponse _account = null, Wallet _wallet = null)
        {
            if (_account == null)
                _account = account;

            if (_wallet == null)
                _wallet = wallet;

            UpdateHotel(hotel, hotelID, _account.accessKey, _wallet.password);
        }

        public async void RemoveHotel(string hotelID, string accountKey, string walletPass)
        {
            await DeleteHotel(accountKey, walletPass, hotelID);
        }

        public async void RemoveHotel(string hotelID, AccountResponse _account = null, Wallet _wallet = null)
        {
            if (_account == null)
                _account = account;

            if (_wallet == null)
                _wallet = wallet;

            RemoveHotel(hotelID, _account.accessKey, _wallet.password);
        }

        public async Task<HotelResponse> CreateHotel(WTHotel hotel, string accountKey, string walletPass)
        {
            return await WriteNewHotel(accountKey, walletPass, JsonConvert.SerializeObject(hotel));
        }

        public async Task<HotelResponse> CreateHotel(WTHotel hotel, AccountResponse _account = null, Wallet _wallet = null)
        {
            if (_account == null)
                _account = account;

            if (_wallet == null)
                _wallet = wallet;

            return await CreateHotel(hotel, _account.accessKey, _wallet.password);
        }

        public async Task<AccountResponse> CreateAccountFromExistingWallet(string privateKey, string walletPass)
        {
            wallet = new Wallet(privateKey, walletPass);

            // Create JSON object for write-API            
            AccountObject accountObject = new AccountObject(wallet);

            // Serialize
            string accountObjectJSON = JsonConvert.SerializeObject(accountObject);

            return await CreateAccountFromJSONWallet(accountObjectJSON);
        }

        // Create account
        public async Task<AccountResponse> CreateAccountFromJSONWallet(string jsonWallet)
        {
            account = await WriteNewAccount(jsonWallet);

            return account;
        }

        // Create new account
        public async Task<AccountResponse> CreateAccountAndWallet(string walletPass)
        {
            wallet = new Wallet(EthECKey.GenerateKey(), walletPass);

            // Create account on the Ethereum network
            var account = new Account(wallet.GetPrivateKey);

            // Create JSON object for write-API            
            AccountObject accountObject = new AccountObject(wallet);

            // Serialize
            string accountObjectJSON = JsonConvert.SerializeObject(accountObject, Formatting.Indented);

            return await CreateAccountFromJSONWallet(accountObjectJSON);
        }

        // Writes new account to the winding tree write API
        private async Task<AccountResponse> WriteNewAccount(string accountJSON)
        {
            StringContent httpContent = new StringContent(accountJSON, Encoding.UTF8, "application/json");
            AccountResponse response = null;

            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync(CREATE_ACCOUNT, httpContent).ConfigureAwait(false);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    response = JsonConvert.DeserializeObject<AccountResponse>(responseContent);
                }
            }

            return response;
        }

        // Writes new account to the winding tree write API
        private async Task<HotelResponse> WriteNewHotel(string accountAccessKey, string walletPassword, string hotelJSON)
        {
            StringContent httpContent = new StringContent(hotelJSON, Encoding.UTF8, "application/json");
            HotelResponse response = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-Access-Key", accountAccessKey);
                httpClient.DefaultRequestHeaders.Add("X-Wallet-Password", walletPassword);

                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync(WRITE_HOTEL, httpContent).ConfigureAwait(false);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    response = JsonConvert.DeserializeObject<HotelResponse>(responseContent);
                }
            }

            return response;
        }

        // Returns list of hotels
        private async Task<HotelResponse> ReadHotels(string accountAccessKey)
        {
            // StringContent httpContent = new StringContent(hotelJSON, Encoding.UTF8, "application/json");
            HotelResponse response = null;

            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.GetAsync(GET_HOTEL + "?limit=100");

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<HotelResponse>(responseContent);
                }
            }

            return response;
        }

        // Delete hotel with ID
        private async Task<HotelResponse> DeleteHotel(string accountAccessKey, string walletPassword, string hotelID)
        {
            // StringContent httpContent = new StringContent(hotelJSON, Encoding.UTF8, "application/json");
            HotelResponse response = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-Access-Key", accountAccessKey);
                httpClient.DefaultRequestHeaders.Add("X-Wallet-Password", walletPassword);

                // Do the actual request and await the response
                var httpResponse = await httpClient.DeleteAsync(WRITE_HOTEL + "/" + hotelID);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<HotelResponse>(responseContent);
                }
            }

            return response;
        }

        // Update hotel
        private async Task<HotelResponse> UpdateHotel(string accountAccessKey, string walletPassword, string hotelID, string hotelJSON)
        {
            StringContent httpContent = new StringContent(hotelJSON, Encoding.UTF8, "application/json");
            HotelResponse response = null;

            using (var httpClient = new HttpClient ())
            {
                httpClient.DefaultRequestHeaders.Add("X-Access-Key", accountAccessKey);
                httpClient.DefaultRequestHeaders.Add("X-Wallet-Password", walletPassword);

                // Do the actual request and await the response
                var httpResponse = await httpClient.PatchAsync(WRITE_HOTEL + "/" + hotelID, httpContent);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<HotelResponse>(responseContent);
                }
            }

            return response;
        }
    }
}


