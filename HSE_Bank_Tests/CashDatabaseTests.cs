using HSE_Bank.Models;
using HSE_Bank.Services;
using Moq;
using Xunit;

namespace HSE_Bank_Tests;

public class CashDatabaseTests
{
    private class TestCashDatabase : CashDateBase
    {
        public TestCashDatabase() : base()
        {
            ClearBankAccounts();
            ClearCategories();
            ClearOperations();
        }
    }

    [Fact]
    public void AddBankAccount_ShouldAddToInternalCollection()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        
        database.AddBankAccount(account);
        
        var accounts = database.GetAllBankAccounts();
        Assert.Single(accounts);
        Assert.Equal(account.Id, accounts[0].Id);
        Assert.Equal(account.Name, accounts[0].Name);
    }
    
    [Fact]
    public void AddCategory_ShouldAddToInternalCollection()
    {
        var database = new TestCashDatabase();
        var category = new Category("Food", OperationType.Expense);
        
        database.AddCategory(category);
        
        var categories = database.GetAllCategories();
        Assert.Single(categories);
        Assert.Equal(category.Id, categories[0].Id);
        Assert.Equal(category.Name, categories[0].Name);
        Assert.Equal(category.Type, categories[0].Type);
    }
    
    [Fact]
    public void AddOperation_ShouldAddToInternalCollection()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        
        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            100m,
            DateTime.Now,
            "Groceries"
        );
        
        database.AddOperation(operation);
        
        var operations = database.GetAllOperations();
        Assert.Single(operations);
        Assert.Equal(operation.Id, operations[0].Id);
        Assert.Equal(operation.BankAccountId, operations[0].BankAccountId);
        Assert.Equal(operation.CategoryId, operations[0].CategoryId);
        Assert.Equal(operation.Type, operations[0].Type);
        Assert.Equal(operation.Amount, operations[0].Amount);
        Assert.Equal(operation.Description, operations[0].Description);
    }
    
    [Fact]
    public void RemoveBankAccount_ShouldRemoveFromInternalCollection()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        database.AddBankAccount(account);
        
        database.RemoveBankAccount(account.Id);
        
        var accounts = database.GetAllBankAccounts();
        Assert.Empty(accounts);
    }
    
    [Fact]
    public void RemoveBankAccount_ShouldRemoveRelatedOperations()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        
        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            100m,
            DateTime.Now,
            "Groceries"
        );
        
        database.AddOperation(operation);
        
        database.RemoveBankAccount(account.Id);
        
        var operations = database.GetAllOperations();
        Assert.Empty(operations);
    }
    
    [Fact]
    public void RemoveCategory_ShouldRemoveFromInternalCollection()
    {
        var database = new TestCashDatabase();
        var category = new Category("Food", OperationType.Expense);
        database.AddCategory(category);
        
        database.RemoveCategory(category.Id);
        
        var categories = database.GetAllCategories();
        Assert.Empty(categories);
    }
    
    [Fact]
    public void RemoveCategory_ShouldRemoveRelatedOperations()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        
        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            100m,
            DateTime.Now,
            "Groceries"
        );
        
        database.AddOperation(operation);
        
        database.RemoveCategory(category.Id);
        
        var operations = database.GetAllOperations();
        Assert.Empty(operations);
    }
    
    [Fact]
    public void RemoveOperation_ShouldRemoveFromInternalCollection()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        
        var operation = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            100m,
            DateTime.Now,
            "Groceries"
        );
        
        database.AddOperation(operation);
        database.RemoveOperation(operation.Id);
        
        var operations = database.GetAllOperations();
        Assert.Empty(operations);
    }
    
    [Fact]
    public void GetBankAccount_ShouldReturnCorrectAccount()
    {
        var database = new TestCashDatabase();
        var account1 = new BankAccount("Account 1");
        var account2 = new BankAccount("Account 2");
        database.AddBankAccount(account1);
        database.AddBankAccount(account2);
        
        var result = database.GetBankAccount(account1.Id);
        
        Assert.NotNull(result);
        Assert.Equal(account1.Id, result.Id);
        Assert.Equal(account1.Name, result.Name);
    }
    
    [Fact]
    public void GetCategory_ShouldReturnCorrectCategory()
    {
        var database = new TestCashDatabase();
        var category1 = new Category("Category 1", OperationType.Income);
        var category2 = new Category("Category 2", OperationType.Expense);
        database.AddCategory(category1);
        database.AddCategory(category2);
        
        var result = database.GetCategory(category1.Id);
        
        Assert.NotNull(result);
        Assert.Equal(category1.Id, result.Id);
        Assert.Equal(category1.Name, result.Name);
        Assert.Equal(category1.Type, result.Type);
    }
    
    [Fact]
    public void GetOperation_ShouldReturnCorrectOperation()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        database.AddBankAccount(account);
        database.AddCategory(category);
        
        var operation1 = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            100m,
            DateTime.Now,
            "Groceries"
        );
        
        var operation2 = new Operation(
            account.Id,
            category.Id,
            OperationType.Expense,
            50m,
            DateTime.Now,
            "Dining"
        );
        
        database.AddOperation(operation1);
        database.AddOperation(operation2);
        
        var result = database.GetOperation(operation1.Id);
        
        Assert.NotNull(result);
        Assert.Equal(operation1.Id, result.Id);
        Assert.Equal(operation1.Amount, result.Amount);
        Assert.Equal(operation1.Description, result.Description);
    }
    
    [Fact]
    public void RecalculateAllOperations_ShouldUpdateAccountBalances()
    {
        var database = new TestCashDatabase();
        var account1 = new BankAccount("Account 1");
        var account2 = new BankAccount("Account 2");
        var incomeCategory = new Category("Salary", OperationType.Income);
        var expenseCategory = new Category("Food", OperationType.Expense);
        
        database.AddBankAccount(account1);
        database.AddBankAccount(account2);
        database.AddCategory(incomeCategory);
        database.AddCategory(expenseCategory);
        
        database.AddOperation(new Operation(
            account1.Id,
            incomeCategory.Id,
            OperationType.Income,
            1000m,
            DateTime.Now.AddDays(-5),
            "Salary"
        ));
        
        database.AddOperation(new Operation(
            account1.Id,
            expenseCategory.Id,
            OperationType.Expense,
            300m,
            DateTime.Now.AddDays(-3),
            "Groceries"
        ));
        
        database.AddOperation(new Operation(
            account2.Id,
            incomeCategory.Id,
            OperationType.Income,
            500m,
            DateTime.Now.AddDays(-2),
            "Side job"
        ));
        
        database.RecalculateAllOperations();
        
        var updatedAccount1 = database.GetBankAccount(account1.Id);
        var updatedAccount2 = database.GetBankAccount(account2.Id);
        
        Assert.Equal(700m, updatedAccount1.Balance);
        Assert.Equal(500m, updatedAccount2.Balance);
    }
    
    [Fact]
    public void ChangeBalance_ShouldUpdateAccountBalance()
    {
        var database = new TestCashDatabase();
        var account = new BankAccount("Test Account");
        database.AddBankAccount(account);
        
        database.ChangeBalance(account.Id, 100m);
        database.ChangeBalance(account.Id, -50m);
        
        var updatedAccount = database.GetBankAccount(account.Id);
        Assert.Equal(50m, updatedAccount.Balance);
    }
    
    [Fact]
    public void Clear_Methods_ShouldClearCollections()
    {
        var database = new TestCashDatabase();
        
        database.AddBankAccount(new BankAccount("Account 1"));
        database.AddCategory(new Category("Category 1", OperationType.Income));
        
        var account = database.GetAllBankAccounts()[0];
        var category = database.GetAllCategories()[0];
        
        database.AddOperation(new Operation(
            account.Id,
            category.Id,
            OperationType.Income,
            100m,
            DateTime.Now,
            "Test"
        ));
        
        database.ClearBankAccounts();
        database.ClearCategories();
        database.ClearOperations();
        
        Assert.Empty(database.GetAllBankAccounts());
        Assert.Empty(database.GetAllCategories());
        Assert.Empty(database.GetAllOperations());
    }
} 