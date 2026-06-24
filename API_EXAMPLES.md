# Примеры запросов к REST API

Этот файл содержит готовые примеры запросов для тестирования всех endpoints API.

## Базовая информация

- **Base URL:** `https://localhost:7147/api`
- **Content-Type:** `application/json`
- **Примечание:** Используйте [Postman](https://www.postman.com/) или [curl](https://curl.se/) для тестирования

## 1. Получение товаров

### Получить список всех товаров

```bash
curl -X GET "https://localhost:7147/api/products" \
  -H "Content-Type: application/json" \
  -k
```

**Response:**
```json
[
  {
    "id": 1,
    "name": "Ноутбук Dell",
    "description": "Ноутбук Dell XPS 13, процессор Intel i7",
    "price": 89999.00,
    "stock": 5,
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 2,
    "name": "Клавиатура механическая",
    "description": "Механическая клавиатура с RGB подсветкой",
    "price": 5999.00,
    "stock": 15,
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 3,
    "name": "Мышь беспроводная",
    "description": "Беспроводная мышь Logitech MX Master 3",
    "price": 7999.00,
    "stock": 20,
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 4,
    "name": "Монитор 27\"",
    "description": "IPS монитор 2560x1440",
    "price": 24999.00,
    "stock": 8,
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 5,
    "name": "Наушники",
    "description": "Наушники Sony WH-1000XM5",
    "price": 29999.00,
    "stock": 12,
    "createdAt": "2024-12-15T00:00:00"
  }
]
```

## 2. Получение клиентов

### Получить список всех клиентов

```bash
curl -X GET "https://localhost:7147/api/clients" \
  -H "Content-Type: application/json" \
  -k
```

**Response:**
```json
[
  {
    "id": 1,
    "name": "Иван Петров",
    "email": "ivan@example.com",
    "phone": "+79991234567",
    "address": "ул. Ленина, д. 10",
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 2,
    "name": "Мария Сидорова",
    "email": "maria@example.com",
    "phone": "+79992345678",
    "address": "ул. Пушкина, д. 20",
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 3,
    "name": "Петр Иванов",
    "email": "petr@example.com",
    "phone": "+79993456789",
    "address": "ул. Толстого, д. 30",
    "createdAt": "2024-12-15T00:00:00"
  }
]
```

## 3. Получение пунктов выдачи

### Получить список пунктов выдачи

```bash
curl -X GET "https://localhost:7147/api/pickup-points" \
  -H "Content-Type: application/json" \
  -k
```

**Response:**
```json
[
  {
    "id": 1,
    "name": "Пункт выдачи #1",
    "address": "ул. Ленина, д. 50",
    "phone": "+78001234567",
    "workingHours": "09:00-21:00",
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 2,
    "name": "Пункт выдачи #2",
    "address": "ул. Октябрьская, д. 100",
    "phone": "+78001234568",
    "workingHours": "10:00-20:00",
    "createdAt": "2024-12-15T00:00:00"
  },
  {
    "id": 3,
    "name": "Пункт выдачи #3",
    "address": "ул. Советская, д. 75",
    "phone": "+78001234569",
    "workingHours": "08:00-22:00",
    "createdAt": "2024-12-15T00:00:00"
  }
]
```

## 4. Управление заказами

### Создать новый заказ

```bash
curl -X POST "https://localhost:7147/api/orders" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userId": 1,
    "pickupPointId": 1,
    "deliveryDate": "2024-12-25T00:00:00",
    "items": [
      {
        "productId": 1,
        "quantity": 1
      },
      {
        "productId": 2,
        "quantity": 2
      }
    ]
  }'
```

**Response (201 Created):**
```json
{
  "id": 1,
  "userId": 1,
  "pickupPointId": 1,
  "orderDate": "2024-12-15T12:30:00",
  "deliveryDate": "2024-12-25T00:00:00",
  "status": "Pending",
  "totalAmount": 101997.00,
  "createdAt": "2024-12-15T12:30:00",
  "updatedAt": "2024-12-15T12:30:00",
  "user": {
    "id": 1,
    "name": "Иван Петров",
    "email": "ivan@example.com",
    "phone": "+79991234567",
    "address": "ул. Ленина, д. 10",
    "createdAt": "2024-12-15T00:00:00"
  },
  "pickupPoint": {
    "id": 1,
    "name": "Пункт выдачи #1",
    "address": "ул. Ленина, д. 50",
    "phone": "+78001234567",
    "workingHours": "09:00-21:00",
    "createdAt": "2024-12-15T00:00:00"
  },
  "orderItems": [
    {
      "id": 1,
      "orderId": 1,
      "productId": 1,
      "quantity": 1,
      "unitPrice": 89999.00,
      "subtotal": 89999.00,
      "createdAt": "2024-12-15T12:30:00",
      "product": {
        "id": 1,
        "name": "Ноутбук Dell",
        "description": "Ноутбук Dell XPS 13, процессор Intel i7",
        "price": 89999.00,
        "stock": 4,
        "createdAt": "2024-12-15T00:00:00"
      }
    },
    {
      "id": 2,
      "orderId": 1,
      "productId": 2,
      "quantity": 2,
      "unitPrice": 5999.00,
      "subtotal": 11998.00,
      "createdAt": "2024-12-15T12:30:00",
      "product": {
        "id": 2,
        "name": "Клавиатура механическая",
        "description": "Механическая клавиатура с RGB подсветкой",
        "price": 5999.00,
        "stock": 13,
        "createdAt": "2024-12-15T00:00:00"
      }
    }
  ]
}
```

### Получить все заказы

```bash
curl -X GET "https://localhost:7147/api/orders" \
  -H "Content-Type: application/json" \
  -k
```

### Получить заказы со статусом "Processing"

```bash
curl -X GET "https://localhost:7147/api/orders?status=Processing" \
  -H "Content-Type: application/json" \
  -k
```

### Получить конкретный заказ по ID

```bash
curl -X GET "https://localhost:7147/api/orders/1" \
  -H "Content-Type: application/json" \
  -k
```

**Response:**
```json
{
  "id": 1,
  "userId": 1,
  "pickupPointId": 1,
  "orderDate": "2024-12-15T12:30:00",
  "deliveryDate": "2024-12-25T00:00:00",
  "status": "Pending",
  "totalAmount": 101997.00,
  "createdAt": "2024-12-15T12:30:00",
  "updatedAt": "2024-12-15T12:30:00",
  "user": { ... },
  "pickupPoint": { ... },
  "orderItems": [ ... ]
}
```

### Изменить статус заказа

```bash
curl -X PATCH "https://localhost:7147/api/orders/1/status" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "status": "Processing"
  }'
```

**Response:**
```json
{
  "id": 1,
  "status": "Processing",
  "updatedAt": "2024-12-15T12:35:00",
  ...
}
```

### Варианты статусов заказа

- `Pending` - В ожидании
- `Processing` - Обработка
- `Ready` - Готово
- `Completed` - Завершено
- `Cancelled` - Отменено

Пример изменения на "Ready":
```bash
curl -X PATCH "https://localhost:7147/api/orders/1/status" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "status": "Ready"
  }'
```

### Отменить заказ

```bash
curl -X DELETE "https://localhost:7147/api/orders/1" \
  -H "Content-Type: application/json" \
  -k
```

**Response:**
```json
"Order cancelled successfully"
```

## 5. Примеры с дополнительными комбинациями

### Создать заказ с множественными товарами

```bash
curl -X POST "https://localhost:7147/api/orders" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userId": 2,
    "pickupPointId": 2,
    "deliveryDate": "2024-12-30",
    "items": [
      {
        "productId": 3,
        "quantity": 1
      },
      {
        "productId": 4,
        "quantity": 1
      },
      {
        "productId": 5,
        "quantity": 2
      }
    ]
  }'
```

### Создать заказ без указания даты доставки

```bash
curl -X POST "https://localhost:7147/api/orders" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userId": 3,
    "pickupPointId": 3,
    "items": [
      {
        "productId": 2,
        "quantity": 3
      }
    ]
  }'
```

## Использование в Postman

1. Откройте Postman
2. Создайте новую коллекцию "Orders API"
3. Для каждого примера создайте новый Request
4. Установите метод (GET, POST, PATCH, DELETE)
5. Вставьте URL и тело запроса
6. Нажмите Send

### Настройка переменных в Postman

Создайте переменную для упрощения:

```
Variables:
- base_url = https://localhost:7147/api
```

Используйте в запросах:
```
{{base_url}}/orders
{{base_url}}/products
{{base_url}}/clients
```

## Тестирование через PowerShell

```powershell
# Установить cert validation skip для https://localhost
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

# Получить товары
$response = Invoke-RestMethod -Uri "https://localhost:7147/api/products" -Method Get
$response | ConvertTo-Json | Out-Host

# Создать заказ
$body = @{
    userId = 1
    pickupPointId = 1
    deliveryDate = "2024-12-25"
    items = @(
        @{productId = 1; quantity = 1}
    )
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:7147/api/orders" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body

$response | ConvertTo-Json | Out-Host
```

## Возможные ошибки

### 400 Bad Request

Причины:
- Неправильно указан ID товара/клиента
- Недостаточно товара на складе
- Пропущены обязательные поля

### 404 Not Found

Причины:
- Заказ/товар/клиент не существует
- Неправильный ID

### 500 Internal Server Error

Причины:
- Ошибка БД
- Неправильная конфигурация

---

Используйте эти примеры для полного тестирования API перед развертыванием в production!
