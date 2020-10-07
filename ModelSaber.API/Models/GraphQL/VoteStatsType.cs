using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class VoteStatsType : ObjectGraphType<VoteStats>
    {
        public VoteStatsType()
        {
            Name = "VoteStats";
            Field(vs => vs.Upvotes).Description("The amount of upvotes the object has.");
            Field(vs => vs.Downvotes).Description("The amount of downvotes the object has.");
        }
    }
}
