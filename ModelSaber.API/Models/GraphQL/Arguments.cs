using GraphQL.Types;

namespace ModelSaber.API.Models.GraphQL
{
    public static class Arguments
    {
        public static QueryArgument<IntGraphType> Page(string description = "The page number.")
        {
            return new QueryArgument<IntGraphType>
            {
                Name = "page",
                DefaultValue = 0,
                Description = description
            };
        }
    }
}
