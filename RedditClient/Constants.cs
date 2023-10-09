namespace RedditApp.RedditClient
{
    public static class Constants
    {
        public const string HttpUriPaht = "https://www.reddit.com";
        public const string OathUriPath = "https://oauth.reddit.com";
        public const string NewPostPath = "/r/funny/new/?sort=new&t=hour";
        public const string TokenUriPath = "/api/v1/access_token";
        public const string Accept = "Accept";
        public const string applicationjson = "application/json";
        public const string Accept_Encoding = "Accept-Encoding";
        public const string gzip = "gzip, deflate, br";
        public const string Authorization = "Authorization";
        public const string Bearer = "Bearer ";
        public const string Connection = "Connection";
        public const string KeepAlive = "keep-alive";
        public const string UserAgent = "User-Agent";
        public const string ChangeMeClient = "ChangeMeClient/0.1 by YourUsername";
        public const string Star = "*/*";
        public const string GrantType = "grant_type";
        public const string Password = "password";
        public const string UserName = "username";
        public const string RateLimitRemaining = "x-ratelimit-remaining";
    }
}
