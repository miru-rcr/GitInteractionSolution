using System.Collections.Generic;
using System.Dynamic;

namespace GitInteraction.DTO
{
    internal class Commit
    {
        public string CommitSha { get; set; }
        public string Message { get; set; }
        public string AuthorName { get; set; }
        public IList<Commit> Parents { get; set; }
    }
}
