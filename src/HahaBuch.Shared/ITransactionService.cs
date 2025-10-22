using Microsoft.AspNetCore.Components.QuickGrid;

namespace HahaBuch.SharedContracts;

/// <summary>
/// Service for interacting with transactions (user expenses and incomes).
/// </summary>
public interface ITransactionService
{
    public Task<TransactionDto> PutTransaction(TransactionDto transactionDto);

    public ValueTask<GridItemsProviderResult<TransactionOverviewDto>> GetTransactions(GridItemsProviderRequest<TransactionOverviewDto> request);
    
    public Task DeleteTransaction(Guid transactionId);
}