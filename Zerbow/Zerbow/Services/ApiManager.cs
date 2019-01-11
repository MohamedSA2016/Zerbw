using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zerbow.Models;
using Xamarin.Forms;

namespace Zerbow.Services
{
    class ApiManager
    {
        private static ApiManager instance;
        private HttpClient client;

        private ApiManager()
        {
            client = new HttpClient();
        }

        public static ApiManager ApiManagerInstance
        {
            get
            {
                if (instance == null)
                    instance = new ApiManager();
                return instance;
            }
        }

       

        public async Task<string> GetPostedImageUri(System.IO.Stream stream)
        {
            HttpClient client2 = new HttpClient();
            client2.DefaultRequestHeaders.Add("Authorization", " Client-ID d35441c92c7c328"); 

            // Request body
            var content = new ByteArrayContent(ToByteArray(stream));
            HttpResponseMessage response = await client2.PostAsync("https://api.imgur.com/3/image", content);

            // Process response
            string res = await response.Content.ReadAsStringAsync();
            Imgur.RootObject rootObject = JsonConvert.DeserializeObject<Imgur.RootObject>(res);
            return rootObject.data.id;
        }

        private byte[] ToByteArray(System.IO.Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }

    }
}