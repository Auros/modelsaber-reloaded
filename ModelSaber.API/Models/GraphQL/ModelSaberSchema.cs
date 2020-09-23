using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace ModelSaber.API.Models.GraphQL
{
    public class ModelSaberSchema : Schema
    {
        public ModelSaberSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<ModelSaberQuery>();
        }
    }
}