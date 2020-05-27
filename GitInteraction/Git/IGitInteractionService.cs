using System.Collections.Generic;
using GitInteraction.DTO;

namespace GitInteraction.Git
{
    internal interface IGitInteractionService
    {
        string CloneRepository(string cloneUrl, string destinationPath);
        void RestoreRepositoryToPreviousState(string repositoryPath, string commitId);
        IList<Commit> GetRepositoryCommits(string repositoryPath);
        IList<Branch> GetRepositoryRemoteBranches(string repositoryPath);
    }
}
