using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace HSE_Bank.FileWorkers.Export {
    public class CSVExporter : IExporter {
        public string ParseBankAccounts(List<BankAccount> bankAccounts) {
            using (var writer = new StringWriter()) {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                    csv.WriteRecords(bankAccounts);
                }
                
                return writer.ToString();
            }
        }

        public string ParseCategories(List<Category> categories) {
            using (var writer = new StringWriter()) {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                    csv.WriteRecords(categories);
                }

                return writer.ToString();
            }
        }

        public string ParseOperations(List<Operation> operations) {
            using (var writer = new StringWriter()) {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                    csv.WriteRecords(operations);
                }

                return writer.ToString();
            }
        }
    }
}


