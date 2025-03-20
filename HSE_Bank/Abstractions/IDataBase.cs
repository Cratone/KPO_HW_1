using System.Collections.Generic;
using HSE_Bank.Models;

namespace HSE_Bank.Abstractions
{
    public interface IDataBase
    {
        public void AddBankAccount(BankAccount bankAccount);
        public void AddCategory(Category category);
        public void AddOperation(Operation operation);
        public void RemoveBankAccount(Guid id);
        public void RemoveCategory(Guid id);
        public void RemoveOperation(Guid id);
        public BankAccount? GetBankAccount(Guid id);
        public Category? GetCategory(Guid id);
        public Operation? GetOperation(Guid id);        
        public List<BankAccount> GetAllBankAccounts();
        public List<Category> GetAllCategories();
        public List<Operation> GetAllOperations();
        public void RecalculateAllOperations();
        public void ChangeBalance(Guid bankAccountId, decimal amount);
        public void ClearBankAccounts();
        public void ClearCategories();
        public void ClearOperations();
    }
} 