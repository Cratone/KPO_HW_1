using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.FileWorkers.Export;
using HSE_Bank.FileWorkers.Import;
using HSE_Bank.Models;
using HSE_Bank_Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace HSE_Bank_Tests;

public class ImportExportCommandTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly MockDatabase _database;
    private readonly ServiceProvider _serviceProvider;

    public ImportExportCommandTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "HSE_Bank_Commands_Tests");
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
        
        var services = new ServiceCollection();
        services.AddSingleton<IDataBase>(_database);
        services.AddTransient<CSVExporter>();
        services.AddTransient<CSVImporter>();
        services.AddTransient<JSONExporter>();
        services.AddTransient<JSONImporter>();
        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Fact]
    public void ExportBankAccountsCommand_ShouldExportBankAccounts()
    {
        var filePath = Path.Combine(_testDirectory, "export_bank_accounts.csv");
        var command = new ExportBankAccountsCommand(_serviceProvider.GetRequiredService<CSVExporter>(), filePath);
        
        var result = command.Execute(_serviceProvider);
        
        Assert.True(File.Exists(filePath));
        Assert.NotEmpty(result);
        
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
    public void ExportCategoriesCommand_ShouldExportCategories()
    {
        var filePath = Path.Combine(_testDirectory, "export_categories.json");
        var command = new ExportCategoriesCommand(_serviceProvider.GetRequiredService<JSONExporter>(), filePath);
        
        var result = command.Execute(_serviceProvider);
        
        Assert.True(File.Exists(filePath));
        Assert.NotEmpty(result);
        
        var fileContent = File.ReadAllText(filePath);
        foreach (var category in _database.GetAllCategories())
        {
            Assert.Contains(category.Id.ToString(), fileContent);
            Assert.Contains($"\"{category.Name}\"", fileContent);
            Assert.Contains(((int)category.Type).ToString(), fileContent);
        }
    }

    [Fact]
    public void ExportOperationsCommand_ShouldExportOperations()
    {
        var filePath = Path.Combine(_testDirectory, "export_operations.csv");
        var command = new ExportOperationsCommand(_serviceProvider.GetRequiredService<CSVExporter>(), filePath);
        
        var result = command.Execute(_serviceProvider);
        
        Assert.True(File.Exists(filePath));
        Assert.NotEmpty(result);
        
        var fileContent = File.ReadAllText(filePath);
        Assert.Contains("id,bank_account_id,amount,time,category_id,type,description", fileContent);
        foreach (var operation in _database.GetAllOperations())
        {
            Assert.Contains(operation.Id.ToString(), fileContent);
            Assert.Contains(operation.BankAccountId.ToString(), fileContent);
            Assert.Contains(operation.Amount.ToString(), fileContent);
            Assert.Contains(operation.CategoryId.ToString(), fileContent);
        }
    }

    [Fact]
    public void ImportBankAccountsCommand_ShouldImportBankAccounts()
    {
        var filePath = Path.Combine(_testDirectory, "import_bank_accounts.csv");
        var exportCommand = new ExportBankAccountsCommand(_serviceProvider.GetRequiredService<CSVExporter>(), filePath);
        exportCommand.Execute(_serviceProvider);
        
        _database.ClearBankAccounts();
        Assert.Empty(_database.GetAllBankAccounts());
        
        var importCommand = new ImportBankAccountsCommand(_serviceProvider.GetRequiredService<CSVImporter>(), filePath);
        
        var result = importCommand.Execute(_serviceProvider);
        
        Assert.NotEmpty(result);
        Assert.Equal(2, _database.GetAllBankAccounts().Count);
        Assert.Contains("Checking", _database.GetAllBankAccounts().Select(a => a.Name));
        Assert.Contains("Savings", _database.GetAllBankAccounts().Select(a => a.Name));
    }

    [Fact]
    public void ImportCategoriesCommand_ShouldImportCategories()
    {
        var filePath = Path.Combine(_testDirectory, "import_categories.json");
        var exportCommand = new ExportCategoriesCommand(_serviceProvider.GetRequiredService<JSONExporter>(), filePath);
        exportCommand.Execute(_serviceProvider);
        
        _database.ClearCategories();
        Assert.Empty(_database.GetAllCategories());
        
        var importCommand = new ImportCategoriesCommand(_serviceProvider.GetRequiredService<JSONImporter>(), filePath);
        
        var result = importCommand.Execute(_serviceProvider);
        
        Assert.NotEmpty(result);
        Assert.Equal(2, _database.GetAllCategories().Count);
        Assert.Contains("Salary", _database.GetAllCategories().Select(c => c.Name));
        Assert.Contains("Food", _database.GetAllCategories().Select(c => c.Name));
    }

    [Fact]
    public void ImportOperationsCommand_ShouldImportOperations()
    {
        var filePath = Path.Combine(_testDirectory, "import_operations.csv");
        var exportCommand = new ExportOperationsCommand(_serviceProvider.GetRequiredService<CSVExporter>(), filePath);
        exportCommand.Execute(_serviceProvider);
        
        _database.ClearOperations();
        Assert.Empty(_database.GetAllOperations());
        
        var importCommand = new ImportOperationsCommand(_serviceProvider.GetRequiredService<CSVImporter>(), filePath);
        
        var result = importCommand.Execute(_serviceProvider);
        
        Assert.NotEmpty(result);
        Assert.Equal(2, _database.GetAllOperations().Count);
        Assert.Contains("Monthly salary", _database.GetAllOperations().Select(o => o.Description));
        Assert.Contains("Groceries", _database.GetAllOperations().Select(o => o.Description));
    }
} 