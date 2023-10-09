using RedditApp.Auth;
using RedditApp.Data;
using RedditApp.DataModel;
using RedditApp.RedditAccess;
using RedditApp.RedditClient;
using RedditApp.StatQuery;

// Load credentials
Secrets.LoadCredentials();

// User Login
var token = await GetToken();
if (token == null)
{
    Console.WriteLine("Invalid User Name or Password!");
    Thread.Sleep(3000);
    return;
}

// Init Data Repo and Reddit Agents.
var repo = new InMemDataRepo();
var agents = new List<IRedditAccessAgent>();

for (int i = 0; i < 5; i++)
{
    IRedditAccessAgent agent = new RedditAgent();
    agent.SetAgentId(i);
    agents.Add(agent);
}

// Run several agents in parallel
Parallel.ForEach(agents, async agent =>
{
    await agent.Run(repo);
});

// Input query command.
while (true)
{
    var stop = ProcessCommand();
    if (stop)
        return;
}

// End of Program Logic


// Helper methods.
async Task<AccessToken> GetToken()
{
    Console.WriteLine("Input User Name:");
    var userName = Console.ReadLine();

    Console.WriteLine("Input Password:");
    var password = Console.ReadLine();

    IRedditClient loginClient = new RedditClient()
    {
        UserName = userName,
        Password = password,
    };

    var token = await loginClient.GetAccessToken();
    return token;
}

bool ProcessCommand()
{
    Console.WriteLine("Please input command (Stop/TopPost/TopAuthor)");
    var input = Console.ReadLine();

    if (input == "Stop")
    {
        foreach (var agent in agents)
        {
            agent.ShutdownAgent();
        }

        Thread.Sleep(1000);
        return true;
    }
    else if (input == "TopPost")
    {
        var list = repo.RetrievePostData();
        IQuery query = new InMemoryDataQuery(list);
        var result = query.QueryData("TopPost");
        Console.WriteLine($"The post that has most upvotes: {result}\n");
    }
    else if (input == "TopAuthor")
    {
        var list = repo.RetrievePostData();
        IQuery query = new InMemoryDataQuery(list);
        var result = query.QueryData("TopAuthor");
        Console.WriteLine($"The author that submitted most posts: {result}\n");
    }

    return false;
}