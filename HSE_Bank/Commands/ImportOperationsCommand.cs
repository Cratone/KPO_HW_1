using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands {
    public class ImportOperationsCommand : Command {
        private readonly IImporter _importer;
        private readonly string _path;
        public ImportOperationsCommand(IImporter importer, string path) {
            _importer = importer;
            _path = path;
        }
        public override string Execute(ServiceProvider serviceProvider) {
            var dataBase = serviceProvider.GetRequiredService<IDataBase>();
            var operations = _importer.ImportOperations(_path);
            dataBase.ClearOperations();
            foreach (var operation in operations) {
                dataBase.AddOperation(operation);
            }
            return "Операции импортированы.";
        }
    }
}   
