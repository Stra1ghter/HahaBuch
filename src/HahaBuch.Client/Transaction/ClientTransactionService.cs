using System.Net.Http.Json;
using HahaBuch.SharedContracts;

namespace HahaBuch.Client.Transaction;

public class ClientTransactionService(HttpClient httpClient) : ITransactionService
{
    private const string BaseUrl = "api/transactions";
        
    public async Task<TransactionDto> PutTransaction(TransactionDto transactionDto)
    {
        var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/{transactionDto.Id}", transactionDto);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<TransactionDto>()
            ?? throw new InvalidOperationException("Failed to deserialize the response.");
    }

    public async Task<IList<TransactionOverviewDto>> GetTransactions()
    {
        var response = await httpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IList<TransactionOverviewDto>>()
            ?? throw new InvalidOperationException("Failed to deserialize the response.");
    }

    public async Task DeleteTransaction(Guid transactionId)
    {
        var response = await httpClient.DeleteAsync($"{BaseUrl}/{transactionId}");
        response.EnsureSuccessStatusCode();
    }
}