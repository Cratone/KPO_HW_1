using HSE_Bank.Abstractions;
using HSE_Bank.Models;

namespace HSE_Bank_Tests.Mocks;

public class MockDatabase : IDataBase
{
    private readonly List<BankAccount> _bankAccounts = new();
    private readonly List<Category> _categories = new();
    private readonly List<Operation> _operations = new();

    public void AddBankAccount(BankAccount bankAccount)
    {
        _bankAccounts.Add(bankAccount);
    }

    public void AddCategory(Category category)
    {
        _categories.Add(category);
    }

    public void AddOperation(Operation operation)
    {
        _operations.Add(operation);
    }

    public void RemoveBankAccount(Guid id)
    {
        _bankAccounts.RemoveAll(acc => acc.Id == id);
        _operations.RemoveAll(op => op.BankAccountId == id);
    }

    public void RemoveCategory(Guid id)
    {
        _categories.RemoveAll(cat => cat.Id == id);
        _operations.RemoveAll(op => op.CategoryId == id);
    }

    public void RemoveOperation(Guid id)
    {
        _operations.RemoveAll(op => op.Id == id);
    }

    public BankAccount? GetBankAccount(Guid id)
    {
        return _bankAccounts.FirstOrDefault(acc => acc.Id == id);
    }

    public Category? GetCategory(Guid id)
    {
        return _categories.FirstOrDefault(cat => cat.Id == id);
    }

    public Operation? GetOperation(Guid id)
    {
        return _operations.FirstOrDefault(op => op.Id == id);
    }

    public List<BankAccount> GetAllBankAccounts()
    {
        return _bankAccounts.ToList();
    }

    public List<Category> GetAllCategories()
    {
        return _categories.ToList();
    }

    public List<Operation> GetAllOperations()
    {
        return _operations.ToList();
    }

    public void RecalculateAllOperations()
    {
        // Reset all bank account balances
        foreach (var account in _bankAccounts)
        {
            account.Balance = 0;
        }

        // Apply all operations to recalculate balances
        foreach (var operation in _operations)
        {
            var account = GetBankAccount(operation.BankAccountId);
            if (account != null)
            {
                if (operation.Type == OperationType.Income)
                {
                    account.Balance += operation.Amount;
                }
                else
                {
                    account.Balance -= operation.Amount;
                }
            }
        }
    }

    public void ChangeBalance(Guid bankAccountId, decimal amount)
    {
        var account = GetBankAccount(bankAccountId);
        if (account != null)
        {
            account.Balance += amount;
        }
    }

    public void ClearBankAccounts()
    {
        _bankAccounts.Clear();
    }

    public void ClearCategories()
    {
        _categories.Clear();
    }

    public void ClearOperations()
    {
        _operations.Clear();
    }
} 