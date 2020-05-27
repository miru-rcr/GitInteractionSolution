using System.Configuration;
using LibGit2Sharp;

namespace GitInteraction.Git
{
    internal class CredentialsService : ICredentialsService
    {
        private static readonly string gitUsername = ConfigurationManager.AppSettings["gitUsername"];
        private static readonly string gitPassword = ConfigurationManager.AppSettings["gitPassword"];

        public UsernamePasswordCredentials GetCredentials()
        {
            return new UsernamePasswordCredentials
            {
                Username = gitUsername,
                Password = gitPassword
            };
        }
    }
}