using Octokit;

namespace GitInteraction.GitHub
{
    internal interface IGitHubSecurityService
    {
        GitHubClient ConnectUsingBasicAuth();
        GitHubClient ConnectUsingOAuth();
    }
}
