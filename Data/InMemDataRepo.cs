using RedditApp.DataModel;
using System.Data;
using System.Linq;

namespace RedditApp.Data
{
    public class InMemDataRepo : IDataRepo
    {
        public static DataSet inMemDataSet = new DataSet(Constants.InMemDataSet);
        private static object _lock = new object();
        DataTable postTable;
        DataTable authorTable;

        public InMemDataRepo()
        {
            InitDataRepo();
        }

        public void InitDataRepo()
        {
            InitPostTable();
            InitAuthorTable();
        }

        public void UpdatePostData(List<Post> posts)
        {
            if (posts == null || posts.Count == 0)
                return;

            lock (_lock)
            {
                foreach (var post in posts)
                {
                    UpsertRow(postTable, post.PostId, post.ToObjects());
                }
            }
        }

        public void UpdateAuthorData(List<Post> posts)
        {
        }

        private void InitPostTable()
        {
            if (inMemDataSet.Tables.Contains(Constants.PostTable) )
                return;

            postTable = inMemDataSet.Tables.Add(Constants.PostTable);
            DataColumn idCol = postTable.Columns.Add(Constants.PostId, typeof(string));
            idCol.AllowDBNull = false;
            idCol.Unique = true;
            postTable.PrimaryKey = new[] { idCol };

            postTable.Columns.Add(Constants.Subreddit, typeof(string));
            postTable.Columns.Add(Constants.Title, typeof(string));
            postTable.Columns.Add(Constants.AuthorId, typeof(string));
            postTable.Columns.Add(Constants.Upvote_ratio, typeof(double));
            postTable.Columns.Add(Constants.Ups, typeof(int));
            postTable.Columns.Add(Constants.Downs, typeof(int));
            postTable.Columns.Add(Constants.NumOfComments, typeof(int));
            postTable.Columns.Add(Constants.CreatedUtcTime, typeof(DateTime));
        }

        public List<Post> RetrievePostData()
        {
            if (!inMemDataSet.Tables.Contains(Constants.PostTable))
                return new List<Post>();

            var postTable = inMemDataSet.Tables[Constants.PostTable];

            var list = new List<Post>();
            foreach (var row in postTable.Rows )
            {
                list.Add(Post.BuildFromData(((DataRow)row).ItemArray));
            }

            return list;
        }

        private void InitAuthorTable()
        {
            if (inMemDataSet.Tables.Contains(Constants.AuthorTable))
                return;

            authorTable = inMemDataSet.Tables.Add(Constants.AuthorTable);
            DataColumn idCol = authorTable.Columns.Add(Constants.AuthorId, typeof(string));
            idCol.AllowDBNull = false;
            idCol.Unique = true;
            authorTable.PrimaryKey = new[] { idCol };
            authorTable.Columns.Add(Constants.Name, typeof(string));
        }

        private static void UpsertRow(DataTable table, string key, object[] data)
        {
            DataRow dr = table.Rows.Find(key);
            if (dr != null)
            {
                dr.AcceptChanges();
                dr.BeginEdit();
                dr.ItemArray = data;
                dr.EndEdit();
            }
            else
            {
                table.Rows.Add(data);
            }
        }
    }
}
