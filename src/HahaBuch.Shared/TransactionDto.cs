using HahaBuch.SharedContracts.DataAnnotations;

namespace HahaBuch.SharedContracts;

public class TransactionDto
{
    public Guid Id { get; set; }

    public Guid? CategoryId { get; set; }

    [LocalizedStringLength(0, 200)]
    public string? Description { get; set; }

    [LocalizedNonZero]
    public decimal Amount { get; set; }

    /// <summary>
    /// Date and time when this transaction took place.
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.Now;
}