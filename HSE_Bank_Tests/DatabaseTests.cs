using HSE_Bank.Models;
using HSE_Bank_Tests.Mocks;
using Xunit;

namespace HSE_Bank_Tests;

public class DatabaseTests
{
    private readonly MockDatabase _database;

    public DatabaseTests()
    {
        _database = new MockDatabase();
    }

    [Fact]
    public void AddBankAccount_ShouldAddAccountToDatabase()
    {
        var account = new BankAccount("Test Account");

        _database.AddBankAccount(account);

        var accounts = _database.GetAllBankAccounts();
        Assert.Single(accounts);
        Assert.Equal(account.Id, accounts[0].Id);
        Assert.Equal(account.Name, accounts[0].Name);
        Assert.Equal(account.Balance, accounts[0].Balance);
    }

    [Fact]
    public void AddIncomeCategory_ShouldAddCategoryToDatabase()
    {
        var category = new Category("Salary", OperationType.Income);

        _database.AddCategory(category);

        var categories = _database.GetAllCategories();
        Assert.Single(categories);
        Assert.Equal(category.Id, categories[0].Id);
        Assert.Equal(category.Name, categories[0].Name);
        Assert.Equal(category.Type, categories[0].Type);
    }

    [Fact]
    public void AddExpenseCategory_ShouldAddCategoryToDatabase()
    {
        var category = new Category("Food", OperationType.Expense);

        _database.AddCategory(category);

        var categories = _database.GetAllCategories();
        Assert.Single(categories);
        Assert.Equal(category.Id, categories[0].Id);
        Assert.Equal(category.Name, categories[0].Name);
        Assert.Equal(category.Type, categories[0].Type);
    }

    [Fact]
    public void AddOperation_ShouldAddOperationToDatabase()
    {
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        _database.AddBankAccount(account);
        _database.AddCategory(category);

        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            500m,
            DateTime.Now,
            "Groceries"
        );

        _database.AddOperation(operation);

        var operations = _database.GetAllOperations();
        Assert.Single(operations);
        Assert.Equal(operation.Id, operations[0].Id);
        Assert.Equal(operation.BankAccountId, operations[0].BankAccountId);
        Assert.Equal(operation.CategoryId, operations[0].CategoryId);
        Assert.Equal(operation.Type, operations[0].Type);
        Assert.Equal(operation.Amount, operations[0].Amount);
        Assert.Equal(operation.Time, operations[0].Time);
        Assert.Equal(operation.Description, operations[0].Description);
    }

    [Fact]
    public void RemoveBankAccount_ShouldRemoveAccountAndRelatedOperations()
    {
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        _database.AddBankAccount(account);
        _database.AddCategory(category);

        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            500m,
            DateTime.Now,
            "Groceries"
        );

        _database.AddOperation(operation);

        _database.RemoveBankAccount(account.Id);

        var accounts = _database.GetAllBankAccounts();
        var operations = _database.GetAllOperations();
        Assert.Empty(accounts);
        Assert.Empty(operations);
    }

    [Fact]
    public void RemoveCategory_ShouldRemoveCategoryAndRelatedOperations()
    {
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        _database.AddBankAccount(account);
        _database.AddCategory(category);

        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            500m,
            DateTime.Now,
            "Groceries"
        );

        _database.AddOperation(operation);

        _database.RemoveCategory(category.Id);

        var categories = _database.GetAllCategories();
        var operations = _database.GetAllOperations();
        Assert.Empty(categories);
        Assert.Empty(operations);
    }

    [Fact]
    public void ChangeBalance_ShouldUpdateAccountBalance()
    {
        var account = new BankAccount("Test Account");
        account.Balance = 1000m;
        _database.AddBankAccount(account);

        _database.ChangeBalance(account.Id, 500m);

        var updatedAccount = _database.GetBankAccount(account.Id);
        Assert.NotNull(updatedAccount);
        Assert.Equal(1500m, updatedAccount.Balance);
    }

    [Fact]
    public void RecalculateAllOperations_ShouldUpdateBalancesBasedOnOperations()
    {
        var account = new BankAccount("Test Account");
        var incomeCategory = new Category("Salary", OperationType.Income);
        var expenseCategory = new Category("Food", OperationType.Expense);
        _database.AddBankAccount(account);
        _database.AddCategory(incomeCategory);
        _database.AddCategory(expenseCategory);

        _database.AddOperation(new Operation(
            account.Id,
            incomeCategory.Id,
            OperationType.Income,
            1000m,
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

        _database.RecalculateAllOperations();

        var updatedAccount = _database.GetBankAccount(account.Id);
        Assert.NotNull(updatedAccount);
        Assert.Equal(700m, updatedAccount.Balance);
    }
} 