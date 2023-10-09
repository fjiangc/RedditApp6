using RedditApp.Data;
using RedditApp.DataModel;

namespace RedditApp.RedditAccess
{
    public class RedditAgent: IRedditAccessAgent
    {
        private RedditClient.RedditClient client = new RedditClient.RedditClient();
        private AccessToken token;

        public async Task Run(IDataRepo repo)
        {
            while (true)
            {
                if (StopFlag) return;

                if (token == null || token.ExpiresAt.Subtract(DateTime.UtcNow).TotalSeconds < 60)
                {
                    token = await client.GetAccessToken();
                }

                await this.PersistPostData(repo);
            }
        }

        public async Task InitiateAgent(int agentNumber)
        {
            AgentNumber = agentNumber;

            if (token == null || token.ExpiresAt.Subtract(DateTime.UtcNow).TotalSeconds < 60)
            {
                token = await client.GetAccessToken();
            }
        }

        public async Task<Listing> RetrievePostData()
        {
            return await client.GetPostListing(token.Token);
        }

        public async Task PersistPostData(IDataRepo repo)
        {
            Log($"Agent {AgentNumber} retrieving post data.");
            var listing = await this.RetrievePostData();

            if (listing == null)
            {
                Log($"Agent {AgentNumber} waiting 10 seconds due to rate limiting being applied.");

                // Normally caused by rate limiting, in this case we just return.
                await Task.Delay(10000);
                return;
            }

            var rateLimitRemaining = listing.RateLimitRemaining;

            // A very simple rate limiting strategy.
            // Based on rate limit remaining number, we delay for different periods.
            if (rateLimitRemaining > 400)
            {
                await Task.Delay(1000);
            }
            else if (rateLimitRemaining > 200)
            {
                await Task.Delay(2000);
            }
            else
            {
                Log($"Agent {AgentNumber} waiting 60 seconds to avoid rate limiting.");
                await Task.Delay(60000);
            }

            var posts = listing?.Data?.PostWraps?.Select(s => s.SinglePost).ToList();
            repo.UpdatePostData(posts);
        }

        public void Log(string message)
        {
            if (ShowLog)
                Console.WriteLine($"{message}");
        }

        public void SetAgentId(int i)
        {
            AgentNumber = i;
        }

        public void ShutdownAgent()
        {
            StopFlag = true;
        }

        private int AgentNumber { get; set; }

        private bool StopFlag { get; set; }

        // only for debugging purpose.
        public bool ShowLog { get; set; }

    }
}
