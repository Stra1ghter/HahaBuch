using HahaBuch.Data;
using HahaBuch.SharedContracts;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.EntityFrameworkCore;

namespace HahaBuch.Transaction;

public class TransactionService(IDbContextFactory<ApplicationDbContext> dbContextFactory, VaultAccessor vaultAccessor) : ITransactionService
{
    public async Task<TransactionDto> PutTransaction(TransactionDto transactionDto)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();
        
        if (transactionDto.Id != Guid.Empty)
        {
            TransactionEntity? existing = await context.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == transactionDto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Transaction not found.");
            
            if (existing.VaultEntityId != usersVaultId)
                throw new UnauthorizedAccessException("You do not have access to this transaction.");
        }
        
        TransactionEntity entity = TransactionEntity.MapFromDto(transactionDto, usersVaultId);
        
        context.Update(entity); // Auto-generated key will be inserted when it has the default value

        await context.SaveChangesAsync();
        
        return TransactionEntity.MapToDto(entity);
    }

    public async ValueTask<GridItemsProviderResult<TransactionOverviewDto>> GetTransactions(GridItemsProviderRequest<TransactionOverviewDto> request)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();
        
        var query = context.Transactions
            .Where(t => t.VaultEntityId == usersVaultId)
            .Include(t => t.Category)
            .OrderByDescending(t => t.DateTime);
        
        var totalCount = await query.CountAsync();
        
        var transactions = await query
            .Skip(request.StartIndex)
            .Take(request.Count ?? 50)
            .Select(t => TransactionEntity.MapToOverviewDto(t))
            .ToListAsync();
        
        return new GridItemsProviderResult<TransactionOverviewDto>
        {
            Items = transactions,
            TotalItemCount = totalCount
        };
    }

    public async Task DeleteTransaction(Guid transactionId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        Guid usersVaultId = await vaultAccessor.GetUsersVaultId();
        
        var transaction = await context.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId && t.VaultEntityId == usersVaultId);

        if (transaction == null)
        {
            throw new KeyNotFoundException("Transaction not found.");
        }

        if (transaction.VaultEntityId != usersVaultId)
        {
            throw new UnauthorizedAccessException("You do not have access to this transaction.");
        }
        
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();
    }
}
