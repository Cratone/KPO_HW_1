using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands {
    public class ExportCategoriesCommand : Command {
        private readonly IExporter _exporter;
        private readonly string _path;
        public ExportCategoriesCommand(IExporter exporter, string path) {
            _exporter = exporter;
            _path = path;
        }
        public override string Execute(ServiceProvider serviceProvider) {
            var dataBase = serviceProvider.GetRequiredService<IDataBase>();
            var categories = dataBase.GetAllCategories();
            _exporter.ExportCategories(categories, _path);
            return "Данные экспортированы.";
        }
    }
}
