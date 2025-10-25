using HahaBuch.Category;
using HahaBuch.Data;
using HahaBuch.SharedContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HahaBuch.Transaction;

/// <summary>
/// Expense or income transaction.
/// </summary>
public class TransactionEntity
{ 
    /// <summary>
    /// Default constructor for EF Core.
    /// </summary>
    public TransactionEntity()
    {  }
    
    /// <summary>
    /// Create a new transaction for a vault.
    /// </summary>
    /// <param name="id">The id of the transaction.</param>
    /// <param name="description">The description of the transaction.</param>
    /// <param name="amount">The amount of Euro for this transaction.</param>
    /// <param name="datetime">The date and time when this transaction took place. Null when this is a new transaction.</param>
    /// <param name="vaultEntityId">The vault id this transaction belongs to.</param>
    /// <param name="categoryId">An optional categoryId that this transaction is associated with.</param>
    public TransactionEntity(Guid id, string? description, decimal amount, DateTime? datetime, Guid vaultEntityId, Guid? categoryId)
    {
        Id = id;
        Description = description;
        Amount = amount;
        VaultEntityId = vaultEntityId;
        CategoryEntityId = categoryId;
        DateTime = datetime ?? DateTime.Now;
    }
    
    [Key]
    public Guid Id { get; set; }
    
    public Guid VaultEntityId { get; set; }
    public VaultEntity VaultEntity { get; set; } = null!;
    
    
    public Guid? CategoryEntityId { get; set; }
    public Category.CategoryEntity? Category { get; set; }
    
    /// <summary>
    /// User-supplied note for this transaction.
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }
    
    public decimal Amount { get; set; }

    // Solutions when switching to PgSQL
    //[Column(TypeName = "timestamp without time zone")] // = Not converted to UTC, stored in local time without time zone information (as this will be used only by the same users)
    public DateTime DateTime { get; set; }
    
    public static TransactionDto MapToDto(TransactionEntity transactionEntity)
        => new TransactionDto
        {
            Id = transactionEntity.Id,
            CategoryId = transactionEntity.CategoryEntityId,
            Description = transactionEntity.Description,
            Amount = transactionEntity.Amount,
            DateTime = transactionEntity.DateTime
        };
    
    public static TransactionOverviewDto MapToOverviewDto(TransactionEntity transactionEntity)
        => new TransactionOverviewDto
        {
            Id = transactionEntity.Id,
            CategoryId = transactionEntity.CategoryEntityId,
            Category = transactionEntity.Category?.Name,
            CategoryColor = transactionEntity.Category?.Color,
            Description = transactionEntity.Description,
            Amount = transactionEntity.Amount,
            DateTime = transactionEntity.DateTime
        };
    
    public static TransactionEntity MapFromDto(TransactionDto transactionDto, Guid vaultId)
        => new TransactionEntity
        (
            id: transactionDto.Id, 
            description: transactionDto.Description,
            amount: transactionDto.Amount,
            datetime: transactionDto.DateTime,
            vaultEntityId: vaultId,
            categoryId: transactionDto.CategoryId
        );
}