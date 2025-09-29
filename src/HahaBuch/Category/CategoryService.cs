using System.Text.RegularExpressions;
using HahaBuch.Data;
using HahaBuch.SharedContracts;
using HahaBuch.SharedContracts.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HahaBuch.Category;

public class CategoryService(IDbContextFactory<ApplicationDbContext> dbContextFactory, VaultAccessor vaultAccessor) : ICategoryService
{
    public async Task<List<CategoryDto>> GetCategories()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();

        return await context.Categories
            .Where(c => c.VaultEntityId == usersVaultId)
            .Select(c => CategoryEntity.MapToDto(c))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetCategory(Guid categoryId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();
        
        return await context.Categories
            .Where(c => c.VaultEntityId == usersVaultId && c.Id == categoryId)
            .Select(c => CategoryEntity.MapToDto(c))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<CategoryDto?> PutCategory(CategoryDto categoryDto)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();
        
        categoryDto.RgbColorString = categoryDto.RgbColorString.ToLowerInvariant();
        ValidateColor(categoryDto);
        
        if (categoryDto.Id != Guid.Empty)
        {
            CategoryEntity? existing = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == categoryDto.Id);
            
            // Set guid in the request means the category should already exist
            if (existing == null)
                throw new KeyNotFoundException("Category not found.");
            
            // User might maliciously try to update another user's category and steal it
            if (existing.VaultEntityId != usersVaultId)
                throw new UnauthorizedAccessException("You do not have access to this category.");
        }
        
        CategoryEntity entity = CategoryEntity.MapFromDto(categoryDto, usersVaultId);

        context.Update(entity); // Auto-generated key will be inserted when it has the default value
        
        await context.SaveChangesAsync();
        
        return CategoryEntity.MapToDto(entity);
    }

    public async Task DeleteCategory(Guid id)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();
        
        CategoryEntity? existing = await context.Categories
            .Include(c => c.Transactions)
            .FirstOrDefaultAsync(x => x.Id == id && x.VaultEntityId == usersVaultId);

        if (existing == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        if (existing.Transactions.Any())
        {
            throw new ReferencedByEntitiesException("Could not delete category, it used by transactions.");
        }
        
        context.Categories.Remove(existing);
        await context.SaveChangesAsync();
    }

    private void ValidateColor(CategoryDto categoryDto)
    {
        if (!RgbColorPattern.IsMatch(categoryDto.RgbColorString))
            throw new FormatException("Color must be a valid 6-digit hexadecimal RGB color code.");
    }

    private static Regex RgbColorPattern => new Regex(@"^[0-9a-f]{6}$", RegexOptions.Compiled);
}