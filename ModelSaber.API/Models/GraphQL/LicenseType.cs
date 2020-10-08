using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class LicenseType : ObjectGraphType<License>
    {
        public LicenseType()
        {
            Name = "License";
            Field(l => l.SourceURL).Description("The URL of the source of the model.");
            Field(l => l.LicenseURL).Description("The URL of the license of the model.");
        }
    }
}