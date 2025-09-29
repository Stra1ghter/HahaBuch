using System.Collections;

namespace HahaBuch.SharedContracts;

/// <summary>
/// Service for interacting with transactions (user expenses and incomes).
/// </summary>
public interface ITransactionService
{
    public Task<TransactionDto> PutTransaction(TransactionDto transactionDto);
    
    public Task<IList<TransactionOverviewDto>> GetTransactions();
    
    public Task DeleteTransaction(Guid transactionId);
}