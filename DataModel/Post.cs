using Newtonsoft.Json;

namespace RedditApp.DataModel
{
    public class Post
    {
        public static Post BuildFromData(Object[] objects)
        {
            return new Post()
            {
                PostId = (string)objects[0],
                Subreddit = (string)objects[1],
                Title = (string)objects[2],
                AuthorId = (string)objects[3],
                Upvote_ratio = (double)objects[4],
                Ups = (int)objects[5],
                Downs = (int)objects[6],
                NumOfComments = (int)objects[7],
                CreatedUtc = (long)((DateTime)objects[8] - new DateTime(1970, 1, 1)).TotalSeconds
            };
        }

        [JsonProperty("id")]
        public string PostId { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("author_fullname")]
        public string AuthorId { get; set; }

        [JsonProperty("upvote_ratio")]
        public double Upvote_ratio { get; set; }

        [JsonProperty("ups")]
        public int Ups { get; set; }

        [JsonProperty("downs")]
        public int Downs { get; set; }

        [JsonProperty("num_comments")]
        public int NumOfComments { get; set; }

        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        public DateTime CreatedUtcTime {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(CreatedUtc).DateTime;
            }
        }

        public Object[] ToObjects()
        {
            return new Object[]
            {
                PostId, Subreddit, Title, AuthorId, Upvote_ratio, Ups, Downs, NumOfComments, CreatedUtcTime
            };
        }
    }

    public class PostWrap
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public Post SinglePost { get; set; }
    }

    public class PostData
    {

        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("children")]
        public PostWrap[] PostWraps { get; set; }
    }
}
