-- Create database
CREATE DATABASE IF NOT EXISTS orders_db;
USE orders_db;

-- Create users table
CREATE TABLE users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    address VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create pickup points table
CREATE TABLE pickup_points (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    address VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    working_hours VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create products table
CREATE TABLE products (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create orders table
CREATE TABLE orders (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    pickup_point_id INT NOT NULL,
    order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    delivery_date DATE,
    status ENUM('pending', 'processing', 'ready', 'completed', 'cancelled') DEFAULT 'pending',
    total_amount DECIMAL(10, 2) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (pickup_point_id) REFERENCES pickup_points(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create order items table
CREATE TABLE order_items (
    id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    subtotal DECIMAL(10, 2) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insert sample data
INSERT INTO users (name, email, phone, address) VALUES
('Иван Петров', 'ivan@example.com', '+79991234567', 'ул. Ленина, д. 10'),
('Мария Сидорова', 'maria@example.com', '+79992345678', 'ул. Пушкина, д. 20'),
('Петр Иванов', 'petr@example.com', '+79993456789', 'ул. Толстого, д. 30');

INSERT INTO pickup_points (name, address, phone, working_hours) VALUES
('Пункт выдачи #1', 'ул. Ленина, д. 50', '+78001234567', '09:00-21:00'),
('Пункт выдачи #2', 'ул. Октябрьская, д. 100', '+78001234568', '10:00-20:00'),
('Пункт выдачи #3', 'ул. Советская, д. 75', '+78001234569', '08:00-22:00');

INSERT INTO products (name, description, price, stock) VALUES
('Ноутбук Dell', 'Ноутбук Dell XPS 13, процессор Intel i7', 89999.00, 5),
('Клавиатура механическая', 'Механическая клавиатура с RGB подсветкой', 5999.00, 15),
('Мышь беспроводная', 'Беспроводная мышь Logitech MX Master 3', 7999.00, 20),
('Монитор 27"', 'IPS монитор 2560x1440', 24999.00, 8),
('Наушники', 'Наушники Sony WH-1000XM5', 29999.00, 12);

-- Create indexes for performance
CREATE INDEX idx_order_user ON orders(user_id);
CREATE INDEX idx_order_status ON orders(status);
CREATE INDEX idx_order_date ON orders(order_date);
CREATE INDEX idx_order_items_order ON order_items(order_id);
