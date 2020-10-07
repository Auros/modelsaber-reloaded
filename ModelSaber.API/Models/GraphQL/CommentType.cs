using System.Linq;
using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class CommentType : ObjectGraphType<Comment>
    {
        public CommentType()
        {
            Name = "Comment";
            Field(type: typeof(GuidGraphType), name: "id", description: "The ID of the comment.");
            Field(type: typeof(GuidGraphType), name: "source", description: "The ID of the object being commented.");
            Field(type: typeof(UserType), name: "commenter", description: "The creator of the comment.");
            Field(c => c.Message).Description("The message associated with the comment.");
            Field(c => c.Time).Description("The time the comment was sent.");
            Field<VoteDataType>(
                "voteData",
                "The vote data of the model",
                resolve: context =>
                {
                    ModelSaberContext modelSaberContext = context.Resolve<ModelSaberContext>();
                    return new VoteData
                    {
                        Votes = modelSaberContext.Votes.Where(v => v.Source == context.Source.Id).ToArray(),
                    };
                }
            );
        }
    }
}