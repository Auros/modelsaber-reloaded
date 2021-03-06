﻿using System.Linq;
using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Name = "User";
            Field(type: typeof(GuidGraphType), name: "id", description: "The ID of the user.");
            Field<IntGraphType>("Role", "The role of the user.", resolve: context => (int)context.Source.Role);
            Field(u => u.Biography, nullable: true).Description("The Markdown bio of the user.");
            Field(type: typeof(DiscordUserType), name: "profile", description: "The discord profile of the user.");
            Field<VoteDataType>(
                "voteData",
                "The vote data of the user.",
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
                "The comments on the user.",
                resolve: context =>
                {
                    ModelSaberContext modelSaberContext = context.Resolve<ModelSaberContext>();
                    return modelSaberContext.Comments.Where(c => c.Source == context.Source.Id);
                }
            );
        }
    }
}