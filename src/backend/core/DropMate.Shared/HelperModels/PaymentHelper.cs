using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.HelperModels
{
    public class PaymentHelper
    {
        public HttpClient ApiHelper { get; set; }
        public PaymentHelper(string token="")
        {
            ApiHelper = new()
            {
                BaseAddress = new Uri("https://dropmate2.onrender.com/api/")
            };
            ApiHelper.DefaultRequestHeaders.Accept.Clear();
            if(!string.IsNullOrEmpty(token))
                 ApiHelper.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ApiHelper.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
