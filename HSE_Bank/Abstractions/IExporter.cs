using HSE_Bank.Models;

namespace HSE_Bank.Abstractions {
    public interface IExporter {
        public string ParseBankAccounts(List<BankAccount> bankAccounts);
        public string ParseCategories(List<Category> categories);
        public string ParseOperations(List<Operation> operations);
        protected void Write(string data, string path) {
            File.WriteAllText(path, data);
        }

        public void ExportBankAccounts(List<BankAccount> bankAccounts, string path) {
            Write(ParseBankAccounts(bankAccounts), path);
        }
        public void ExportCategories(List<Category> categories, string path) {
            Write(ParseCategories(categories), path);
        }
        public void ExportOperations(List<Operation> operations, string path) {
            Write(ParseOperations(operations), path);
        }
    }   
}
