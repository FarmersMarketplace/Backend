using FarmersMarketplace.Application.DataTransferObjects.Account;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace FarmersMarketplace.Application.Services.Business
{
    public class LiqpayService
    {
        protected readonly IConfiguration Configuration;

        public LiqpayService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task Pay(CustomerPaymentDataDto paymentData, decimal totalPayment, Guid orderId, string receiverCard)
        {
            var requestData = new
            {
                action = "p2p",
                version = 3,
                public_key = Configuration["Liqpay:PublicKey"],
                amount = totalPayment,
                currency = "UAH",
                description = $"order {orderId}",
                order_id = orderId.ToString(),
                card_exp_month = paymentData.CardExpirationMonth,
                card_exp_year = paymentData.CardExpirationYear,
                private_key = Configuration["Liqpay:PrivateKey"],
                card = paymentData.CardNumber,
                receiver_card = receiverCard
            };

            string jsonString = JsonConvert.SerializeObject(requestData);
            string base64EncodedData = await Base64Encode(jsonString);
            string signature = await GenerateSignature(Configuration["Liqpay:PrivateKey"], base64EncodedData);

            using (var client = new HttpClient())
            {
                string apiUrl = "https://www.liqpay.ua/api/request";

                var content = new StringContent($"data={Uri.EscapeDataString(base64EncodedData)}&signature={Uri.EscapeDataString(signature)}", Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await client.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error: " + response.StatusCode);
                }
            }
        }

        private async Task<string> GenerateSignature(string private_key, string data)
        {
            string concatenatedString = private_key + data + private_key;

            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private async Task<string> Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
