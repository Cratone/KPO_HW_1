using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using HSE_Bank_Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HSE_Bank_Tests;

public class CommandImplementationTests
{
    private ServiceProvider SetupServiceProvider(MockDatabase database)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDataBase>(database);
        services.AddTransient<BankAccountFactory>();
        services.AddTransient<CategoryFactory>();
        services.AddTransient<OperationFactory>();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void CreateBankAccountCommand_ShouldCreateBankAccount()
    {
        var database = new MockDatabase();
        var serviceProvider = SetupServiceProvider(database);
        var command = new CreateBankAccountCommand("Test Account");

        var result = command.Execute(serviceProvider);

        var accounts = database.GetAllBankAccounts();
        Assert.Single(accounts);
        Assert.Equal("Test Account", accounts[0].Name);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void DeleteBankAccountCommand_ShouldDeleteBankAccount()
    {
        var database = new MockDatabase();
        var account = new BankAccount("Test Account");
        database.AddBankAccount(account);
        var serviceProvider = SetupServiceProvider(database);
        var command = new DeleteBankAccountCommand(account.Id);

        var result = command.Execute(serviceProvider);

        var accounts = database.GetAllBankAccounts();
        Assert.Empty(accounts);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateCategoryCommand_ShouldCreateCategory()
    {
        var database = new MockDatabase();
        var serviceProvider = SetupServiceProvider(database);
        var command = new CreateCategoryCommand("Food", OperationType.Expense);

        var result = command.Execute(serviceProvider);

        var categories = database.GetAllCategories();
        Assert.Single(categories);
        Assert.Equal("Food", categories[0].Name);
        Assert.Equal(OperationType.Expense, categories[0].Type);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void DeleteCategoryCommand_ShouldDeleteCategory()
    {
        var database = new MockDatabase();
        var category = new Category("Food", OperationType.Expense);
        database.AddCategory(category);
        var serviceProvider = SetupServiceProvider(database);
        var command = new DeleteCategoryCommand(category.Id);

        var result = command.Execute(serviceProvider);

        var categories = database.GetAllCategories();
        Assert.Empty(categories);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetIncomeCommand_ShouldAddIncomeOperation()
    {
        var database = new MockDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Salary", OperationType.Income);
        database.AddBankAccount(account);
        database.AddCategory(category);
        var serviceProvider = SetupServiceProvider(database);
        
        decimal amount = 1000m;
        var command = new GetIncomeCommand(account.Id, category.Id, amount, DateTime.Now, "Monthly salary");

        var result = command.Execute(serviceProvider);

        var operations = database.GetAllOperations();
        Assert.Single(operations);
        Assert.Equal(account.Id, operations[0].BankAccountId);
        Assert.Equal(category.Id, operations[0].CategoryId);
        Assert.Equal(amount, operations[0].Amount);
        Assert.Equal("Monthly salary", operations[0].Description);
        Assert.Equal(OperationType.Income, operations[0].Type);

        var updatedAccount = database.GetBankAccount(account.Id);
        Assert.NotNull(updatedAccount);
        Assert.Equal(amount, updatedAccount.Balance);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void IncurExpenseCommand_ShouldAddExpenseOperation()
    {
        var database = new MockDatabase();
        var account = new BankAccount("Test Account");
        account.Balance = 2000m;
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        var serviceProvider = SetupServiceProvider(database);
        
        decimal amount = 500m;
        var command = new IncurExpenseCommand(account.Id, category.Id, amount, DateTime.Now, "Groceries");

        var result = command.Execute(serviceProvider);

        var operations = database.GetAllOperations();
        Assert.Single(operations);
        Assert.Equal(account.Id, operations[0].BankAccountId);
        Assert.Equal(category.Id, operations[0].CategoryId);
        Assert.Equal(amount, operations[0].Amount);
        Assert.Equal("Groceries", operations[0].Description);
        Assert.Equal(OperationType.Expense, operations[0].Type);

        var updatedAccount = database.GetBankAccount(account.Id);
        Assert.NotNull(updatedAccount);
        Assert.Equal(1500m, updatedAccount.Balance);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void DeleteOperationCommand_ShouldDeleteOperationAndUpdateBalance()
    {
        var database = new MockDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        
        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            500m,
            DateTime.Now,
            "Groceries"
        );
        database.AddOperation(operation);
        database.ChangeBalance(account.Id, -500m);
        
        var serviceProvider = SetupServiceProvider(database);
        var command = new DeleteOperationCommand(operation.Id);

        var result = command.Execute(serviceProvider);

        var operations = database.GetAllOperations();
        Assert.Empty(operations);
        
        var updatedAccount = database.GetBankAccount(account.Id);
        Assert.NotNull(updatedAccount);
        Assert.Equal(0m, updatedAccount.Balance);
        Assert.NotEmpty(result);
    }
} 