using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.HelperModels
{
    public class GoogleMapHelper
    {
        public HttpClient ApiHelper { get; set; }
        public GoogleMapHelper()
        {
            ApiHelper = new()
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/json?")
            };
            ApiHelper.DefaultRequestHeaders.Accept.Clear();
            ApiHelper.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
