using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Business.Library
{

    public static class utils
    {
        public static string GUID()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
        public static string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        public static T SendDataApi<T>(string url, string data, string authorization = "")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(authorization))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                }

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                var contentResult = result.Content.ReadAsStringAsync().Result;
                T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentResult);
                return model;
            }
        }
        public static T SendDataApi<T>(string url, string data, out string contentResult)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                contentResult = result.Content.ReadAsStringAsync().Result;
                T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentResult);
                return model;
            }
        }
        public static void OneWaySendDataApi(string url, string data)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(data, Encoding.GetEncoding("TIS-620"), "application/json");
                client.PostAsync(url, content);
            }
        }
        public static string SerializeObject(dynamic model)
        {
            return JsonConvert.SerializeObject(model);
        }
        public static string SendDataApiAuthorization(string url, string data, string authorization)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                var contentResult = result.Content.ReadAsStringAsync().Result;
                return contentResult;
            }
        }
        public static T SendDataGoogleApi<T>(string url, string data, out string contentResult)
        {
            using (var client = new HttpClient())
            {
                var keystring = "AIzaSyDZmuCtFcZSQcg2FVMpP5K4vP2AnHkx4Hc";
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + keystring);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                contentResult = result.Content.ReadAsStringAsync().Result;
                T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentResult);
                return model;
            }
        }

        internal static List<T> SendDataApi<T>(object p, string v)
        {
            throw new NotImplementedException();
        }

       
    }

    public class AppSettingConfig
    {
        public string GetUrl(string key)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);
            var configuration = builder.Build();
            return configuration.GetConnectionString(key).ToString();
        }
    }
}
