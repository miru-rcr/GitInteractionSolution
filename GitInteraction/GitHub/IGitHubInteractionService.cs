namespace GitInteraction.GitHub
{
    internal interface IGitHubInteractionService
    {
        string GetRepositoryCloneUrl(string owner, string repoName);
    }
}
