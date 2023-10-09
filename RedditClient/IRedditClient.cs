using RedditApp.DataModel;

namespace RedditApp.RedditClient
{
    public interface IRedditClient
    {
        Task<AccessToken> GetAccessToken();

        Task<Listing> GetPostListing(string token);
    }
}
