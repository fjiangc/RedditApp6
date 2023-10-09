using RedditApp.Data;
using RedditApp.DataModel;

namespace RedditApp.RedditAccess
{
    public interface IRedditAccessAgent
    {
        Task<Listing> RetrievePostData();

        Task PersistPostData(IDataRepo repo);

        Task Run(IDataRepo repo);
    }
}
