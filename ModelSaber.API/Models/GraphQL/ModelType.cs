using GraphQL.Types;
using ModelSaber.Common;

namespace ModelSaber.API.Models.GraphQL
{
    public class ModelType : ObjectGraphType<Model>
    {
        public ModelType()
        {
            Name = "Model";
            Field(type: typeof(GuidGraphType), name: "id", description: "The ID of the model.");
            Field(m => m.Name).Description("The name of the model.");
            Field(m => m.Hash).Description("The MD5 hash of the model file.");
            Field(m => m.Tags).Description("The tags of the model.");
            Field(type: typeof(UserType), name: "uploader", description: "The uploader of the model.");
            Field(m => m.DownloadCount).Description("The download count of the model.");
            Field<FileTypeType>("fileType", description: "The file type of the model file.");
            Field(m => m.Description).Description("The Markdown description of the model.");
            Field(m => m.DownloadURL).Description("The relative URL to download the model.");
            Field(m => m.ThumbnailURL).Description("The relative URL of the thumbnail for the model.");
            Field(m => m.UploadDate).Description("The time (UTC) when the model was uploaded.");
            Field(type: typeof(CollectionType), name: "collection", description: "The collection the model is present on.");
            Field<VisibilityType>("visibility", description: "The visibility of this model.");
            Field<ApprovalStatusType>("approvalStatus", description: "The approval status of this model.");
        }
    }

    public class FileTypeType : EnumerationGraphType
    {
        public FileTypeType()
        {
            Name = "FileType";
            Description = "The file type of a model file.";
            AddValue("Other", "Unknown file type.", 0);
            AddValue("Single", "This model file is a singular file.", 1);
            AddValue("Archive", "This model file is an archive that needs to be unzipped.", 2);
        }
    }
}
