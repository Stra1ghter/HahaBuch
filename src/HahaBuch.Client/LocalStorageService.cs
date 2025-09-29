using Microsoft.JSInterop;
using System.Text.Json;

namespace HahaBuch.Client
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime js;

        public LocalStorageService(IJSRuntime js)
        {
            this.js = js;
        }

        public ValueTask RemoveItemAsync(string key) =>
            js.InvokeVoidAsync("localStorageInterop.removeItem", key);

        public async ValueTask SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await SetItemAsync(key, json);
        }

        public async ValueTask<T?> GetItemAsync<T>(string key)
        {
            string? json = await GetItemAsync(key);
            if (json is null)
                return default;
            return JsonSerializer.Deserialize<T>(json);
        }

        private ValueTask SetItemAsync(string key, string value) =>
            js.InvokeVoidAsync("localStorageInterop.setItem", key, value);

        private ValueTask<string?> GetItemAsync(string key) =>
            js.InvokeAsync<string?>("localStorageInterop.getItem", key);
    }
}