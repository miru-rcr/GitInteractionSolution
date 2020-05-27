using LibGit2Sharp;

namespace GitInteraction.Git
{
    internal interface ICredentialsService
    {
        UsernamePasswordCredentials GetCredentials();
    }
}
