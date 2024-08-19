using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = operation.RequestBody?.Content;
        if (parameters != null && parameters.ContainsKey("multipart/form-data"))
        {
            operation.RequestBody.Content["multipart/form-data"].Schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["file"] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                }
            };
        }
    }
}
