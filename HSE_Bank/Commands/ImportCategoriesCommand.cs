using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands {
    public class ImportCategoriesCommand : Command {
        private readonly IImporter _importer;
        private readonly string _path;
        public ImportCategoriesCommand(IImporter importer, string path) {
            _importer = importer;
            _path = path;
        }
        public override string Execute(ServiceProvider serviceProvider) {
            var dataBase = serviceProvider.GetRequiredService<IDataBase>();
            var categories = _importer.ImportCategories(_path);
            dataBase.ClearCategories();
            foreach (var category in categories) {
                dataBase.AddCategory(category);
            }
            return "Категории импортированы.";
        }
    }
}   
