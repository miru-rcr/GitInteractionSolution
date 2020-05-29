using GitInteraction.Git;
using GitInteraction.GitHub;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace GitInteraction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string chosenCommitId = string.Empty;
            string destinationPath = ConfigurationManager.AppSettings["destinationFolder"];
            string gitHubRepoOwner = ConfigurationManager.AppSettings["gitUsername"];
            string gitHubRepoName = ConfigurationManager.AppSettings["repoName"];
            string historyJsonPath = ConfigurationManager.AppSettings["historyJsonPath"];

            GitInteractionService gitInteractionService = new GitInteractionService();
            GitHubInteractionService gitHubInteractionService = new GitHubInteractionService();
            string cloneUrl = gitHubInteractionService.GetRepositoryCloneUrl(gitHubRepoOwner, gitHubRepoName);

            try
            {
                string clonedRepositoryPath = gitInteractionService.CloneRepository(cloneUrl, destinationPath);

                IList<DTO.Commit> repositoryCommits = gitInteractionService.GetRepositoryCommits(clonedRepositoryPath);
                chosenCommitId = repositoryCommits.First().CommitSha;
                gitInteractionService.RestoreRepositoryToPreviousState(clonedRepositoryPath, chosenCommitId);

                //list of branches + history
                IList<DTO.Branch> branches = gitInteractionService.GetRemoteBranchesHistory(clonedRepositoryPath);
                string jsonBranches = JsonConvert.SerializeObject(branches, Formatting.Indented);
                WriteJsonToFile(jsonBranches, historyJsonPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            Console.WriteLine(value: $"Clone for repository is done! Restored to: {chosenCommitId}");
            Console.ReadLine();
            
        }

        private static void WriteJsonToFile(string jsonContent, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine(jsonContent);
                sw.Close();
            }
        }
    }
}
