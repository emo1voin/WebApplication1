# Быстрый старт 🚀

## За 5 минут до запуска приложения

### Шаг 1: Подготовка БД (2 минуты)

```bash
# 1. Откройте DBforge for MySQL
# 2. Подключитесь к серверу:
#    Host: 192.168.100.35
#    User: user01
#    Password: 81328
# 3. Откройте файл database_schema.sql
# 4. Нажмите Execute (Ctrl+E)
# 5. Готово! ✓
```

### Шаг 2: Запуск приложения (2 минуты)

```bash
# Откройте командную строку в папке проекта WebApplication1
cd C:\Users\andre\source\repos\WebApplication1\WebApplication1

# Восстановите зависимости
dotnet restore

# Запустите приложение
dotnet run
```

### Шаг 3: Проверка работоспособности (1 минута)

```
✓ API запустилось на: https://localhost:7147
✓ Swagger UI: https://localhost:7147/swagger
✓ Web Interface: https://localhost:7147
```

---

## Что дальше?

### 1. Протестировать API в Swagger

1. Откройте https://localhost:7147/swagger в браузере
2. Попробуйте endpoints:
   - `GET /api/products` - получить товары
   - `GET /api/clients` - получить клиентов
   - `POST /api/orders` - создать заказ

### 2. Использовать Web Interface

1. Откройте https://localhost:7147 в браузере
2. Перейдите на вкладку "Создать заказ"
3. Выберите клиента, пункт выдачи, добавьте товары
4. Нажмите "Создать заказ"

### 3. Протестировать API через cURL

```bash
# Получить все товары
curl -X GET "https://localhost:7147/api/products" -k

# Получить всех клиентов
curl -X GET "https://localhost:7147/api/clients" -k

# Получить все заказы
curl -X GET "https://localhost:7147/api/orders" -k
```

---

## Структура файлов

```
WebApplication1/
├── Controllers/           # REST API endpoints
├── Models/               # Модели данных (Order, Product, User, etc.)
├── Data/                 # DbContext для Entity Framework
├── wwwroot/              # Web Interface (HTML, CSS, JS)
├── Program.cs            # Конфигурация приложения
├── appsettings.json      # Параметры подключения к БД
└── WebApplication1.csproj # Зависимости проекта
```

---

## Основные функции

### Backend API
- ✅ POST /api/orders - создание заказа
- ✅ GET /api/orders - получение всех заказов
- ✅ GET /api/orders/{id} - получение заказа по ID
- ✅ PATCH /api/orders/{id}/status - изменение статуса
- ✅ DELETE /api/orders/{id} - отмена заказа
- ✅ GET /api/products - список товаров
- ✅ GET /api/clients - список клиентов
- ✅ GET /api/pickup-points - пункты выдачи

### Frontend
- ✅ Просмотр всех заказов с фильтрацией по статусу
- ✅ Создание новых заказов
- ✅ Просмотр деталей заказа
- ✅ Изменение статуса заказа
- ✅ Отмена заказа

---

## Проверка подключения к БД

Если приложение не стартует, проверьте подключение:

```bash
# В папке проекта выполните
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=192.168.100.35;Port=3306;Database=orders_db;Uid=user01;Pwd=81328;"
```

---

## Примеры использования API

### Создать заказ
```bash
curl -X POST "https://localhost:7147/api/orders" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userId": 1,
    "pickupPointId": 1,
    "items": [{"productId": 1, "quantity": 1}]
  }'
```

### Получить заказ
```bash
curl -X GET "https://localhost:7147/api/orders/1" -k
```

### Изменить статус
```bash
curl -X PATCH "https://localhost:7147/api/orders/1/status" \
  -H "Content-Type: application/json" \
  -k \
  -d '{"status": "Processing"}'
```

---

## Для детальной информации

📖 Полная документация:
- `README.md` - полное описание проекта
- `REST_API_DOCUMENTATION.md` - документация всех endpoints
- `API_EXAMPLES.md` - примеры всех запросов
- `DBFORGE_IMPORT_GUIDE.md` - руководство по импорту БД

---

## Что-то не работает?

### Приложение не запускается
```bash
# Очистите проект
dotnet clean

# Восстановите зависимости
dotnet restore

# Пересоберите
dotnet build

# Запустите снова
dotnet run
```

### Ошибка подключения к БД
- Проверьте, что MySQL сервер запущен на 192.168.100.35:3306
- Проверьте учетные данные user01:81328
- Проверьте, что БД orders_db создана

### HTTPS ошибка "SEC_ERROR_UNKNOWN_ISSUER"
- Нажмите "Advanced"
- Нажмите "Accept the Risk and Continue"
- Это нормально для self-signed сертификата локального сервера

---

**Готово!** 🎉 Приложение полностью настроено и готово к использованию.

Если возникли вопросы, обратитесь к документации или проверьте логи приложения.
