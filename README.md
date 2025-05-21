# 📦 Inventory Management System (CLI-based)

A simple **Console-based Inventory Management System** built with **C#** and **ADO.NET** using **SQL Server** (no ORMs).

---

## 🚀 Features

- ✅ Add, View, Edit, Delete, and Search Products
- ✅ Fully Async ADO.NET queries (`SqlConnection`, `SqlCommand`)
- ✅ Secure: Prevents SQL Injection using Parameterized Queries
- ✅ Input Validation (e.g., empty check)
- ✅ Clean repository based architecture
- ✅ Simple interactive CLI experience

---

## 🛠 Requirements

- [.NET SDK 6+ or 8+](https://dotnet.microsoft.com/)
- Microsoft SQL Server + SQL Server Management Studio (SSMS)

---

## 🧱 Database Setup

1. Open SSMS.
2. Run the following SQL:

```sql
CREATE DATABASE InventoryDB;

USE InventoryDB;

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Quantity INT,
    Price FLOAT
);
```
---

## ⚙️ Configuration
Check your connection string in:

```csharp
// In Program.cs
string _connectionString = "Server=YOUR_PC_NAME;Database=InventoryDB;Integrated Security=True;";
// or
string _connectionString = "Server=YOUR_PC_NAME;Database=InventoryDB;Trusted_Connection=True;";

```
---

## 🧪 Unit Testing

Unit tests are written using **xUnit**, with **integration-style testing** on a real SQL Server database.

### ✅ Highlights:

- `ProductRepositoryTests.cs` verifies Add, Read, Update, Delete operations
- `IAsyncLifetime` ensures proper setup and cleanup
- All test products are auto-deleted after tests run to avoid database pollution
- Safe to run repeatedly without manual cleanup

### ▶️ Run tests manually:

```bash
dotnet test InventoryManagementTests/InventoryManagment.Tests/InventoryManagment.Tests.csproj
```

> Make sure your SQL Server is running and the `InventoryDB` database exists before testing.