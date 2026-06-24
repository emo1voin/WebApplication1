using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using WpfApp1.Models;

namespace WpfApp1.Services;

public interface IApiService
{
    Task<List<OrderModel>> GetOrdersAsync(string? status = null);
    Task<OrderModel?> GetOrderAsync(int id);
    Task<OrderModel?> CreateOrderAsync(CreateOrderRequest request);
    Task<bool> UpdateOrderStatusAsync(int id, string status);
    Task<bool> CancelOrderAsync(int id);
    Task<List<ProductModel>> GetProductsAsync();
    Task<List<UserModel>> GetClientsAsync();
    Task<List<PickupPointModel>> GetPickupPointsAsync();
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:7167/api";

    public ApiService()
    {
        // Игнорировать SSL сертификаты для localhost
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

        _httpClient = new HttpClient(handler);
    }

    private JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    public async Task<List<OrderModel>> GetOrdersAsync(string? status = null)
    {
        try
        {
            string url = $"{BaseUrl}/orders";
            if (!string.IsNullOrEmpty(status))
                url += $"?status={status}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<OrderModel>>(json, GetJsonOptions()) ?? new List<OrderModel>();
            return orders;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке заказов: {ex.Message}", ex);
        }
    }

    public async Task<OrderModel?> GetOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/orders/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OrderModel>(json, GetJsonOptions());
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке заказа: {ex.Message}", ex);
        }
    }

    public async Task<OrderModel?> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/orders",
                request,
                GetJsonOptions()
            );
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OrderModel>(json, GetJsonOptions());
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при создании заказа: {ex.Message}", ex);
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int id, string status)
    {
        try
        {
            var request = new { status };
            var response = await _httpClient.PatchAsJsonAsync(
                $"{BaseUrl}/orders/{id}/status",
                request,
                GetJsonOptions()
            );
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обновлении статуса: {ex.Message}", ex);
        }
    }

    public async Task<bool> CancelOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/orders/{id}");
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при отмене заказа: {ex.Message}", ex);
        }
    }

    public async Task<List<ProductModel>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductModel>>(json, GetJsonOptions()) ?? new List<ProductModel>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке товаров: {ex.Message}", ex);
        }
    }

    public async Task<List<UserModel>> GetClientsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/clients");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserModel>>(json, GetJsonOptions()) ?? new List<UserModel>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке клиентов: {ex.Message}", ex);
        }
    }

    public async Task<List<PickupPointModel>> GetPickupPointsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/pickup-points");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PickupPointModel>>(json, GetJsonOptions()) ?? new List<PickupPointModel>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке пунктов выдачи: {ex.Message}", ex);
        }
    }
}
