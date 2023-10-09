using RedditApp.DataModel;

namespace RedditApp.Data
{
    public interface IDataRepo
    {
        void InitDataRepo();

        void UpdatePostData(List<Post> posts);

        List<Post> RetrievePostData();
    }
}
