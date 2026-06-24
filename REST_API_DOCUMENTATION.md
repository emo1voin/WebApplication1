# REST API для управления заказами

## Настройка

### Требования
- .NET 8.0 SDK
- MySQL Server 8.0+
- DBforge for MySQL

### Строка подключения
```
Server=192.168.100.35;Port=3306;Database=orders_db;Uid=user01;Pwd=81328;
```

### Создание БД
1. Откройте DBforge for MySQL
2. Подключитесь к серверу 192.168.100.35 с учетными данными user01 / 81328
3. Выполните скрипт из файла `database_schema.sql`

## API Endpoints

### 1. Заказы (Orders)

#### POST /api/orders - Создание нового заказа
**Описание:** Создает новый заказ с товарами

**Request:**
```json
{
  "userId": 1,
  "pickupPointId": 1,
  "deliveryDate": "2024-12-25",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 3,
      "quantity": 1
    }
  ]
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "userId": 1,
  "pickupPointId": 1,
  "orderDate": "2024-12-15T10:30:00Z",
  "deliveryDate": "2024-12-25T00:00:00Z",
  "status": "Pending",
  "totalAmount": 105997.00,
  "createdAt": "2024-12-15T10:30:00Z",
  "updatedAt": "2024-12-15T10:30:00Z",
  "user": {
    "id": 1,
    "name": "Иван Петров",
    "email": "ivan@example.com",
    "phone": "+79991234567",
    "address": "ул. Ленина, д. 10"
  },
  "pickupPoint": {
    "id": 1,
    "name": "Пункт выдачи #1",
    "address": "ул. Ленина, д. 50",
    "phone": "+78001234567",
    "workingHours": "09:00-21:00"
  },
  "orderItems": [
    {
      "id": 1,
      "orderId": 1,
      "productId": 1,
      "quantity": 2,
      "unitPrice": 89999.00,
      "subtotal": 179998.00,
      "product": {
        "id": 1,
        "name": "Ноутбук Dell",
        "description": "Ноутбук Dell XPS 13, процессор Intel i7",
        "price": 89999.00,
        "stock": 3
      }
    }
  ]
}
```

#### GET /api/orders - Получение всех заказов
**Описание:** Получает список всех заказов с опциональной фильтрацией по статусу

**Query Parameters:**
- `status` (optional): Pending, Processing, Ready, Completed, Cancelled

**Example:** `GET /api/orders?status=Pending`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "userId": 1,
    "status": "Pending",
    "totalAmount": 105997.00,
    "user": { ... },
    "pickupPoint": { ... },
    "orderItems": [ ... ]
  }
]
```

#### GET /api/orders/{id} - Получение деталей заказа
**Описание:** Получает полную информацию о конкретном заказе

**Path Parameters:**
- `id` (required): ID заказа

**Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "pickupPointId": 1,
  "orderDate": "2024-12-15T10:30:00Z",
  "deliveryDate": "2024-12-25T00:00:00Z",
  "status": "Pending",
  "totalAmount": 105997.00,
  "user": { ... },
  "pickupPoint": { ... },
  "orderItems": [ ... ]
}
```

#### PATCH /api/orders/{id}/status - Изменение статуса заказа
**Описание:** Изменяет статус существующего заказа

**Path Parameters:**
- `id` (required): ID заказа

**Request:**
```json
{
  "status": "Processing"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "status": "Processing",
  "updatedAt": "2024-12-15T11:45:00Z",
  ...
}
```

#### DELETE /api/orders/{id} - Отмена заказа
**Описание:** Отменяет заказ и возвращает товары на склад

**Path Parameters:**
- `id` (required): ID заказа

**Response (200 OK):**
```json
{
  "message": "Order cancelled successfully"
}
```

### 2. Товары (Products)

#### GET /api/products - Получение списка товаров
**Описание:** Получает список всех доступных товаров

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Ноутбук Dell",
    "description": "Ноутбук Dell XPS 13, процессор Intel i7",
    "price": 89999.00,
    "stock": 3,
    "createdAt": "2024-12-01T00:00:00Z"
  },
  {
    "id": 2,
    "name": "Клавиатура механическая",
    "description": "Механическая клавиатура с RGB подсветкой",
    "price": 5999.00,
    "stock": 15,
    "createdAt": "2024-12-01T00:00:00Z"
  }
]
```

### 3. Клиенты (Clients)

#### GET /api/clients - Получение списка клиентов
**Описание:** Получает список всех клиентов для выбора при создании заказа

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Иван Петров",
    "email": "ivan@example.com",
    "phone": "+79991234567",
    "address": "ул. Ленина, д. 10",
    "createdAt": "2024-12-01T00:00:00Z"
  }
]
```

### 4. Пункты выдачи (Pickup Points)

#### GET /api/pickup-points - Получение списка пунктов выдачи
**Описание:** Получает список всех пунктов выдачи

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Пункт выдачи #1",
    "address": "ул. Ленина, д. 50",
    "phone": "+78001234567",
    "workingHours": "09:00-21:00",
    "createdAt": "2024-12-01T00:00:00Z"
  }
]
```

## Коды ошибок

| Статус | Описание |
|--------|---------|
| 200 OK | Успешный запрос |
| 201 Created | Ресурс создан успешно |
| 400 Bad Request | Некорректные данные в запросе |
| 404 Not Found | Ресурс не найден |
| 500 Internal Server Error | Ошибка сервера |

## Примеры использования

### Создание заказа с помощью cURL

```bash
curl -X POST https://localhost:7147/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "pickupPointId": 1,
    "deliveryDate": "2024-12-25",
    "items": [
      {
        "productId": 1,
        "quantity": 1
      }
    ]
  }'
```

### Получение заказов со статусом "Processing"

```bash
curl -X GET "https://localhost:7147/api/orders?status=Processing"
```

### Изменение статуса заказа

```bash
curl -X PATCH https://localhost:7147/api/orders/1/status \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Completed"
  }'
```

## Примечания

- JWT-токены не требуются. Авторизация не реализована.
- Все даты возвращаются в формате ISO 8601 (UTC)
- Валюта: рубли (₽)
- При создании заказа автоматически уменьшается количество товара на складе
- При отмене заказа (если он не завершен) товары возвращаются на склад
