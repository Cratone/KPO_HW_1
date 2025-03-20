using HSE_Bank.Abstractions;
using HSE_Bank.Models;

namespace HSE_Bank.Services
{
    public class RunTimeDateBase : IDataBase
    {
        private readonly List<BankAccount> _bankAccounts;
        private readonly List<Category> _categories;
        private readonly List<Operation> _operations;

        public RunTimeDateBase()
        {
            _bankAccounts = new List<BankAccount>();
            _categories = new List<Category>();
            _operations = new List<Operation>();
        }

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
            return _bankAccounts;
        }

        public List<Category> GetAllCategories()
        {
            return _categories;
        }

        public List<Operation> GetAllOperations()
        {
            return _operations; 
        }

        public void RecalculateAllOperations()
        {
            foreach (var bankAccount in _bankAccounts)
            {
                bankAccount.Balance = 0;
                foreach (var operation in _operations.Where(op => op.BankAccountId == bankAccount.Id)) 
                {
                    if (operation.Type == OperationType.Income)
                    {
                        bankAccount.Balance += operation.Amount;
                    }
                    else
                    {
                        bankAccount.Balance -= operation.Amount;
                    }
                }
            }
        }

        public void ChangeBalance(Guid bankAccountId, decimal amount)
        {
            var bankAccount = GetBankAccount(bankAccountId);
            if (bankAccount != null)
            {
                bankAccount.Balance += amount;  
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
}