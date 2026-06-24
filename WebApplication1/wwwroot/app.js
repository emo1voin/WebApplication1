const API_URL = 'https://localhost:7147/api';
let currentOrderId = null;
let allOrders = [];
let allProducts = [];
let allClients = [];
let allPickupPoints = [];

// Initialize application
document.addEventListener('DOMContentLoaded', () => {
    loadClients();
    loadPickupPoints();
    loadProducts();
    loadOrders();
    
    document.getElementById('orderForm').addEventListener('submit', handleCreateOrder);
    
    // Add first order item field by default
    setTimeout(() => addOrderItem(), 500);
});

// Tab navigation
function showTab(tabName) {
    const tabs = document.querySelectorAll('.tab-content');
    tabs.forEach(tab => tab.style.display = 'none');
    document.getElementById(tabName).style.display = 'block';
    
    if (tabName === 'orders') {
        loadOrders();
    }
}

// Load products from API
async function loadProducts() {
    try {
        const response = await fetch(`${API_URL}/products`);
        allProducts = await response.json();
        updateProductSelects();
    } catch (error) {
        console.error('Error loading products:', error);
        showNotification('Ошибка при загрузке товаров', 'error');
    }
}

// Load clients from API
async function loadClients() {
    try {
        const response = await fetch(`${API_URL}/clients`);
        allClients = await response.json();
        const select = document.getElementById('userId');
        select.innerHTML = '<option value="">-- Выберите клиента --</option>';
        allClients.forEach(client => {
            const option = document.createElement('option');
            option.value = client.id;
            option.textContent = `${client.name} (${client.email})`;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Error loading clients:', error);
        showNotification('Ошибка при загрузке клиентов', 'error');
    }
}

// Load pickup points from API
async function loadPickupPoints() {
    try {
        const response = await fetch(`${API_URL}/pickup-points`);
        allPickupPoints = await response.json();
        const select = document.getElementById('pickupPointId');
        select.innerHTML = '<option value="">-- Выберите пункт выдачи --</option>';
        allPickupPoints.forEach(point => {
            const option = document.createElement('option');
            option.value = point.id;
            option.textContent = `${point.name} (${point.address})`;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Error loading pickup points:', error);
        showNotification('Ошибка при загрузке пунктов выдачи', 'error');
    }
}

// Load orders from API
async function loadOrders() {
    try {
        const status = document.getElementById('statusFilter')?.value || '';
        const url = status ? `${API_URL}/orders?status=${status}` : `${API_URL}/orders`;
        const response = await fetch(url);
        allOrders = await response.json();
        displayOrders();
    } catch (error) {
        console.error('Error loading orders:', error);
        showNotification('Ошибка при загрузке заказов', 'error');
    }
}

// Display orders
function displayOrders() {
    const ordersList = document.getElementById('ordersList');
    
    if (allOrders.length === 0) {
        ordersList.innerHTML = '<p style="text-align: center; color: #999; padding: 40px;">Нет заказов</p>';
        return;
    }
    
    ordersList.innerHTML = allOrders.map(order => `
        <div class="order-card" onclick="showOrderDetails(${order.id})">
            <div class="order-header">
                <div class="order-id">Заказ #${order.id}</div>
                <div class="order-status status-${order.status.toLowerCase()}">${getStatusText(order.status)}</div>
            </div>
            <div class="order-info">
                <div class="info-item">
                    <strong>Клиент:</strong>
                    ${order.user?.name || 'N/A'}
                </div>
                <div class="info-item">
                    <strong>Пункт выдачи:</strong>
                    ${order.pickupPoint?.name || 'N/A'}
                </div>
                <div class="info-item">
                    <strong>Дата заказа:</strong>
                    ${new Date(order.orderDate).toLocaleDateString('ru-RU')}
                </div>
                <div class="info-item">
                    <strong>Дата доставки:</strong>
                    ${order.deliveryDate ? new Date(order.deliveryDate).toLocaleDateString('ru-RU') : 'Не указана'}
                </div>
            </div>
            <div class="order-items-preview">
                <strong>Товары:</strong> ${order.orderItems?.map(item => `${item.product?.name} x${item.quantity}`).join(', ') || 'N/A'}
            </div>
            <div class="order-total">Итого: ${order.totalAmount.toFixed(2)} ₽</div>
        </div>
    `).join('');
}

// Filter orders by status
function filterOrders() {
    loadOrders();
}

// Show order details in modal
async function showOrderDetails(orderId) {
    currentOrderId = orderId;
    const order = allOrders.find(o => o.id === orderId);
    
    if (!order) return;
    
    const detailsHtml = `
        <div class="order-detail-item">
            <strong>ID заказа:</strong>
            ${order.id}
        </div>
        <div class="order-detail-item">
            <strong>Клиент:</strong>
            ${order.user?.name} (${order.user?.email})
        </div>
        <div class="order-detail-item">
            <strong>Пункт выдачи:</strong>
            ${order.pickupPoint?.name}<br/>
            ${order.pickupPoint?.address}
        </div>
        <div class="order-detail-item">
            <strong>Статус:</strong>
            ${getStatusText(order.status)}
        </div>
        <div class="order-detail-item">
            <strong>Дата заказа:</strong>
            ${new Date(order.orderDate).toLocaleDateString('ru-RU', {
                year: 'numeric',
                month: 'long',
                day: 'numeric',
                hour: '2-digit',
                minute: '2-digit'
            })}
        </div>
        <div class="order-detail-item">
            <strong>Дата доставки:</strong>
            ${order.deliveryDate ? new Date(order.deliveryDate).toLocaleDateString('ru-RU') : 'Не указана'}
        </div>
        <div class="order-detail-item">
            <strong>Товары в заказе:</strong>
            <ul style="margin-top: 10px; margin-left: 20px;">
                ${order.orderItems?.map(item => `
                    <li>${item.product?.name} - ${item.quantity} шт. x ${item.unitPrice} ₽ = ${item.subtotal} ₽</li>
                `).join('') || '<li>Нет товаров</li>'}
            </ul>
        </div>
        <div class="order-detail-item" style="border: none;">
            <strong>Итого:</strong> ${order.totalAmount.toFixed(2)} ₽
        </div>
    `;
    
    document.getElementById('orderDetails').innerHTML = detailsHtml;
    document.getElementById('orderModal').style.display = 'block';
}

// Close modal
function closeOrderModal() {
    document.getElementById('orderModal').style.display = 'none';
    currentOrderId = null;
}

// Update order status
async function updateOrderStatus() {
    const newStatus = document.getElementById('newStatus').value;
    
    if (!newStatus || !currentOrderId) {
        showNotification('Выберите новый статус', 'error');
        return;
    }
    
    try {
        const response = await fetch(`${API_URL}/orders/${currentOrderId}/status`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ status: newStatus })
        });
        
        if (response.ok) {
            showNotification('Статус заказа обновлен', 'success');
            closeOrderModal();
            loadOrders();
        } else {
            showNotification('Ошибка при обновлении статуса', 'error');
        }
    } catch (error) {
        console.error('Error updating status:', error);
        showNotification('Ошибка при обновлении статуса', 'error');
    }
}

// Cancel order
async function cancelOrder() {
    if (!currentOrderId || !confirm('Вы уверены, что хотите отменить этот заказ?')) {
        return;
    }
    
    try {
        const response = await fetch(`${API_URL}/orders/${currentOrderId}`, {
            method: 'DELETE'
        });
        
        if (response.ok) {
            showNotification('Заказ отменен', 'success');
            closeOrderModal();
            loadOrders();
        } else {
            showNotification('Ошибка при отмене заказа', 'error');
        }
    } catch (error) {
        console.error('Error canceling order:', error);
        showNotification('Ошибка при отмене заказа', 'error');
    }
}

// Add order item row
function addOrderItem() {
    const container = document.getElementById('orderItems');
    const itemIndex = container.children.length;
    
    const html = `
        <div class="order-item">
            <select class="product-select" onchange="updateProductPrice(${itemIndex})">
                <option value="">-- Выберите товар --</option>
                ${allProducts.map(p => `<option value="${p.id}" data-price="${p.price}">${p.name} (${p.price} ₽)</option>`).join('')}
            </select>
            <input type="number" class="quantity-input" placeholder="Количество" value="1" min="1" onchange="updateItemSubtotal(${itemIndex})">
            <input type="number" class="price-input" placeholder="Цена" readonly>
            <button type="button" class="btn-remove" onclick="removeOrderItem(${itemIndex})">Удалить</button>
        </div>
    `;
    
    const div = document.createElement('div');
    div.innerHTML = html;
    container.appendChild(div.firstElementChild);
}

// Remove order item
function removeOrderItem(index) {
    const items = document.querySelectorAll('.order-item');
    if (items.length > 1) {
        items[index].remove();
    } else {
        showNotification('Заказ должен содержать хотя бы один товар', 'error');
    }
}

// Update product price
function updateProductPrice(index) {
    const items = document.querySelectorAll('.order-item');
    const priceInput = items[index].querySelector('.price-input');
    const select = items[index].querySelector('.product-select');
    const selectedOption = select.options[select.selectedIndex];
    const price = selectedOption.dataset.price || 0;
    priceInput.value = price;
    updateItemSubtotal(index);
}

// Update item subtotal
function updateItemSubtotal(index) {
    const items = document.querySelectorAll('.order-item');
    const priceInput = items[index].querySelector('.price-input');
    const quantityInput = items[index].querySelector('.quantity-input');
    // Subtotal calculation is just for reference, actual calculation happens on backend
}

// Update product selects
function updateProductSelects() {
    const items = document.querySelectorAll('.product-select');
    const optionsHtml = `
        <option value="">-- Выберите товар --</option>
        ${allProducts.map(p => `<option value="${p.id}" data-price="${p.price}">${p.name} (${p.price} ₽)</option>`).join('')}
    `;
    items.forEach(select => {
        const currentValue = select.value;
        select.innerHTML = optionsHtml;
        select.value = currentValue;
    });
}

// Handle create order form submission
async function handleCreateOrder(e) {
    e.preventDefault();
    
    const userId = document.getElementById('userId').value;
    const pickupPointId = document.getElementById('pickupPointId').value;
    const deliveryDate = document.getElementById('deliveryDate').value;
    
    if (!userId || !pickupPointId) {
        showNotification('Заполните все поля', 'error');
        return;
    }
    
    const items = [];
    document.querySelectorAll('.order-item').forEach(item => {
        const productId = item.querySelector('.product-select').value;
        const quantity = parseInt(item.querySelector('.quantity-input').value);
        
        if (productId && quantity > 0) {
            items.push({
                productId: parseInt(productId),
                quantity: quantity
            });
        }
    });
    
    if (items.length === 0) {
        showNotification('Добавьте хотя бы один товар', 'error');
        return;
    }
    
    try {
        const response = await fetch(`${API_URL}/orders`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userId: parseInt(userId),
                pickupPointId: parseInt(pickupPointId),
                deliveryDate: deliveryDate || null,
                items: items
            })
        });
        
        if (response.ok) {
            showNotification('Заказ создан успешно', 'success');
            resetOrderForm();
            setTimeout(() => {
                showTab('orders');
            }, 1000);
        } else {
            const error = await response.text();
            showNotification(`Ошибка: ${error}`, 'error');
        }
    } catch (error) {
        console.error('Error creating order:', error);
        showNotification('Ошибка при создании заказа', 'error');
    }
}

