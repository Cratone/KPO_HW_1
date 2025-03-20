using HSE_Bank.Abstractions;
using HSE_Bank.Models;

namespace HSE_Bank.Services
{
    public class CashDateBase : IDataBase
    {
        private readonly List<BankAccount> _bankAccounts;
        private readonly List<Category> _categories;
        private readonly List<Operation> _operations;
        private readonly SQLDateBase _sqlDateBase;

        public CashDateBase()
        {
            _sqlDateBase = new SQLDateBase();
            _bankAccounts = _sqlDateBase.GetAllBankAccounts();
            _categories = _sqlDateBase.GetAllCategories();
            _operations = _sqlDateBase.GetAllOperations();
        }   
        
        public void AddBankAccount(BankAccount bankAccount)
        {
            _bankAccounts.Add(bankAccount);
            _sqlDateBase.AddBankAccount(bankAccount);
        }
        public void AddCategory(Category category)
        {
            _categories.Add(category);
            _sqlDateBase.AddCategory(category);
        }

        public void AddOperation(Operation operation)
        {
            _operations.Add(operation);
            _sqlDateBase.AddOperation(operation);
        }

        public void RemoveBankAccount(Guid id)
        {
            _bankAccounts.RemoveAll(acc => acc.Id == id);
            _operations.RemoveAll(op => op.BankAccountId == id);
            _sqlDateBase.RemoveBankAccount(id);
        }

        public void RemoveCategory(Guid id)
        {
            _categories.RemoveAll(cat => cat.Id == id);
            _operations.RemoveAll(op => op.CategoryId == id);
            _sqlDateBase.RemoveCategory(id);
        }
        public void RemoveOperation(Guid id)
        {
            _operations.RemoveAll(op => op.Id == id);
            _sqlDateBase.RemoveOperation(id);
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
            _sqlDateBase.RecalculateAllOperations();
        }

        public void ChangeBalance(Guid bankAccountId, decimal amount)
        {
            var bankAccount = GetBankAccount(bankAccountId);
            if (bankAccount != null)
            {
                bankAccount.Balance += amount;  
            }
            _sqlDateBase.ChangeBalance(bankAccountId, amount);
        }     

        public void ClearBankAccounts()
        {
            _bankAccounts.Clear();
            _sqlDateBase.ClearBankAccounts();
        }

        public void ClearCategories()
        {
            _categories.Clear();
            _sqlDateBase.ClearCategories();
        }

        public void ClearOperations()
        {
            _operations.Clear();
            _sqlDateBase.ClearOperations();
        }     
    }
}
