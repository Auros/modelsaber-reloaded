using System.Linq;
using System.Collections.Generic;

namespace ModelSaber.Models.Model
{
    public class ModelStats
    {
        public ulong ID { get; set; }
        public int Downloads { get; set; }
        public int Upvotes => Votes.Count(v => v.IsUpvote);
        public int Downvotes => Votes.Count(v => !v.IsUpvote);
        public List<Vote> Votes { get; set; } = new List<Vote>();
    }
}