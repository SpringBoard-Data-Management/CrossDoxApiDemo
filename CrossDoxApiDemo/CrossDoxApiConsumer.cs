using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security;
using System.Text;

namespace CrossDoxApiDemo
{
    class CrossDoxApiConsumer
    {
        private const string ApiUrl = "https://api.crossdox.com/";
        private static readonly HttpClient client = new HttpClient();

        private readonly string token;
        private readonly string apiToken;


        public CrossDoxApiConsumer(string ApiToken, string userName, string password)
        {
            apiToken = ApiToken;
            string ApiPath = "AuthApi/Login";
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiUrl + ApiPath);

            req.Headers.Add("ApiToken", ApiToken);

            req.Content = ToContent(new Dictionary<string, string>
            {
                {"UserName", userName},
                {"Password", password}
            });


            HttpResponseMessage resp = client.SendAsync(req).Result;
            if (!resp.IsSuccessStatusCode)
            {
                throw new SecurityException(); // Do error checking
            }

            token = JsonConvert.DeserializeObject<string>(resp.Content.ReadAsStringAsync().Result);
        }

        public CrossDoxParsedData ParseFiles(FileInfo[] files)
        {
            string ApiPath = "UploadApi/ParseXLSX";
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiUrl + ApiPath);

            req.Headers.Add("Token", token);

            using var formData = new MultipartFormDataContent();
            foreach (FileInfo file in files)
            {
                formData.Add(new StreamContent(file.OpenRead()), file.Name, file.Name);
            }
            req.Content = formData;

            var resp = client.SendAsync(req).Result;
            if (!resp.IsSuccessStatusCode)
            {
                return null; // Do error checking
            }

            return JsonConvert.DeserializeObject<CrossDoxParsedData>(resp.Content.ReadAsStringAsync().Result);
        }

        public void UpdateUserPreferences(CrossDoxUserPreferences usersPreferencesModel)
        {
            string ApiPath = "UsersApi/Preferences";
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiUrl + ApiPath);

            req.Headers.Add("ApiToken", apiToken);
            req.Headers.Add("Token", token);

            req.Content = ToContent(usersPreferencesModel);

            HttpResponseMessage resp = client.SendAsync(req).Result;
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception("API error " + resp.StatusCode); // Do error checking
            }
        }

        private static StringContent ToContent(object payload)
        {
            return ToStringContent(JsonConvert.SerializeObject(payload));
        }

        private static StringContent ToStringContent(string serialized)
        {
            return new StringContent(serialized, Encoding.Default, "application/json");
        }
    }
}
