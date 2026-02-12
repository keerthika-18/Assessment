use PasteryDB




SELECT DB_NAME();
CREATE TABLE Products
(
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductDescription NVARCHAR(100),
    ProductName NVARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);
CREATE TABLE Inventory
(
    InventoryId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Orders
(
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATETIME NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL
);
CREATE TABLE OrderItems
(
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

INSERT INTO Products (ProductName, ProductDescription, Price)
VALUES
('Chocolate Cake', 'Rich chocolate pastry cake', 450.00),
('Veg Puff', 'Crispy baked puff', 40.00),
('Butter Cookie', 'Crunchy butter cookies', 120.00),
('Cream Roll', 'Sweet cream filled roll', 60.00);

INSERT INTO Inventory (ProductId, Quantity)
VALUES
(1, 10),
(2, 50),
(3, 30),
(4, 25);

INSERT INTO Orders (OrderDate, TotalPrice)
VALUES
(GETDATE(), 580.00),
(GETDATE(), 240.00);

INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
VALUES
(1, 1, 1, 450.00),
(1, 2, 2, 80.00),
(2, 3, 2, 240.00);
SELECT * FROM Products;
SELECT * FROM Inventory;
SELECT * FROM Orders;
SELECT * FROM OrderItems;

SELECT 
    p.ProductId,
    p.ProductName,
    p.Price,
    i.Quantity AS AvailableQty
FROM Products p
JOIN Inventory i ON p.ProductId = i.ProductId
