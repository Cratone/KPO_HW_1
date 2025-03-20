using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace HSE_Bank.FileWorkers.Import {
    public class CSVImporter : IImporter {
        public List<BankAccount> ParseBankAccounts(string data) {
            var reader = new StringReader(data);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<BankAccount>().ToList();
        }
        public List<Category> ParseCategories(string data) {
            var reader = new StringReader(data);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Category>().ToList();
        }
        public List<Operation> ParseOperations(string data) {
            var reader = new StringReader(data);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Operation>().ToList();
        }
    }
}


