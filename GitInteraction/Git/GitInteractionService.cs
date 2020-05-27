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

        public IList<DTO.Branch> GetRepositoryRemoteBranches(string repositoryPath)
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
                            // commit level
                            DTO.Commit commitCopy = new DTO.Commit
                            {
                                CommitSha = commit.Sha,
                                Message = commit.Message
                            };

                            List<DTO.Commit> listOfParents = new List<DTO.Commit>();
                            IEnumerable<Commit> innerCommitParentsNode = commit.Parents;
                            while (innerCommitParentsNode.Any())
                            {
                                foreach (Commit innerCommit in innerCommitParentsNode)
                                {
                                    DTO.Commit parentCopy = new DTO.Commit
                                    {
                                        CommitSha = innerCommit.Sha,
                                        Message = innerCommit.Message
                                    };
                                    innerCommitParentsNode = innerCommit.Parents;

                                    listOfParents.Add(parentCopy);
                                }
                            }

                            commitCopy.Parents = listOfParents;
                            listOfCommits.Add(commitCopy);
                        }
                        branchCopy.Commits = listOfCommits;

                        branches.Add(branchCopy);
                    }
                }
            }

            return branches;
        }

        public IList<DTO.Commit> GetRepositoryCommits(string repositoryPath)
        {
            List<DTO.Commit> commits = new List<DTO.Commit>();
            using (Repository repo = new Repository(repositoryPath))
            {
                foreach (Commit commit in repo.Commits)
                {
                    commits.Add(new DTO.Commit() { CommitSha = commit.Sha, Message = commit.Message });
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
