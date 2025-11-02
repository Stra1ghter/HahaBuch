namespace HahaBuch.SharedContracts;

/// <summary>
/// Total amounts per category within a given year for the current user's vault.
/// Provides separate sums for expenses (negative amounts) and income (positive amounts).
/// </summary>
public class CategoryYearTotalDto
{
    public Guid CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    /// <summary>
    /// 6 character hex color (without #) of the category.
    /// </summary>
    public string CategoryColor { get; set; } = string.Empty;
    public int Year { get; set; }
    /// <summary>
    /// Sum of all expense transactions (originally negative amounts) returned as a positive number.
    /// </summary>
    public decimal ExpenseTotal { get; set; }
    /// <summary>
    /// Sum of all income transactions (positive amounts).
    /// </summary>
    public decimal IncomeTotal { get; set; }
}
