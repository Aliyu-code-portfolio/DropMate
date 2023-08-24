using System.Net.Http.Headers;

namespace DropMate2.Shared.HelperModels
{
    public class PayStackHelper
    {
        public HttpClient ApiClient { get; set; }
        public PayStackHelper()
        {
            string paystackSecret = Environment.GetEnvironmentVariable("PaymentSecret");
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.paystack.co/")
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecret);
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
