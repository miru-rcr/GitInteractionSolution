using System;
using System.Configuration;
using System.Reflection;
using Octokit;

namespace GitInteraction.GitHub
{
    internal class GitHubSecurityService : IGitHubSecurityService
    {
        private static readonly string GitHubIdentity =
            Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;


        public GitHubClient ConnectUsingBasicAuth()
        {
            //string gitUsername = ConfigurationManager.AppSettings["gitUsername"];
            //string gitPassword = ConfigurationManager.AppSettings["gitPassword"];
            string privateAccessToken = ConfigurationManager.AppSettings["token"];
            ProductHeaderValue productInfo = new ProductHeaderValue(GitHubIdentity);

            //Credentials credentials = new Credentials(gitUsername, gitPassword, AuthenticationType.Basic);
            //OR
            Credentials credentials = new Credentials(privateAccessToken);
            GitHubClient gitHubClient = null;
            try
            {
                gitHubClient = new GitHubClient(productInfo) { Credentials = credentials };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            return gitHubClient;
        }

        public GitHubClient ConnectUsingOAuth()
        {
            string clientId = ConfigurationManager.AppSettings["clientId"];
            string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            string authCode = ConfigurationManager.AppSettings["authCode"];

            ProductHeaderValue productInfo = new ProductHeaderValue(GitHubIdentity);

            string token = RequestOAuthToken(clientId, clientSecret, authCode);

            Credentials credentials = new Credentials(token);

            return new GitHubClient(productInfo) { Credentials = credentials };
        }

        private string RequestOAuthToken(string clientId, string clientSecret, string authCode)
        {
            ProductHeaderValue productInfo = new ProductHeaderValue(GitHubIdentity);
            GitHubClient client = new GitHubClient(productInfo);

            OauthToken oauthToken =
                client.Oauth.CreateAccessToken(new OauthTokenRequest(clientId, clientSecret, authCode)).GetAwaiter().GetResult();

            return oauthToken.AccessToken;
        }
    }
}