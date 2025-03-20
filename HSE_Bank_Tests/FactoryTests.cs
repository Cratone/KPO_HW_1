using HSE_Bank.Models;
using HSE_Bank.Services;
using Xunit;

namespace HSE_Bank_Tests;

public class FactoryTests
{
    [Fact]
    public void BankAccountFactory_CreateBankAccount_ShouldCreateWithCorrectName()
    {
        var factory = new BankAccountFactory();
        var accountName = "Test Account";

        var account = factory.CreateBankAccount(accountName);

        Assert.NotNull(account);
        Assert.Equal(accountName, account.Name);
        Assert.Equal(0m, account.Balance);
        Assert.NotEqual(Guid.Empty, account.Id);
    }

    [Fact]
    public void CategoryFactory_CreateIncomeCategory_ShouldCreateWithCorrectName()
    {
        var factory = new CategoryFactory();
        var categoryName = "Salary";

        var category = factory.CreateCategory(categoryName, OperationType.Income);

        Assert.NotNull(category);
        Assert.Equal(categoryName, category.Name);
        Assert.Equal(OperationType.Income, category.Type);
        Assert.NotEqual(Guid.Empty, category.Id);
    }

    [Fact]
    public void CategoryFactory_CreateExpenseCategory_ShouldCreateWithCorrectName()
    {
        var factory = new CategoryFactory();
        var categoryName = "Food";

        var category = factory.CreateCategory(categoryName, OperationType.Expense);

        Assert.NotNull(category);
        Assert.Equal(categoryName, category.Name);
        Assert.Equal(OperationType.Expense, category.Type);
        Assert.NotEqual(Guid.Empty, category.Id);
    }

    [Fact]
    public void OperationFactory_CreateOperation_ShouldCreateWithCorrectValues()
    {
        var factory = new OperationFactory();
        var account = new BankAccount("Test Account");
        var category = new Category("Food", OperationType.Expense);
        decimal amount = 100m;
        var description = "Test Operation";
        var time = DateTime.Now;

        var operation = factory.CreateOperation(
            account,
            category,
            amount,
            time,
            description
        );

        Assert.NotNull(operation);
        Assert.Equal(account.Id, operation.BankAccountId);
        Assert.Equal(category.Id, operation.CategoryId);
        Assert.Equal(category.Type, operation.Type);
        Assert.Equal(amount, operation.Amount);
        Assert.Equal(description, operation.Description);
        Assert.Equal(time, operation.Time);
        Assert.NotEqual(Guid.Empty, operation.Id);
    }
} 