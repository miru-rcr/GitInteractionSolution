using System.Collections.Generic;

namespace GitInteraction.DTO
{
    internal class Commit
    {
        public string CommitSha { get; set; }
        public string Message { get; set; }
        public IList<Commit> Parents { get; set; }
    }
}
