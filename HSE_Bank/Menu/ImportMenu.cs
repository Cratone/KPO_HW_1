using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using HSE_Bank.FileWorkers.Import;

namespace HSE_Bank.Menu {
    public class ImportMenu {
        private readonly ServiceProvider _serviceProvider;
        public ImportMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            IImporter importer = null;
            switch (Prompt.Select("Выберите тип файла", new[] { "CSV", "JSON", "Назад" })) {
                case "CSV":
                    importer = new CSVImporter();
                    break;
                case "JSON":
                    importer = new JSONImporter();
                    break;
                case "Назад":
                    return;
            }
            if (importer == null) {
                return;
            }
            string path = Prompt.Input<string>("Введите путь к файлу");
            switch (Prompt.Select("Выберите какие данные импортировать", new[] { "Банковские счета", "Категории", "Операции", "Назад" })) {
                case "Банковские счета":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new ImportBankAccountsCommand(importer, path)).Execute(_serviceProvider));
                    break;
                case "Категории":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new ImportCategoriesCommand(importer, path)).Execute(_serviceProvider));
                    break;
                case "Операции":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new ImportOperationsCommand(importer, path)).Execute(_serviceProvider));
                    break;
                case "Назад":
                    return;
            }
        }
    }
}