using HahaBuch.Data;
using HahaBuch.SharedContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HahaBuch.Analysis;

public class AnalysisService(IDbContextFactory<ApplicationDbContext> dbContextFactory, VaultAccessor vaultAccessor, IStringLocalizer<SharedResources> Loc) : IAnalysisService
{
    public async Task<List<CategoryYearTotalDto>> GetCategoryYearTotals(int year)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        Guid vaultId = await vaultAccessor.GetUsersVaultId();

        DateTime start = new DateTime(year, 1, 1);
        DateTime endExclusive = start.AddYears(1);

        var query = context.Transactions
            .Include(t => t.Category)
            .Where(t => t.VaultEntityId == vaultId && t.DateTime >= start && t.DateTime < endExclusive)
            .GroupBy(t => t.CategoryEntityId)
            .Select(g => new CategoryYearTotalDto
            {
                CategoryId = g.Key ?? Guid.Empty,
                Category = g.First().Category != null ? g.First().Category!.Name : Loc["Other"],
                CategoryColor = g.First().Category != null ? g.First().Category!.Color : "000000",
                Year = year,
                ExpenseTotal = -g.Where(t => t.Amount < 0).Sum(t => t.Amount), // negate to make positive
                IncomeTotal = g.Where(t => t.Amount > 0).Sum(t => t.Amount)
            });

        return await query.ToListAsync();
    }
}
