// using AccountyMinAPI.Data;

// namespace AccountyMinAPI.Api;

// public static class CategoryAPI
// {
//     public static async Task<IResult> GetAllCategories(ICategoryRepository repo)
//     {
//         try
//         {
//             var categories = await repo.GetAllCategories();
//             List<CategoryGetDto> categoriesDto = new();
//             foreach (var category in categories)
//             {
//                 CategoryModel.ToCategoryGetDto(category, out CategoryGetDto dto);
//                 categoriesDto.Add(dto);
//             }
//             return Results.Ok(categoriesDto);
//         }
//         catch (System.Exception ex)
//         {
//             return Results.Problem(ex.Message);
//         }
//     }

//     public static async Task<IResult> InsertCategory(CategoryPostDto dto, ICategoryRepository repo)
//     {
//         try
//         {
//             var mappingResponse = CategoryPostDto.ToCategoryModel(dto, out CategoryModel category);
//             switch (mappingResponse)
//             {
//                 case MappingResponse.OK:
//                     await repo.InsertCategory(category);
//                     return Results.Ok($"Category added - {category.Name}");
//                 case MappingResponse.MISSING_CATEGORY:
//                     return Results.BadRequest("Missing category");
//                 default:
//                     return Results.Problem("Something went wrong");
//             }
//         }
//         catch (CategoryAlreadyExistsException ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//         catch (Exception ex)
//         {
//             return Results.Problem(ex.Message);
//         }
//     }

//     public static async Task<IResult> DeleteCategory(string id, ICategoryRepository repo)
//     {
//         try
//         {
//             var deleteResult = await repo.DeleteCategoryById(id);
//             if (deleteResult)
//                 return Results.Ok($"Category deleted - {id}");
//             return Results.BadRequest($"Category id not found - {id}");
//         }
//         catch (Exception ex)
//         {
//             return Results.Problem(ex.Message);
//         }
//     }
// }