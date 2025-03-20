using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Data.Sqlite;

namespace HSE_Bank.Services
{
    public class SQLDateBase : IDataBase
    {
        private readonly string _connectionString;
        public SQLDateBase()
        {
            _connectionString = "Data Source=HSE_Bank.db";
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS BankAccounts (Id TEXT PRIMARY KEY, Name TEXT, Balance REAL)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Categories (Id TEXT PRIMARY KEY, Name TEXT, Type TEXT)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Operations (Id TEXT PRIMARY KEY, BankAccountId TEXT, CategoryId TEXT, Type TEXT, Amount REAL, Date TEXT, Description TEXT)";
                command.ExecuteNonQuery();
            }
        }
        public void AddBankAccount(BankAccount bankAccount) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO BankAccounts (Id, Name, Balance) VALUES (@id, @name, @balance)";
                command.Parameters.AddWithValue("@id", bankAccount.Id);
                command.Parameters.AddWithValue("@name", bankAccount.Name);
                command.Parameters.AddWithValue("@balance", bankAccount.Balance);   
                command.ExecuteNonQuery();
            }
        }
        public void AddCategory(Category category) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Categories (Id, Name, Type) VALUES (@id, @name, @type)";
                command.Parameters.AddWithValue("@id", category.Id);
                command.Parameters.AddWithValue("@name", category.Name);
                command.Parameters.AddWithValue("@type", category.Type);
                command.ExecuteNonQuery();
            }
        }
        public void AddOperation(Operation operation) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Operations (Id, BankAccountId, CategoryId, Amount, Type, Date, Description) VALUES (@id, @bankAccountId, @categoryId, @amount, @type, @date, @description)";
                command.Parameters.AddWithValue("@id", operation.Id);
                command.Parameters.AddWithValue("@bankAccountId", operation.BankAccountId); 
                command.Parameters.AddWithValue("@categoryId", operation.CategoryId);   
                command.Parameters.AddWithValue("@amount", operation.Amount);
                command.Parameters.AddWithValue("@type", operation.Type);
                command.Parameters.AddWithValue("@date", operation.Time);
                command.Parameters.AddWithValue("@description", operation.Description ?? "");
                command.ExecuteNonQuery();
            }
        }
        public void RemoveBankAccount(Guid id) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM BankAccounts WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM Operations WHERE BankAccountId = @id";
                command.ExecuteNonQuery();
            }
        }
        public void RemoveCategory(Guid id) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Categories WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM Operations WHERE CategoryId = @id";
                command.ExecuteNonQuery();
            }
        }
        public void RemoveOperation(Guid id) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Operations WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
        public BankAccount? GetBankAccount(Guid id) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM BankAccounts WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader()) {
                    if (reader.Read()) {
                        return new BankAccount(Guid.Parse(reader["Id"].ToString()), reader["Name"].ToString(), decimal.Parse(reader["Balance"].ToString()));
                    }
                }
                return null;
            }
        }
        public Category? GetCategory(Guid id) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Categories WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader()) {
                    if (reader.Read()) {
                        return new Category(Guid.Parse(reader["Id"].ToString()), reader["Name"].ToString(), (OperationType)Enum.Parse(typeof(OperationType), reader["Type"].ToString()));
                    }
                }
                return null;
            }
        }
        public Operation? GetOperation(Guid id) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Operations WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader()) {
                    if (reader.Read()) {
                        return new Operation(Guid.Parse(reader["Id"].ToString()), Guid.Parse(reader["BankAccountId"].ToString()), Guid.Parse(reader["CategoryId"].ToString()), (OperationType)Enum.Parse(typeof(OperationType), reader["Type"].ToString()), decimal.Parse(reader["Amount"].ToString()), DateTime.Parse(reader["Date"].ToString()), reader["Description"].ToString());
                    }
                }
                return null;
            }
        }
        public List<BankAccount> GetAllBankAccounts() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM BankAccounts";
                List<BankAccount> bankAccounts = new List<BankAccount>();   
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        bankAccounts.Add(new BankAccount(Guid.Parse(reader["Id"].ToString()), reader["Name"].ToString(), decimal.Parse(reader["Balance"].ToString())));
                    }
                }
                return bankAccounts;
            }
        }
        public List<Category> GetAllCategories() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Categories";
                List<Category> categories = new List<Category>();
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        categories.Add(new Category(Guid.Parse(reader["Id"].ToString()), reader["Name"].ToString(), (OperationType)Enum.Parse(typeof(OperationType), reader["Type"].ToString())));
                    }
                }
                return categories;
            }
        }
        public List<Operation> GetAllOperations() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Operations";
                List<Operation> operations = new List<Operation>();
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        operations.Add(new Operation(Guid.Parse(reader["Id"].ToString()), Guid.Parse(reader["BankAccountId"].ToString()), Guid.Parse(reader["CategoryId"].ToString()), (OperationType)Enum.Parse(typeof(OperationType), reader["Type"].ToString()), decimal.Parse(reader["Amount"].ToString()), DateTime.Parse(reader["Date"].ToString()), reader["Description"].ToString()));
                    }
                }
                return operations;
            }
        }
        public void RecalculateAllOperations() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE BankAccounts SET Balance = COALESCE((SELECT SUM(Amount) FROM Operations WHERE BankAccountId = BankAccounts.Id AND Type = 'Income'), 0) - COALESCE((SELECT SUM(Amount) FROM Operations WHERE BankAccountId = BankAccounts.Id AND Type = 'Expense'), 0) WHERE Id = BankAccounts.Id";
                command.ExecuteNonQuery();
            }
        }
        public void ChangeBalance(Guid bankAccountId, decimal amount) {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE BankAccounts SET Balance = Balance + @amount WHERE Id = @bankAccountId";
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@bankAccountId", bankAccountId);
                command.ExecuteNonQuery();
            }
        }

        public void ClearBankAccounts() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM BankAccounts";
                command.ExecuteNonQuery();
            }
        }           
        public void ClearCategories() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Categories";
                command.ExecuteNonQuery();
            }
        }
        public void ClearOperations() {
            using (var connection = new SqliteConnection(_connectionString)) {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Operations";
                command.ExecuteNonQuery();
            }
        }
    }
}
