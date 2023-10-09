using Newtonsoft.Json;
using RedditApp.Auth;
using RedditApp.DataModel;
using System.IO.Compression;
using System.Text;

namespace RedditApp.RedditClient
{
    public class RedditClient : IRedditClient
    {
        public string UserName = Secrets.UserName;
        public string Password = Secrets.Password;

        public async Task<AccessToken> GetAccessToken()
        {
            string uriPath = Constants.HttpUriPaht;
            var client = new HttpClient();
            client.BaseAddress = new Uri(uriPath);

            var tokenReqParams = new List<KeyValuePair<string, string>>();
            tokenReqParams.Add(new KeyValuePair<string, string>(Constants.GrantType, Constants.Password));
            tokenReqParams.Add(new KeyValuePair<string, string>(Constants.UserName, this.UserName));
            tokenReqParams.Add(new KeyValuePair<string, string>(Constants.Password, this.Password));
            var req = new HttpRequestMessage(HttpMethod.Post, Constants.TokenUriPath) { Content = new FormUrlEncodedContent(tokenReqParams) };

            req.Headers.Add(Constants.Accept, Constants.Star);
            req.Headers.Add(Constants.Accept_Encoding, Constants.gzip);
            req.Headers.Add(Constants.Authorization, Secrets.TokenAuth);
            req.Headers.Add(Constants.Connection, Constants.KeepAlive);
            req.Headers.Add(Constants.UserAgent, Constants.ChangeMeClient);

            try
            {
                using HttpResponseMessage response = await client.SendAsync(req);
                Stream responded;
                if (response.IsSuccessStatusCode)
                {
                    response.Content.ReadAsStringAsync().Wait();
                    responded = response.Content.ReadAsStreamAsync().Result;
                    Stream decompressed = new GZipStream(responded, CompressionMode.Decompress);
                    StreamReader objReader = new StreamReader(decompressed, Encoding.UTF8);
                    string sLine;
                    sLine = objReader.ReadToEnd();
                    var token = JsonConvert.DeserializeObject<AccessToken>(sLine);
                    return token;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception happened with http request to reddit: " + ex.ToString());
            }

            return null;
        }

        public async Task<Listing> GetPostListing(string token)
        {
            string uriPath = Constants.OathUriPath;
            var client = new HttpClient();
            client.BaseAddress = new Uri(uriPath);

            var req = new HttpRequestMessage(HttpMethod.Get, Constants.NewPostPath);
            req.Headers.Add(Constants.Accept, Constants.applicationjson);
            req.Headers.Add(Constants.Accept_Encoding, Constants.gzip);
            req.Headers.Add(Constants.Authorization, Constants.Bearer + token);
            req.Headers.Add(Constants.Connection, Constants.KeepAlive);
            req.Headers.Add(Constants.UserAgent, Constants.ChangeMeClient);

            try
            {
                using HttpResponseMessage response = await client.SendAsync(req);

                Stream responded;
                if (response.IsSuccessStatusCode)
                {
                    response.Content.ReadAsStringAsync().Wait();
                    responded = response.Content.ReadAsStreamAsync().Result;
                    Stream decompressed = new GZipStream(responded, CompressionMode.Decompress);
                    StreamReader objReader = new StreamReader(decompressed, Encoding.UTF8);
                    string sLine;
                    sLine = objReader.ReadToEnd();
                    var listing = JsonConvert.DeserializeObject<Listing>(sLine);
                    var rateLimitRemaining = (int)double.Parse(response.Headers.GetValues(Constants.RateLimitRemaining).FirstOrDefault() ?? "0");
                    listing.RateLimitRemaining = rateLimitRemaining;
                    return listing;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception happened with http request to reddit: " + ex.ToString());
            }

            return null;
        }
    }
}
