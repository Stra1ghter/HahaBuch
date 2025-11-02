namespace HahaBuch.SharedContracts;

/// <summary>
/// Service for retrieving analysis results for the current user.
/// </summary>
public interface IAnalysisService
{
    /// <summary>
    /// Get total amounts spent per category for the selected year (only the current user's vault).
    /// </summary>
    Task<List<CategoryYearTotalDto>> GetCategoryYearTotals(int year);
}
