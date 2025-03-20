using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HSE_Bank.FileWorkers.Export {
    public class JSONExporter : IExporter {
        public string ParseBankAccounts(List<BankAccount> bankAccounts) {
            return JsonConvert.SerializeObject(bankAccounts);
        }

        public string ParseCategories(List<Category> categories) {
            return JsonConvert.SerializeObject(categories);
        }
        
        public string ParseOperations(List<Operation> operations) {
            return JsonConvert.SerializeObject(operations);
        }
    }
}
