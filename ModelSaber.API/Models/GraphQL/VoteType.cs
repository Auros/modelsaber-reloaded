using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class VoteType : ObjectGraphType<Vote>
    {
        public VoteType()
        {
            Name = "Vote";
            Field(type: typeof(GuidGraphType), name: "id", description: "The ID of the playlist.");
            Field(type: typeof(UserType), name: "user", description: "The creator of the playlist.");
            Field(v => v.IsUpvote).Description("Is this vote positive?");
        }
    }
}
