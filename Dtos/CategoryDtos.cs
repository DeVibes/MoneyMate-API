// namespace AccountyMinAPI.Dtos;

// public record CategoryGetDto
// {
//     public string Id { get; init; } = String.Empty;
//     public string Name { get; init; } = String.Empty;
// }

// public record CategoryPostDto
// {
//     public string Name { get; init; } = String.Empty;
//     public static MappingResponse ToCategoryModel(CategoryPostDto dto, out CategoryModel model)
//     {
//         model = null;
//         if (String.IsNullOrEmpty(dto.Name))
//             return MappingResponse.MISSING_CATEGORY;
//         model = new() { Name = dto.Name };
//         return MappingResponse.OK;
//     }
// }