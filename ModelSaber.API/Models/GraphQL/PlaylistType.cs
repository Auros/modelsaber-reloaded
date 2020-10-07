using System.Linq;
using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class PlaylistType : ObjectGraphType<Playlist>
    {
        public PlaylistType()
        {
            Name = "Playlist";
            Field(type: typeof(GuidGraphType), name: "id", description: "The ID of the playlist.");
            Field(type: typeof(UserType), name: "user", description: "The creator of the playlist.");
            Field(type: typeof(ModelType), name: "models", description: "The models in this playlist.");
            Field(p => p.Name).Description("The name of the playlist.");
            Field(p => p.Description).Description("The Markdown description of the playlist.");
            Field(p => p.ThumbnailURL).Description("The relative URL of the thumbnail for the playlist.");
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
            Field<ListGraphType<CommentType>>(
                "comments",
                "The comments on the model.",
                resolve: context =>
                {
                    ModelSaberContext modelSaberContext = context.Resolve<ModelSaberContext>();
                    return modelSaberContext.Comments.Where(c => c.Source == context.Source.Id);
                }
            );
        }
    }
}