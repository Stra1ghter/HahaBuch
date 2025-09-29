using System.Net.Http.Json;
using HahaBuch.SharedContracts;
using HahaBuch.SharedContracts.Exceptions;

namespace HahaBuch.Client.Services;

public class ClientCategoryService : ICategoryService
{
    private readonly HttpClient httpClient;
    private readonly string baseUrl = "api/categories";

    public ClientCategoryService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<List<CategoryDto>> GetCategories()
        => await httpClient.GetFromJsonAsync<List<CategoryDto>>(baseUrl)
            ?? new List<CategoryDto>();

    public async Task<CategoryDto> GetCategory(Guid categoryId)
        => await httpClient.GetFromJsonAsync<CategoryDto>($"{baseUrl}/{categoryId}")
            ?? throw new KeyNotFoundException();

    public async Task<CategoryDto> PutCategory(CategoryDto categoryDto)
    {
        var response = await httpClient.PutAsJsonAsync($"{baseUrl}/{categoryDto.Id}", categoryDto);
        response.EnsureSuccessStatusCode(); 
        return await response.Content.ReadFromJsonAsync<CategoryDto>()
            ?? throw new InvalidOperationException("Failed to deserialize the response.");
    }

    public async Task DeleteCategory(Guid categoryId)
    {
        var response = await httpClient.DeleteAsync($"{baseUrl}/{categoryId}");
        
        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.NotFound:
                throw new KeyNotFoundException("Category not found.");
            case System.Net.HttpStatusCode.BadRequest:
                throw new ReferencedByEntitiesException(response.ReasonPhrase!);
            
            default:
                return;
        }
    }
}