using Newtonsoft.Json;
using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using System.Collections.Generic;

namespace HSE_Bank.FileWorkers.Import {
    public class JSONImporter : IImporter {
        public List<BankAccount> ParseBankAccounts(string data) {
            return JsonConvert.DeserializeObject<List<BankAccount>>(data);
        }
        public List<Category> ParseCategories(string data) {
            return JsonConvert.DeserializeObject<List<Category>>(data);
        }
        public List<Operation> ParseOperations(string data) {
            return JsonConvert.DeserializeObject<List<Operation>>(data);
        }
    }
}   
