using System.Transactions;
using HahaBuch.SharedContracts.DataAnnotations;

namespace HahaBuch.SharedContracts;

public class TransactionOverviewDto
{
    public Guid Id { get; set; }
    
    public Guid? CategoryId { get; set; }

    public string? Category { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    /// <summary>
    /// Date and time when this transaction took place.
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.Now;

    public TransactionDto MapToTransactionDto()
        => new ()
        {
            Id = Id,
            CategoryId = CategoryId,
            Description = Description,
            Amount = Amount,
            DateTime = DateTime
        };

    /// <summary>
    /// Map from TransactionDto to TransactionOverviewDto.
    /// NOTE: don't forget to set the category name manually if needed.
    /// </summary>
    /// <param name="transactionDto">The transaction DTO to map from.</param>
    /// <returns>The mapped TransactionOverviewDTO.</returns>
    public static TransactionOverviewDto MapFromTransactionDto(TransactionDto transactionDto)
        => new()
        {
            Id = transactionDto.Id,
            CategoryId = transactionDto.CategoryId,
            Description = transactionDto.Description,
            Amount = transactionDto.Amount,
            DateTime = transactionDto.DateTime,
        };
}