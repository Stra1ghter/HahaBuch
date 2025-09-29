namespace HahaBuch.SharedContracts;

/// <summary>
/// Service for managing categories for the current user.
/// Will automatically filter for the current user's vault.
/// </summary>
public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategories();
    Task<CategoryDto> GetCategory(Guid categoryId);
    Task<CategoryDto> PutCategory(CategoryDto categoryDto);
    Task DeleteCategory(Guid categoryId);
}