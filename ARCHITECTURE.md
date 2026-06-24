# Архитектура системы управления заказами

## 📐 Общая архитектура

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         WEB BROWSER (Frontend)                           │
│  ┌────────────────────────────────────────────────────────────────────┐ │
│  │                    https://localhost:7147                          │ │
│  │  ┌──────────────────┐           ┌──────────────────────────────┐  │ │
│  │  │   index.html     │           │   REST API Requests (JSON)   │  │ │
│  │  │  ┌────────────┐  │           │   • POST /api/orders        │  │ │
│  │  │  │  app.js    │  │◄─────────►│   • GET /api/orders         │  │ │
│  │  │  │  (890 ln)  │  │           │   • PATCH /api/orders/{id}  │  │ │
│  │  │  └────────────┘  │           │   • DELETE /api/orders/{id} │  │ │
│  │  │  ┌────────────┐  │           │   • GET /api/products       │  │ │
│  │  │  │ style.css  │  │           │   • GET /api/clients        │  │ │
│  │  │  │ (650 ln)   │  │           │   • GET /api/pickup-points  │  │ │
│  │  │  └────────────┘  │           └──────────────────────────────┘  │ │
│  │  └────────────────────────────────────────────────────────────────┘  │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
│                                   │                                         │
│                         HTTPS / JSON over HTTP                            │
│                                   │                                         │
└───────────────────────────────────┼─────────────────────────────────────────┘
                                    ▼
        ┌───────────────────────────────────────────────────────┐
        │      ASP.NET Core 8.0 REST API Server                │
        │   (https://localhost:7147)                           │
        ├───────────────────────────────────────────────────────┤
        │                                                       │
        │  ┌──────────────────────────────────────────────────┐ │
        │  │          Program.cs                             │ │
        │  │  • DbContext configuration                      │ │
        │  │  • CORS setup                                   │ │
        │  │  • Swagger/OpenAPI                              │ │
        │  └──────────────────────────────────────────────────┘ │
        │                         ▼                              │
        │  ┌──────────────────────────────────────────────────┐ │
        │  │     4 REST API Controllers                       │ │
        │  │  ┌────────────────────────────────────────────┐ │ │
        │  │  │ OrdersController (186 ln)                 │ │ │
        │  │  │ • CreateOrder (POST)                      │ │ │
        │  │  │ • GetOrders (GET with filter)             │ │ │
        │  │  │ • GetOrder (GET by ID)                    │ │ │
        │  │  │ • UpdateOrderStatus (PATCH)               │ │ │
        │  │  │ • CancelOrder (DELETE)                    │ │ │
        │  │  └────────────────────────────────────────────┘ │ │
        │  │  ┌────────────────────────────────────────────┐ │ │
        │  │  │ ProductsController                        │ │ │
        │  │  │ • GetProducts                             │ │ │
        │  │  └────────────────────────────────────────────┘ │ │
        │  │  ┌────────────────────────────────────────────┐ │ │
        │  │  │ ClientsController                         │ │ │
        │  │  │ • GetClients                              │ │ │
        │  │  └────────────────────────────────────────────┘ │ │
        │  │  ┌────────────────────────────────────────────┐ │ │
        │  │  │ PickupPointsController                    │ │ │
        │  │  │ • GetPickupPoints                         │ │ │
        │  │  └────────────────────────────────────────────┘ │ │
        │  └──────────────────────────────────────────────────┘ │
        │                         ▼                              │
        │  ┌──────────────────────────────────────────────────┐ │
        │  │   Entity Framework Core 8.0                     │ │
        │  │   (DbContext + Models)                          │ │
        │  │  ┌────────────────────────────────────────────┐ │ │
        │  │  │ OrdersDbContext                           │ │ │
        │  │  │ • DbSet<Order>                            │ │ │
        │  │  │ • DbSet<OrderItem>                        │ │ │
        │  │  │ • DbSet<Product>                          │ │ │
        │  │  │ • DbSet<User>                             │ │ │
        │  │  │ • DbSet<PickupPoint>                      │ │ │
        │  │  │                                           │ │ │
        │  │  │ Configuration:                            │ │ │
        │  │  │ • Relationships (FK)                      │ │ │
        │  │  │ • Cascade delete rules                    │ │ │
        │  │  │ • Data conversion (Status enum)           │ │ │
        │  │  └────────────────────────────────────────────┘ │ │
        │  └──────────────────────────────────────────────────┘ │
        │                         ▼                              │
        │  ┌──────────────────────────────────────────────────┐ │
        │  │   Pomelo MySQL Provider                         │ │
        │  │   (Connection pooling, query execution)         │ │
        │  └──────────────────────────────────────────────────┘ │
        │                                                       │
        └───────────────────────────────────┬───────────────────┘
                                           ▼
                    ┌─────────────────────────────────┐
                    │   MySQL 8.0 Database            │
                    │   (192.168.100.35:3306)         │
                    ├─────────────────────────────────┤
                    │ orders_db                       │
                    │ ┌───────────────────────────┐   │
                    │ │ users (3)                 │   │
                    │ │ • id                      │   │
                    │ │ • name                    │   │
                    │ │ • email                   │   │
                    │ │ • phone                   │   │
                    │ │ • address                 │   │
                    │ └───────────────────────────┘   │
                    │ ┌───────────────────────────┐   │
                    │ │ products (5)              │   │
                    │ │ • id                      │   │
                    │ │ • name                    │   │
                    │ │ • price                   │   │
                    │ │ • stock                   │   │
                    │ └───────────────────────────┘   │
                    │ ┌───────────────────────────┐   │
                    │ │ pickup_points (3)         │   │
                    │ │ • id                      │   │
                    │ │ • name                    │   │
                    │ │ • address                 │   │
                    │ │ • phone                   │   │
                    │ └───────────────────────────┘   │
                    │ ┌───────────────────────────┐   │
                    │ │ orders                    │   │
                    │ │ • id                      │   │
                    │ │ • user_id → users         │   │
                    │ │ • pickup_point_id → pp    │   │
                    │ │ • status (ENUM)           │   │
                    │ │ • total_amount            │   │
                    │ │ • order_date              │   │
                    │ └───────────────────────────┘   │
                    │ ┌───────────────────────────┐   │
                    │ │ order_items               │   │
                    │ │ • id                      │   │
                    │ │ • order_id → orders       │   │
                    │ │ • product_id → products   │   │
                    │ │ • quantity                │   │
                    │ │ • unit_price              │   │
                    │ │ • subtotal                │   │
                    │ └───────────────────────────┘   │
                    │                                 │
                    │ User: user01                    │
                    │ Password: 81328                 │
                    │ Engine: InnoDB                  │
                    │ Charset: utf8mb4                │
                    └─────────────────────────────────┘
```

## 🔄 Поток данных при создании заказа

```
┌─────────────────────────────┐
│   Frontend Form Submit       │
│ (user selects data)         │
└──────────────┬──────────────┘
               ▼
┌─────────────────────────────────────────────────────────┐
│ JavaScript app.js                                       │
│ • Gather form data                                      │
│ • Validate inputs                                       │
│ • Build JSON payload                                    │
└──────────────┬──────────────────────────────────────────┘
               │
               ▼
        POST /api/orders
    Content-Type: application/json
    {
      "userId": 1,
      "pickupPointId": 1,
      "deliveryDate": "2024-12-25",
      "items": [
        {"productId": 1, "quantity": 1},
        {"productId": 2, "quantity": 2}
      ]
    }
               │
               ▼
┌─────────────────────────────────────────────────────────┐
│ OrdersController.CreateOrder()                          │
│ • Validate request                                      │
│ • Fetch products from DB                               │
│ • Check stock availability                             │
└──────────────┬──────────────────────────────────────────┘
               ▼
┌─────────────────────────────────────────────────────────┐
│ Entity Framework Core (LINQ)                            │
│ • Generate SQL queries                                  │
│ • Execute in transaction                               │
│ • Handle relationships                                  │
└──────────────┬──────────────────────────────────────────┘
               ▼
┌──────────────────────────────────────────────────────────┐
│ MySQL Queries                                           │
│ BEGIN TRANSACTION                                       │
│  1. INSERT INTO orders (...)                            │
│  2. INSERT INTO order_items (...)                       │
│  3. UPDATE products SET stock = ...                     │
│ COMMIT                                                  │
└──────────────┬───────────────────────────────────────────┘
               ▼
┌──────────────────────────────────────────────────────────┐
│ Response (201 Created)                                   │
│ {                                                        │
│   "id": 1,                                              │
│   "status": "Pending",                                  │
│   "totalAmount": 101997.00,                             │
│   "user": {...},                                        │
│   "orderItems": [...],                                  │
│   ...                                                    │
│ }                                                        │
└──────────────┬───────────────────────────────────────────┘
               ▼
┌──────────────────────────────────────────────────────────┐
│ Frontend JavaScript                                     │
│ • Parse response                                        │
│ • Show success notification                             │
│ • Reload orders list                                    │
│ • Navigate to orders tab                                │
└──────────────────────────────────────────────────────────┘
```

## 📊 Диаграмма базы данных (ERD)

```
┌──────────────────┐         ┌──────────────────┐
│      users       │         │   pickup_points  │
├──────────────────┤         ├──────────────────┤
│ id (PK)          │         │ id (PK)          │
│ name             │         │ name             │
│ email (UNIQUE)   │         │ address          │
│ phone            │         │ phone            │
│ address          │         │ working_hours    │
│ created_at       │         │ created_at       │
└────────┬─────────┘         └────────┬─────────┘
         │                           │
         │ (1:N)                    │ (1:N)
         │                           │
         └────────────┬──────────────┘
                      │
                      ▼
            ┌──────────────────┐
            │     orders       │
            ├──────────────────┤
            │ id (PK)          │
            │ user_id (FK)     │
            │ pickup_point_id  │
            │ (FK)             │
            │ order_date       │
            │ delivery_date    │
            │ status (ENUM)    │
            │ total_amount     │
            │ created_at       │
            │ updated_at       │
            └────────┬─────────┘
                     │
                     │ (1:N)
                     │
                     ▼
            ┌──────────────────┐
            │  order_items     │
            ├──────────────────┤
            │ id (PK)          │
            │ order_id (FK)    │
            │ product_id (FK)  │
            │ quantity         │
            │ unit_price       │
            │ subtotal         │
            │ created_at       │
            └────────┬─────────┘
                     │
                     │ (N:1)
                     │
                     ▼
            ┌──────────────────┐
            │    products      │
            ├──────────────────┤
            │ id (PK)          │
            │ name             │
            │ description      │
            │ price            │
            │ stock            │
            │ created_at       │
            └──────────────────┘

Relationships:
──────────
users → orders (1:N) - ON DELETE CASCADE
pickup_points → orders (1:N) - ON DELETE RESTRICT
orders → order_items (1:N) - ON DELETE CASCADE
products → order_items (N:1) - ON DELETE RESTRICT
```

## 🔐 Слои безопасности

```
┌────────────────────────────────────────────────────────┐
│ Layer 1: HTTPS/SSL                                     │
│ • Self-signed certificate (localhost only)             │
│ • Encryption in transit                                │
└────────────────────────────────────────────────────────┘
               ▼
┌────────────────────────────────────────────────────────┐
│ Layer 2: CORS Policy                                   │
│ • Allow all origins (configurable)                     │
│ • Specific methods (GET, POST, PATCH, DELETE)          │
│ • Specific headers                                      │
└────────────────────────────────────────────────────────┘
               ▼
┌────────────────────────────────────────────────────────┐
│ Layer 3: Controller Validation                         │
│ • ModelState check                                     │
│ • Business logic validation                            │
│ • Null/Empty checks                                    │
└────────────────────────────────────────────────────────┘
               ▼
┌────────────────────────────────────────────────────────┐
│ Layer 4: Database Constraints                          │
│ • Foreign keys                                         │
│ • Unique constraints                                   │
│ • Check constraints                                    │
│ • Data types enforcement                               │
└────────────────────────────────────────────────────────┘
               ▼
┌────────────────────────────────────────────────────────┐
│ Layer 5: Transaction Management                        │
│ • ACID compliance                                      │
│ • Rollback on error                                    │
│ • Stock consistency                                    │
└────────────────────────────────────────────────────────┘
```

## 🔌 Интеграция компонентов

```
Frontend Components:
├── Navigation Bar
│   ├── "Заказы" tab link
│   └── "Создать заказ" tab link
│
├── Orders Tab
│   ├── Filter by status (dropdown)
│   ├── Orders list (grid of cards)
│   │   ├── Order card (clickable)
│   │   │   ├── Order ID & Status badge
│   │   │   ├── Customer info
│   │   │   ├── Pickup point
│   │   │   ├── Items preview
│   │   │   └── Total amount
│   │   └── Order details modal (on click)
│   │       ├── Full order information
│   │       ├── Detailed items list
│   │       ├── Status change dropdown
│   │       ├── Cancel button
│   │       └── Apply button
│   └── Notifications (top-right)
│
└── Create Order Tab
    ├── Client selector (dropdown, API data)
    ├── Pickup point selector (dropdown, API data)
    ├── Delivery date (input, calendar)
    ├── Items section
    │   ├── Item rows (product + quantity)
    │   ├── Add item button
    │   └── Remove item button (per row)
    ├── Form actions
    │   ├── Submit button
    │   └── Cancel button
    └── Notifications (top-right)

Backend Components:
├── OrdersController
│   ├── CreateOrder() → POST /api/orders
│   ├── GetOrders() → GET /api/orders
│   ├── GetOrder() → GET /api/orders/{id}
│   ├── UpdateOrderStatus() → PATCH /api/orders/{id}/status
│   └── CancelOrder() → DELETE /api/orders/{id}
│
├── ProductsController
│   └── GetProducts() → GET /api/products
│
├── ClientsController
│   └── GetClients() → GET /api/clients
│
├── PickupPointsController
│   └── GetPickupPoints() → GET /api/pickup-points
│
└── OrdersDbContext
    ├── DbSet<Order>
    ├── DbSet<OrderItem>
    ├── DbSet<Product>
    ├── DbSet<User>
    ├── DbSet<PickupPoint>
    └── Configuration (relationships)

Database Layer:
├── users table
├── products table
├── pickup_points table
├── orders table
├── order_items table
├── Indexes
└── Foreign keys
```

## 📈 Масштабируемость

### Текущая архитектура
- Single-server deployment
- Direct MySQL connection
- No caching layer
- No load balancing

### Возможные улучшения
```
                    ┌─────────────────┐
                    │  Load Balancer  │
                    └────────┬────────┘
                             │
                ┌────────────┼────────────┐
                │            │            │
                ▼            ▼            ▼
        ┌───────────┐ ┌───────────┐ ┌───────────┐
        │ Server 1  │ │ Server 2  │ │ Server 3  │
        └─────┬─────┘ └─────┬─────┘ └─────┬─────┘
              └──────────────┼──────────────┘
                             ▼
                    ┌─────────────────┐
                    │  Redis Cache    │
                    └────────┬────────┘
                             │
                ┌────────────┼────────────┐
                │            │            │
                ▼            ▼            ▼
            ┌─────────────────────────────┐
            │   MySQL Primary Server       │
            └─────────────────────────────┘
                    │
                    ▼
            ┌─────────────────────────────┐
            │  MySQL Replica Servers      │
            └─────────────────────────────┘
```

---

Эта архитектура обеспечивает:
- ✅ Чистое разделение concernов (MVC pattern)
- ✅ Масштабируемость
- ✅ Легкость тестирования
- ✅ Простоту поддержания
- ✅ Гибкость для будущих расширений
