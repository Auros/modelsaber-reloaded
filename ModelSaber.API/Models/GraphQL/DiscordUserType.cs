using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class DiscordUserType : ObjectGraphType<DiscordUser>
    {
        public DiscordUserType()
        {
            Field(du => du.Id).Description("The discord ID of the user profile.");
            Field(du => du.Username).Description("The discord username of the user profile.");
            Field(du => du.Discriminator).Description("The discord discriminator (the four numbers at the end of a profile) of the user profile.");
            Field(du => du.Avatar).Description("The URL of the profile picture of the user profile.");
        }
    }
}