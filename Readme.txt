Summary

A simple implementation with a in-memory dataset to store the reddit post data.

1. Reddit API: https://oauth.reddit.com/r/funny/new
This API only returns 25 (not 100) posts. I guess somehow the rate limiting triggered some changes so that it only returns 25 posts now. So far the best 3rd party data API is provided by "PushShift", but you have to have a moderator role in Reddit first.

2. In-memory data storage.
This code is in Data folder. We use ADO DataSet here, if we want to, there should be enough ways to persist the data into a file or a real DB. We designed two tables with schema - Post and Author, so far Author table isn't used yet. The DataSet is initialized each time we start the program, also will be destructed when the program ends.

3. Auth.
Username and password should be provided in Secrets.cs file. TokenAuth is directly copied from Postman, only for retrieving auth token. https://github.com/reddit-archive/reddit/wiki/OAuth2-Quick-Start-Example

4. Parallel Agents
RedditAgent is defined in the folder of RedditAccess. It wraps the code that retrieves reddit data and then store in the DataSet. In program.cs file, we directly use Parallel.Invoke to start multiple agents running together.

5. Statistics Query
Query is defined in StatQuery folder. With this amount of data, we simply use linq code to get the top post and top author. In real world with bigger data, this could be more complicated, may need to use MapReduce sort of technology.

6. User Login.
This is very easy implementation. Just use the "GetAccessToken" method to verify the username and password.

7. program.cs
it is consist of three logic, but fairly straightforward.
a) User Login
b) Init Data Repo and Reddit Agents.
c) Input query command.

8. Unit tests
I included some unit tests for the data manipulation part, coverage rate isn't so high. I could write more unit tests if I invest more time.