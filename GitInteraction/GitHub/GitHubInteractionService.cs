using Octokit;

namespace GitInteraction.GitHub
{
    internal class GitHubInteractionService : IGitHubInteractionService
    {
        private readonly IGitHubSecurityService gitHubSecurityService;

        public GitHubInteractionService()
        {
            gitHubSecurityService = new GitHubSecurityService();
        }

        public string GetRepositoryCloneUrl(string owner, string repoName)
        {
            GitHubClient client = gitHubSecurityService.ConnectUsingBasicAuth();
            return client.Repository.Get(owner, repoName).GetAwaiter().GetResult().CloneUrl;
        }
    }
}