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
            Field(u => u.Profile).Description("The discord profile of the user.");
        }
    }
}