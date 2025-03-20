using HSE_Bank.Abstractions;
using HSE_Bank.FileWorkers.Export;
using HSE_Bank.FileWorkers.Import;
using HSE_Bank.Models;
using HSE_Bank_Tests.Mocks;

namespace HSE_Bank_Tests;

public class FileWorkersTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly MockDatabase _database;

    public FileWorkersTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "HSE_Bank_Tests");
        Directory.CreateDirectory(_testDirectory);
        _database = new MockDatabase();
        
        _database.AddBankAccount(new BankAccount(Guid.NewGuid(), "Checking", 1000m));
        _database.AddBankAccount(new BankAccount(Guid.NewGuid(), "Savings", 5000m));
        
        _database.AddCategory(new Category("Salary", OperationType.Income));
        _database.AddCategory(new Category("Food", OperationType.Expense));
        
        var account = _database.GetAllBankAccounts()[0];
        var incomeCategory = _database.GetAllCategories().Find(c => c.Type == OperationType.Income);
        var expenseCategory = _database.GetAllCategories().Find(c => c.Type == OperationType.Expense);
        
        if (account != null && incomeCategory != null && expenseCategory != null)
        {
            _database.AddOperation(new Operation(
                account.Id,
                incomeCategory.Id,
                OperationType.Income,
                1500m,
                DateTime.Now.AddDays(-5),
                "Monthly salary"
            ));
            
            _database.AddOperation(new Operation(
                account.Id,
                expenseCategory.Id,
                OperationType.Expense,
                300m,
                DateTime.Now.AddDays(-2),
                "Groceries"
            ));
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Fact]
    public void CSVExporter_ExportBankAccounts_ShouldCreateFileWithCorrectData()
    {
        var filePath = Path.Combine(_testDirectory, "bank_accounts.csv");
        IExporter exporter = new CSVExporter();
        
        exporter.ExportBankAccounts(_database.GetAllBankAccounts(), filePath);
        
        Assert.True(File.Exists(filePath));
        var fileContent = File.ReadAllText(filePath);
        Assert.Contains("id,name,balance", fileContent);
        foreach (var account in _database.GetAllBankAccounts())
        {
            Assert.Contains(account.Id.ToString(), fileContent);
            Assert.Contains(account.Name, fileContent);
            Assert.Contains(account.Balance.ToString(), fileContent);
        }
    }

    [Fact]
    public void CSVExporter_ExportCategories_ShouldCreateFileWithCorrectData()
    {
        var filePath = Path.Combine(_testDirectory, "categories.csv");
        IExporter exporter = new CSVExporter();
        
        exporter.ExportCategories(_database.GetAllCategories(), filePath);
        
        Assert.True(File.Exists(filePath));
        var fileContent = File.ReadAllText(filePath);
        Assert.Contains("id,name,type", fileContent);
        foreach (var category in _database.GetAllCategories())
        {
            Assert.Contains(category.Id.ToString(), fileContent);
            Assert.Contains(category.Name, fileContent);
            Assert.Contains(((int)category.Type).ToString(), fileContent);
        }
    }

    [Fact]
    public void CSVExporter_ExportOperations_ShouldCreateFileWithCorrectData()
    {
        var filePath = Path.Combine(_testDirectory, "operations.csv");
        IExporter exporter = new CSVExporter();
        
        exporter.ExportOperations(_database.GetAllOperations(), filePath);
        
        Assert.True(File.Exists(filePath));
        var fileContent = File.ReadAllText(filePath);
        Assert.Contains("id,bank_account_id,amount,time,category_id,type,description", fileContent);
        foreach (var operation in _database.GetAllOperations())
        {
            Assert.Contains(operation.Id.ToString(), fileContent);
            Assert.Contains(operation.BankAccountId.ToString(), fileContent);
            Assert.Contains(operation.Amount.ToString(), fileContent);
            Assert.Contains(operation.CategoryId.ToString(), fileContent);
            Assert.Contains(((int)operation.Type).ToString(), fileContent);
            Assert.Contains(operation.Description ?? string.Empty, fileContent);
        }
    }

    [Fact]
    public void JSONExporter_ExportBankAccounts_ShouldCreateFileWithCorrectData()
    {
        var filePath = Path.Combine(_testDirectory, "bank_accounts.json");
        IExporter exporter = new JSONExporter();
        
        exporter.ExportBankAccounts(_database.GetAllBankAccounts(), filePath);
        
        Assert.True(File.Exists(filePath));
        var fileContent = File.ReadAllText(filePath);
        foreach (var account in _database.GetAllBankAccounts())
        {
            Assert.Contains(account.Id.ToString(), fileContent);
            Assert.Contains($"\"{account.Name}\"", fileContent);
            Assert.Contains(account.Balance.ToString(), fileContent);
        }
    }

    [Fact]
    public void JSONExporter_ExportCategories_ShouldCreateFileWithCorrectData()
    {
        var filePath = Path.Combine(_testDirectory, "categories.json");
        IExporter exporter = new JSONExporter();
        
        exporter.ExportCategories(_database.GetAllCategories(), filePath);
        
        Assert.True(File.Exists(filePath));
        var fileContent = File.ReadAllText(filePath);
        foreach (var category in _database.GetAllCategories())
        {
            Assert.Contains(category.Id.ToString(), fileContent);
            Assert.Contains($"\"{category.Name}\"", fileContent);
            Assert.Contains(((int)category.Type).ToString(), fileContent);
        }
    }

    [Fact]
    public void JSONExporter_ExportOperations_ShouldCreateFileWithCorrectData()
    {
        var filePath = Path.Combine(_testDirectory, "operations.json");
        IExporter exporter = new JSONExporter();
        
        exporter.ExportOperations(_database.GetAllOperations(), filePath);
        
        Assert.True(File.Exists(filePath));
        var fileContent = File.ReadAllText(filePath);
        foreach (var operation in _database.GetAllOperations())
        {
            Assert.Contains(operation.Id.ToString(), fileContent);
            Assert.Contains(operation.BankAccountId.ToString(), fileContent);
            Assert.Contains(operation.Amount.ToString(), fileContent);
            Assert.Contains(operation.CategoryId.ToString(), fileContent);
            Assert.Contains(((int)operation.Type).ToString(), fileContent);
            Assert.Contains($"\"{operation.Description}\"", fileContent);
        }
    }

    [Fact]
    public void CSVImporter_ImportBankAccounts_ShouldReadDataFromFile()
    {
        var accounts = _database.GetAllBankAccounts();
        var filePath = Path.Combine(_testDirectory, "bank_accounts_import.csv");
        IExporter exporter = new CSVExporter();
        exporter.ExportBankAccounts(accounts, filePath);
        
        IImporter importer = new CSVImporter();
        
        var importedAccounts = importer.ImportBankAccounts(filePath);
        
        Assert.Equal(accounts.Count, importedAccounts.Count);
        for (int i = 0; i < accounts.Count; i++)
        {
            Assert.Equal(accounts[i].Id, importedAccounts[i].Id);
            Assert.Equal(accounts[i].Name, importedAccounts[i].Name);
            Assert.Equal(accounts[i].Balance, importedAccounts[i].Balance);
        }
    }

    [Fact]
    public void CSVImporter_ImportCategories_ShouldReadDataFromFile()
    {
        var categories = _database.GetAllCategories();
        var filePath = Path.Combine(_testDirectory, "categories_import.csv");
        IExporter exporter = new CSVExporter();
        exporter.ExportCategories(categories, filePath);
        
        IImporter importer = new CSVImporter();
        
        var importedCategories = importer.ImportCategories(filePath);
        
        Assert.Equal(categories.Count, importedCategories.Count);
        for (int i = 0; i < categories.Count; i++)
        {
            Assert.Equal(categories[i].Id, importedCategories[i].Id);
            Assert.Equal(categories[i].Name, importedCategories[i].Name);
            Assert.Equal(categories[i].Type, importedCategories[i].Type);
        }
    }

    [Fact]
    public void CSVImporter_ImportOperations_ShouldReadDataFromFile()
    {
        var operations = _database.GetAllOperations();
        var filePath = Path.Combine(_testDirectory, "operations_import.csv");
        IExporter exporter = new CSVExporter();
        exporter.ExportOperations(operations, filePath);
        
        IImporter importer = new CSVImporter();
        
        var importedOperations = importer.ImportOperations(filePath);
        
        Assert.Equal(operations.Count, importedOperations.Count);
        for (int i = 0; i < operations.Count; i++)
        {
            Assert.Equal(operations[i].Id, importedOperations[i].Id);
            Assert.Equal(operations[i].BankAccountId, importedOperations[i].BankAccountId);
            Assert.Equal(operations[i].CategoryId, importedOperations[i].CategoryId);
            Assert.Equal(operations[i].Type, importedOperations[i].Type);
            Assert.Equal(operations[i].Amount, importedOperations[i].Amount);
            Assert.Equal(operations[i].Description, importedOperations[i].Description);
        }
    }

    [Fact]
    public void JSONImporter_ImportBankAccounts_ShouldReadDataFromFile()
    {
        var accounts = _database.GetAllBankAccounts();
        var filePath = Path.Combine(_testDirectory, "bank_accounts_import.json");
        IExporter exporter = new JSONExporter();
        exporter.ExportBankAccounts(accounts, filePath);
        
        IImporter importer = new JSONImporter();
        
        var importedAccounts = importer.ImportBankAccounts(filePath);
        
        Assert.Equal(accounts.Count, importedAccounts.Count);
        for (int i = 0; i < accounts.Count; i++)
        {
            Assert.Equal(accounts[i].Id, importedAccounts[i].Id);
            Assert.Equal(accounts[i].Name, importedAccounts[i].Name);
            Assert.Equal(accounts[i].Balance, importedAccounts[i].Balance);
        }
    }

    [Fact]
    public void JSONImporter_ImportCategories_ShouldReadDataFromFile()
    {
        var categories = _database.GetAllCategories();
        var filePath = Path.Combine(_testDirectory, "categories_import.json");
        IExporter exporter = new JSONExporter();
        exporter.ExportCategories(categories, filePath);
        
        IImporter importer = new JSONImporter();
        
        var importedCategories = importer.ImportCategories(filePath);
        
        Assert.Equal(categories.Count, importedCategories.Count);
        for (int i = 0; i < categories.Count; i++)
        {
            Assert.Equal(categories[i].Id, importedCategories[i].Id);
            Assert.Equal(categories[i].Name, importedCategories[i].Name);
            Assert.Equal(categories[i].Type, importedCategories[i].Type);
        }
    }

    [Fact]
    public void JSONImporter_ImportOperations_ShouldReadDataFromFile()
    {
        var operations = _database.GetAllOperations();
        var filePath = Path.Combine(_testDirectory, "operations_import.json");
        IExporter exporter = new JSONExporter();
        exporter.ExportOperations(operations, filePath);
        
        IImporter importer = new JSONImporter();
        
        var importedOperations = importer.ImportOperations(filePath);
        
        Assert.Equal(operations.Count, importedOperations.Count);
        for (int i = 0; i < operations.Count; i++)
        {
            Assert.Equal(operations[i].Id, importedOperations[i].Id);
            Assert.Equal(operations[i].BankAccountId, importedOperations[i].BankAccountId);
            Assert.Equal(operations[i].CategoryId, importedOperations[i].CategoryId);
            Assert.Equal(operations[i].Type, importedOperations[i].Type);
            Assert.Equal(operations[i].Amount, importedOperations[i].Amount);
            Assert.Equal(operations[i].Description, importedOperations[i].Description);
        }
    }
} 