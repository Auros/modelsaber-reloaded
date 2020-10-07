using System.Linq;
using GraphQL.Types;
using ModelSaber.Common;
using Microsoft.AspNetCore.Http;

namespace ModelSaber.API.Models.GraphQL
{
    public class VoteDataType : ObjectGraphType<VoteData>
    {
        public VoteDataType()
        {
            Name = "VoteData";
            Field<VoteType>(
                "authVote",
                "The vote object for the currently authenticated user",
                resolve: context =>
                {
                    ModelSaberContext modelSaberContext = context.Resolve<ModelSaberContext>();
                    HttpContext httpContext = context.HttpContext();
                    User visitor = (User)httpContext.Items["User"];

                    if (visitor != null)
                    {
                        return context.Source.Votes.FirstOrDefault(x => x.Voter.Id == visitor.Id);
                    }
                    return null;
                }
            );
            Field<VoteStatsType>(
                "voteStats",
                resolve: context =>
                {
                    var votes = context.Source.Votes;
                    var upvotes = votes.Count(x => x.IsUpvote);
                    var downvotes = votes.Length - upvotes;
                    return new VoteStats
                    {
                        Upvotes = upvotes,
                        Downvotes = downvotes
                    };
                }
            );
        }
    }
}