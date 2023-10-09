using Newtonsoft.Json;

namespace RedditApp.DataModel
{
    public class Listing
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public PostData Data { get; set; }

        public int RateLimitRemaining { get; set; }

    }
}
