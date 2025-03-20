using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using HSE_Bank.FileWorkers.Export;

namespace HSE_Bank.Menu {
    public class ExportMenu {
        private readonly ServiceProvider _serviceProvider;
        public ExportMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            IExporter exporter = null;
            switch (Prompt.Select("Выберите тип файла", new[] { "CSV", "JSON", "Назад" })) {
                case "CSV":
                    exporter = new CSVExporter();
                    break;
                case "JSON":
                    exporter = new JSONExporter();
                    break;
                case "Назад":
                    return;
            }
            if (exporter == null) {
                return;
            }
            string path = Prompt.Input<string>("Введите путь к файлу");
            switch (Prompt.Select("Выберите какие данные экспортировать", new[] { "Банковские счета", "Категории", "Операции", "Назад" })) {
                case "Банковские счета":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new ExportBankAccountsCommand(exporter, path)).Execute(_serviceProvider));
                    break;
                case "Категории":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new ExportCategoriesCommand(exporter, path)).Execute(_serviceProvider));
                    break;
                case "Операции":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new ExportOperationsCommand(exporter, path)).Execute(_serviceProvider));
                    break;
                case "Назад":
                    return;
            }
        }
    }
}