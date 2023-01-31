using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace InterfaceWMSAPI
{
    public class CommonUtility
    {
        public static T SendDataGetApi<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.GetAsync(url).Result;
                var contentResult = result.Content.ReadAsStringAsync().Result;
                T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentResult);
                return model;
            }
        }

        public static T SendDataPostApi<T>(string url, string data)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                var contentResult = result.Content.ReadAsStringAsync().Result;
                T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentResult);
                return model;
            }
        }
    }
}
