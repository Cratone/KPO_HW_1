using HSE_Bank.Models;
using Xunit;

namespace HSE_Bank_Tests;

public class ModelTests
{
    [Fact]
    public void BankAccount_Constructor_WithNameOnly_ShouldInitializeWithDefaults()
    {
        var accountName = "Test Account";
        var account = new BankAccount(accountName);

        Assert.Equal(accountName, account.Name);
        Assert.Equal(0, account.Balance);
        Assert.NotEqual(Guid.Empty, account.Id);
    }

    [Fact]
    public void BankAccount_Constructor_WithAllParameters_ShouldInitializeCorrectly()
    {
        var id = Guid.NewGuid();
        var name = "Test Account";
        var balance = 1000m;

        var account = new BankAccount(id, name, balance);

        Assert.Equal(id, account.Id);
        Assert.Equal(name, account.Name);
        Assert.Equal(balance, account.Balance);
    }

    [Fact]
    public void Category_Constructor_WithAllParameters_ShouldInitializeCorrectly()
    {
        var id = Guid.NewGuid();
        var name = "Food";
        var type = OperationType.Expense;

        var category = new Category(id, name, type);

        Assert.Equal(id, category.Id);
        Assert.Equal(name, category.Name);
        Assert.Equal(type, category.Type);
    }

    [Fact]
    public void Category_Constructor_WithNameAndType_ShouldInitializeWithDefaults()
    {
        var name = "Food";
        var type = OperationType.Expense;

        var category = new Category(name, type);

        Assert.Equal(name, category.Name);
        Assert.Equal(type, category.Type);
        Assert.NotEqual(Guid.Empty, category.Id);
    }

    [Fact]
    public void Operation_Constructor_WithAllParameters_ShouldInitializeCorrectly()
    {
        var id = Guid.NewGuid();
        var bankAccountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var type = OperationType.Expense;
        var amount = 500m;
        var time = DateTime.Now;
        var description = "Groceries";

        var operation = new Operation(
            id,
            bankAccountId,
            categoryId,
            type,
            amount,
            time,
            description
        );

        Assert.Equal(id, operation.Id);
        Assert.Equal(bankAccountId, operation.BankAccountId);
        Assert.Equal(categoryId, operation.CategoryId);
        Assert.Equal(type, operation.Type);
        Assert.Equal(amount, operation.Amount);
        Assert.Equal(time, operation.Time);
        Assert.Equal(description, operation.Description);
    }

    [Fact]
    public void Operation_Constructor_WithMinimalParameters_ShouldInitializeWithDefaults()
    {
        var bankAccountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var type = OperationType.Expense;
        var amount = 500m;
        var time = DateTime.Now;
        var description = "Groceries";

        var operation = new Operation(
            bankAccountId,
            categoryId,
            type,
            amount,
            time,
            description
        );

        Assert.NotEqual(Guid.Empty, operation.Id);
        Assert.Equal(bankAccountId, operation.BankAccountId);
        Assert.Equal(categoryId, operation.CategoryId);
        Assert.Equal(type, operation.Type);
        Assert.Equal(amount, operation.Amount);
        Assert.Equal(time, operation.Time);
        Assert.Equal(description, operation.Description);
    }
} 