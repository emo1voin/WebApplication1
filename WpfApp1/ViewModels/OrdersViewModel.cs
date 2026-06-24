using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModels;

public partial class OrdersViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<OrderModel> orders = new();

    [ObservableProperty]
    private OrderModel? selectedOrder;

    [ObservableProperty]
    private string statusFilter = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    public OrdersViewModel()
    {
        _apiService = new ApiService();
    }

    [RelayCommand]
    public async Task LoadOrders()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var orders = await _apiService.GetOrdersAsync(
                string.IsNullOrEmpty(StatusFilter) ? null : StatusFilter
            );

            Orders = new ObservableCollection<OrderModel>(orders);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task UpdateOrderStatus(string? newStatus)
    {
        if (SelectedOrder == null || string.IsNullOrEmpty(newStatus))
        {
            ErrorMessage = "Выберите заказ и статус";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            await _apiService.UpdateOrderStatusAsync(SelectedOrder.Id, newStatus);
            SelectedOrder.Status = newStatus;
            SelectedOrder = null;

            SuccessMessage = "Статус заказа обновлен!";
            await LoadOrdersCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task CancelOrder()
    {
        if (SelectedOrder == null)
        {
            ErrorMessage = "Выберите заказ для отмены";
            return;
        }

        if (System.Windows.MessageBox.Show(
            $"Вы уверены что хотите отменить заказ #{SelectedOrder.Id}?",
            "Подтверждение",
            System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
        {
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            await _apiService.CancelOrderAsync(SelectedOrder.Id);

            SuccessMessage = "Заказ отменен!";
            SelectedOrder = null;
            await LoadOrdersCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task FilterByStatus()
    {
        await LoadOrdersCommand.ExecuteAsync(null);
    }
}
