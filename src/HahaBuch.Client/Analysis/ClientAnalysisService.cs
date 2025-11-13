using System.Net.Http.Json;
using HahaBuch.SharedContracts;

namespace HahaBuch.Client.Analysis;

public class ClientAnalysisService : IAnalysisService
{
    private readonly HttpClient httpClient;
    private const string BaseUrl = "api/analysis";

    public ClientAnalysisService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<CategoryYearTotalDto>> GetCategoryYearTotals(int year, int month)
    {
        return await httpClient.GetFromJsonAsync<List<CategoryYearTotalDto>>($"{BaseUrl}/categorytotals/{year}/{month}")
               ?? new List<CategoryYearTotalDto>();
    }
}
