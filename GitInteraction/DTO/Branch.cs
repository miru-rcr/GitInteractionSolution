using System.Collections.Generic;

namespace GitInteraction.DTO
{
    internal class Branch
    {
        public bool IsRemote { get; set; }
        public string Name { get; set; }
        public IList<Commit> Commits { get; set; }
    }
}
