using HahaBuch.SharedContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Net.Http.Json;

namespace HahaBuch.Client.Transaction;

public class ClientTransactionService(HttpClient httpClient, NavigationManager navigationManager) : ITransactionService
{
    private const string BaseUrl = "api/transactions";

    public async Task<TransactionDto> PutTransaction(TransactionDto transactionDto)
    {
        var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/{transactionDto.Id}", transactionDto);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<TransactionDto>()
            ?? throw new InvalidOperationException("Failed to deserialize the response.");
    }

    private async ValueTask<GridItemsProviderResult<TransactionOverviewDto>> GetTransactions(
        GridItemsProviderRequest<TransactionOverviewDto> request)
    {
        string url = navigationManager.GetUriWithQueryParameters(BaseUrl, new Dictionary<string, object?>
            {
                { "StartIndex", request.StartIndex },
                { "Count", request.Count },
            });

        var response = await httpClient.GetAsync(url, request.CancellationToken);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<GridItemsProviderResult<TransactionOverviewDto>>(cancellationToken: request.CancellationToken);

        return items;
    }

    public async Task DeleteTransaction(Guid transactionId)
    {
        var response = await httpClient.DeleteAsync($"{BaseUrl}/{transactionId}");
        response.EnsureSuccessStatusCode();
    }

    ValueTask<GridItemsProviderResult<TransactionOverviewDto>> ITransactionService.GetTransactions(GridItemsProviderRequest<TransactionOverviewDto> request)
    {
        return GetTransactions(request);
    }
}