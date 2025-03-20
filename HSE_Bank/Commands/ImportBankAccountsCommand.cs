using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands {
    public class ImportBankAccountsCommand : Command {
        private readonly IImporter _importer;
        private readonly string _path;
        public ImportBankAccountsCommand(IImporter importer, string path) {
            _importer = importer;
            _path = path;
        }
        public override string Execute(ServiceProvider serviceProvider) {
            var dataBase = serviceProvider.GetRequiredService<IDataBase>();
            var bankAccounts = _importer.ImportBankAccounts(_path);
            dataBase.ClearBankAccounts();
            foreach (var bankAccount in bankAccounts) {
                dataBase.AddBankAccount(bankAccount);
            }
            return "Банковские счета импортированы.";
        }
    }
}   
