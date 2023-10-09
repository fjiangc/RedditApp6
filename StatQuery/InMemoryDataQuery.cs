using RedditApp.DataModel;

namespace RedditApp.StatQuery
{
    public class InMemoryDataQuery: IQuery
    {
        public const string TopPost = "TopPost";
        public const string TopUser = "TopAuthor";
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
            var ups = this.Posts.Max(post => post.Ups);
            var post = this.Posts.FirstOrDefault(p => p.Ups == ups);

            if (post == null) { return "no found!"; }
            return post?.PostId + "(Id): " + post?.Title + "(title)";
        }

        private string QueryTopAuthor()
        {
            var dict = this.Posts.GroupBy(p => p.AuthorId).ToDictionary(g => g.Key, g => g.Count());
            var maxCount = dict.Values.Max();
            var author = dict.Keys.Where(k => dict[k] == maxCount).FirstOrDefault();
            if (author == null) { return "no found!"; }
            return author;
        }
    }
}
