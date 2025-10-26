using System.ComponentModel.DataAnnotations;
using HahaBuch.Data;
using HahaBuch.SharedContracts;
using HahaBuch.Transaction;

namespace HahaBuch.Category;

/// <summary>
/// Category entity for assigning transactions to a category.
/// </summary>
public class CategoryEntity
{
    public CategoryEntity()
    { }
    
    public CategoryEntity(Guid vaultId, string name)
    {
        Name = name;
    }
    
    [Key]
    public Guid Id { get; set; }
    public Guid VaultEntityId { get; set; }
    public VaultEntity VaultEntity { get; set; } = null!;
    
    [StringLength(30)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// A user-selected description for the category, e.g. for tooltips when entering transactions.
    /// </summary>
    [StringLength(120)] public string? Description { get; set; }
    
    [StringLength(6)]
    public string Color { get; set; } = null!;
    
    public IEnumerable<TransactionEntity> Transactions { get; set; } = null!;
    
    public static CategoryDto MapToDto(CategoryEntity categoryEntity)
        => new CategoryDto()
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name,
            Description = categoryEntity.Description,
            RgbColorString = categoryEntity.Color
        };

    public static CategoryEntity MapFromDto(CategoryDto category, Guid vaultId)
        => new CategoryEntity()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.RgbColorString,
            VaultEntityId = vaultId,
        };
}