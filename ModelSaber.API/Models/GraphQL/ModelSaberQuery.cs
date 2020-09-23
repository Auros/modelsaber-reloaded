using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.API.Models.GraphQL
{
    public class ModelSaberQuery : ObjectGraphType
    {
        public ModelSaberQuery()
        {
            Name = "Query";
            FieldAsync<ListGraphType<CollectionType>>(
                "collections",
                resolve: async context =>
                {
                    ModelSaberContext modelSaberContext = context.Resolve<ModelSaberContext>();
                    return await modelSaberContext.Collections.ToListAsync();
                }
            );
        }
    }
}