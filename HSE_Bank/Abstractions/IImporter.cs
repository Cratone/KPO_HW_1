using HSE_Bank.Models;
using System.IO;
namespace HSE_Bank.Abstractions {
    public interface IImporter {
        public List<BankAccount> ParseBankAccounts(string data);
        public List<Category> ParseCategories(string data);
        public List<Operation> ParseOperations(string data);
        public List<BankAccount> ImportBankAccounts(string path) {
            string data = File.ReadAllText(path);
            return ParseBankAccounts(data);
        }
        public List<Category> ImportCategories(string path) {
            string data = File.ReadAllText(path);
            return ParseCategories(data);
        }
        public List<Operation> ImportOperations(string path) {
            string data = File.ReadAllText(path);
            return ParseOperations(data);
        }
    }
}
