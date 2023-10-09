using RedditApp.Auth;
using RedditApp.Data;
using RedditApp.RedditAccess;
using RedditApp.RedditClient;
using RedditApp.StatQuery;


Secrets.LoadCredentials();

// User Login
Console.WriteLine("Input User Name:");
var userName = Console.ReadLine();

Console.WriteLine("Input Password:");
var password = Console.ReadLine();

var loginClient = new RedditClient()
{
    UserName = userName,
    Password = password,
};

var token = await loginClient.GetAccessToken();
if (token == null)
{
    Console.WriteLine("Invalid User Name or Password!");
    Thread.Sleep(3000);
    return;
}

// Init Data Repo and Reddit Agents.
var repo = new InMemDataRepo();
var agents = new List<RedditAgent>();

// Run several agents in parallel
for (int i = 0; i < 1; i++)
{
    var agent = new RedditAgent();
    agent.AgentNumber = i;
    agents.Add(agent);
}

Parallel.ForEach(agents, async agent =>
{
    await agent.Run(repo);
});

// Input query command.
while (true)
{
    Console.WriteLine("Please input command (Stop/TopPost/TopAuthor)");
    var input = Console.ReadLine();

    if (input == "Stop")
    {
        foreach (var agent in agents)
        {
            agent.StopFlag = true;
        }

        Thread.Sleep(1000);
        break;
    }
    else if (input == "TopPost")
    {
        var list = repo.RetrievePostData();
        var query = new InMemoryDataQuery(list);
        var result = query.QueryData("TopPost");
        Console.WriteLine($"The post that has most upvotes: {result}\n");
    }
    else if (input == "TopAuthor")
    {
        var list = repo.RetrievePostData();
        var query = new InMemoryDataQuery(list);
        var result = query.QueryData("TopAuthor");
        Console.WriteLine($"The author that submitted most posts: {result}\n");
    }
}