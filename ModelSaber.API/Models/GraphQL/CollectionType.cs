using GraphQL;
using System.Linq;
using GraphQL.Types;
using ModelSaber.Common;
using Microsoft.AspNetCore.Http;

namespace ModelSaber.API.Models.GraphQL
{
    public class CollectionType : ObjectGraphType<Collection>
    {
        public CollectionType()
        {
            Name = "Collection";
            Field(type: typeof(GuidGraphType), name: "id", description: "The ID of the collection.");
            Field(c => c.Name).Description("The name of the collection.");
            Field(c => c.IconURL).Description("The local URL of the icon for the collection.");
            Field(c => c.Description, nullable: true).Description("The Markdown description of the collection.");
            Field(c => c.DefaultInstallPath, nullable: true).Description("The default relative path that files in this collection install to.");
            Field<VisibilityType>("defaultVisibility", description: "The default visbility for any models uploaded to this collection.");
            Field<ApprovalStatusType>("defaultApprovalStatus", description: "The default approval status for any models uploaded to this collection.");

            Field<ListGraphType<ModelType>>(
                "models",
                "The models in the collection",
                arguments: new QueryArguments(
                    Arguments.Page(),
                    Arguments.Count()
                ),
                resolve: context =>
                {
                    ModelSaberContext modelSaberContext = context.Resolve<ModelSaberContext>();
                    HttpContext httpContext = context.HttpContext();
                    User visitor = (User)httpContext.Items["User"];

                    int page = context.GetArgument<int>("page");
                    int count = context.GetArgument<int>("count");

                    if (visitor != null)
                    {
                        if (visitor.Role.HasFlag(Role.Manager))
                        {
                            modelSaberContext.Models.Skip(page * count).Take(count);
                        }
                        return modelSaberContext.Models.Where(x => (x.Uploader.Id == visitor.Id) || (x.Status == ApprovalStatus.Approved && x.Visibility == Visibility.Public)).Skip(page * count).Take(count);
                    }
                    return modelSaberContext.Models.Where(x => x.Status == ApprovalStatus.Approved && x.Visibility == Visibility.Public).Skip(page * count).Take(count);
                }
            );
        }
    }

    public class VisibilityType : EnumerationGraphType
    {
        public VisibilityType()
        {
            Name = "Visibility";
            Description = "The visibility of a model.";
            AddValue("Public", "Everyone is able to view public models.", 0);
            AddValue("Private", "Only the creator can see private models.", 1);
            AddValue("Unlisted", "Anyone with the direct link will be able to see unlisted models.", 2);
        }
    }

    public class ApprovalStatusType : EnumerationGraphType
    {
        public ApprovalStatusType()
        {
            Name = "ApprovalStatus";
            Description = "The approval status of a model.";
            AddValue("Denied", "This model is denied.", 0);
            AddValue("Approved", "This model is denied.", 1);
            AddValue("Unapproved", "This model is denied.", 2);
        }
    }
}