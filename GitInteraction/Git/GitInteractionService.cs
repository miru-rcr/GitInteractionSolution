using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LibGit2Sharp;

namespace GitInteraction.Git
{
    internal class GitInteractionService : IGitInteractionService
    {
        public string CloneRepository(string cloneUrl, string destinationPath)
        {
            string destinationFolder = ConfigurationManager.AppSettings["destinationFolder"];

            //CloneOptions cloneOptions = new CloneOptions
            //{
            //    CredentialsProvider = (url, user, cred) => new CredentialsService().GetCredentials()
            //};
            //string gitRepoPath = Repository.Clone(cloneUrl, destinationFolder, cloneOptions);

            string gitRepoPath = Repository.Clone(cloneUrl, destinationFolder);

            return gitRepoPath;
        }

        public IList<DTO.Branch> GetRemoteBranchesHistory(string repositoryPath)
        {
            List<DTO.Branch> branches = new List<DTO.Branch>();
            using (Repository repo = new Repository(repositoryPath))
            {
                foreach (Branch branch in repo.Branches)
                {
                    if (branch.IsRemote)
                    {
                        DTO.Branch branchCopy = new DTO.Branch
                        {
                            IsRemote = branch.IsRemote,
                            Name = branch.FriendlyName
                        };

                        List<DTO.Commit> listOfCommits = new List<DTO.Commit>();

                        foreach (Commit commit in branch.Commits)
                        {
                           listOfCommits.Add(ComputeParentsRecursively(commit));
                        }

                        branchCopy.Commits = listOfCommits;
                         
                        branches.Add(branchCopy);
                    }
                }
            }

            return branches;
        }

        private DTO.Commit ComputeParentsRecursively(Commit commit)
        {
            var newCommit = new DTO.Commit
            {
                Parents = new List<DTO.Commit>()
            };

            if (commit.Parents.Any())
            {
                newCommit.CommitSha = commit.Sha;
                newCommit.Message = commit.Message;
                newCommit.AuthorName = commit.Author.Name;
                foreach(var parent in commit.Parents)
                {
                    newCommit.Parents.Add(ComputeParentsRecursively(parent));
                }
            }
            else
            {
                return new DTO.Commit()
                {
                    CommitSha = commit.Sha,
                    Message = commit.Message,
                    AuthorName = commit.Author.Name,
                    Parents = null
                };
            }

            return newCommit;
        }

        public IList<DTO.Commit> GetRepositoryCommits(string repositoryPath)
        {
            List<DTO.Commit> commits = new List<DTO.Commit>();
            using (Repository repo = new Repository(repositoryPath))
            {
                foreach (Commit commit in repo.Commits)
                {
                    commits.Add(new DTO.Commit() { CommitSha = commit.Sha, Message = commit.Message, AuthorName = commit.Author.Name });
                }
            }

            return commits;
        }

        public void RestoreRepositoryToPreviousState(string repositoryPath, string commitId)
        {
            using (Repository repo = new Repository(repositoryPath))
            {
                repo.Reset(ResetMode.Hard, commitId);
            }
        }
    }
}
