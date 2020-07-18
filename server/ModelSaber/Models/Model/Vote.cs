using MSUser = ModelSaber.Models.User.User;

namespace ModelSaber.Models.Model
{
    public class Vote
    {
        public ulong ID { get; set; }
        public MSUser Voter { get; set; }
        public string VoterID { get; set; }
        public bool IsUpvote { get; set; }
    }
}