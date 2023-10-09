using RedditApp.DataModel;

namespace RedditApp.StatQuery
{
    public class InMemoryDataQuery: IQuery
    {
        public const string TopPost = "TopPost";
        public const string TopUser = "TopAuthor";
        public const string DataNotReady = "Post data isn't ready yet, please wait a few seconds for data to be populated.";
        public List<Post> Posts;

        public InMemoryDataQuery(List<Post> posts)
        {
            this.Posts = posts;
        }

        public string QueryData(string command)
        {
            if (command == TopPost)
            {
                return QueryTopPost();
            }
            else if (command == TopUser)
            {
                return QueryTopAuthor();
            }
            else
                return "Wrong command!";                
        }

        private string QueryTopPost()
        {
            if (this.Posts == null || this.Posts.Count == 0)
            {
                return DataNotReady;
            }

            var ups = this.Posts.Max(post => post.Ups);
            var post = this.Posts.FirstOrDefault(p => p.Ups == ups);

            if (post == null) { return "no found!"; }
            return post?.PostId + "(Id): " + post?.Title + $"(title) with {post?.Ups} upvotes";
        }

        private string QueryTopAuthor()
        {
            if (this.Posts == null || this.Posts.Count == 0)
            {
                return DataNotReady;
            }

            var dict = this.Posts.GroupBy(p => p.AuthorId).ToDictionary(g => g.Key, g => g.Count());
            var maxCount = dict.Values.Max();
            var author = dict.Keys.Where(k => dict[k] == maxCount).FirstOrDefault();
            if (author == null) { return "no found!"; }
            return author;
        }
    }
}
