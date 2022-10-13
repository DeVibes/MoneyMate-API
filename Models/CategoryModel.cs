using AccountyMinAPI.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountyMinAPI.Models;
public record CategoryModel
{
    public ObjectId Id { get; init; }
    public string Name { get; init; } = String.Empty;
    
    //TODO Handle mapping errors from DB
    public static MappingResponse ToCategoryGetDto(CategoryModel model, out CategoryGetDto dto)
    {
        dto = new()
        {
            Id = model.Id.ToString(),
            Name = model.Name
        };
        return MappingResponse.OK;
    }
}