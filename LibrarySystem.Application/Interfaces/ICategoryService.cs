using LibrarySystem.Application.Dtos.CategoryDtos;

namespace LibrarySystem.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
    Task<CategoryDto> GetOneCategoryAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CategoryDtoForInsertion categoryDtoForInsertion);
    Task UpdateCategoryAsync(CategoryDtoForUpdate categoryDtoForUpdate);
    Task DeleteCategoryAsync(int id);
}