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
    /// A user-selected abbreviation for the category, e.g. "FO" for Food.
    /// </summary>
    [StringLength(4)] public string? Abbreviation { get; set; }
    
    [StringLength(6)]
    public string Color { get; set; } = null!;
    
    public IEnumerable<TransactionEntity> Transactions { get; set; } = null!;
    
    public static CategoryDto MapToDto(CategoryEntity categoryEntity)
        => new CategoryDto()
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name,
            Abbreviation = categoryEntity.Abbreviation,
            RgbColorString = categoryEntity.Color
        };

    public static CategoryEntity MapFromDto(CategoryDto category, Guid vaultId)
        => new CategoryEntity()
        {
            Id = category.Id,
            Name = category.Name,
            Abbreviation = category.Abbreviation,
            Color = category.RgbColorString,
            VaultEntityId = vaultId,
        };
}