// Reset order form
function resetOrderForm() {
    document.getElementById('orderForm').reset();
    const container = document.getElementById('orderItems');
    container.innerHTML = '';
    addOrderItem();
}

// Get status text translation
function getStatusText(status) {
    const statusMap = {
        'Pending': 'В ожидании',
        'Processing': 'Обработка',
        'Ready': 'Готово',
        'Completed': 'Завершено',
        'Cancelled': 'Отменено'
    };
    return statusMap[status] || status;
}

// Show notification
function showNotification(message, type = 'info') {
    const div = document.createElement('div');
    div.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 15px 20px;
        border-radius: 5px;
        color: white;
        font-weight: bold;
        z-index: 9999;
        animation: slideIn 0.3s ease-in-out;
    `;
    
    if (type === 'success') {
        div.style.background = '#28a745';
    } else if (type === 'error') {
        div.style.background = '#dc3545';
    } else {
        div.style.background = '#17a2b8';
    }
    
    div.textContent = message;
    document.body.appendChild(div);
    
    setTimeout(() => {
        div.style.animation = 'slideOut 0.3s ease-in-out';
        setTimeout(() => div.remove(), 300);
    }, 3000);
}

// Close modal when clicking outside
window.onclick = function(event) {
    const modal = document.getElementById('orderModal');
    if (event.target == modal) {
        closeOrderModal();
    }
}

// Add animation styles
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(400px);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(400px);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);
