using Newtonsoft.Json;

namespace RedditApp.DataModel
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("token_type")]
        public string Type { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime ExpiresAt
        {
            get
            {
                return DateTime.UtcNow.AddSeconds(ExpiresIn);
            }
        }
    }
}
