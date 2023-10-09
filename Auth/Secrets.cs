namespace RedditApp.Auth
{
    public static class Secrets
    {
        public static string UserName = "";

        public static string Password = "";

        public static string TokenAuth = "";

        public static void LoadCredentials()
        {
            var lines = File.ReadAllLines(@".\Auth\credential.psd");
            UserName = lines[0];
            Password = lines[1];
            TokenAuth = lines[2];
        }
    }
}
