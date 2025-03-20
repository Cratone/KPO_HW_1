using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;
using Sharprompt;

namespace HSE_Bank.Menu {
    public class CategoryMenu {
        private readonly ServiceProvider _serviceProvider;
        public CategoryMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            switch (Prompt.Select("Выберите опцию", new[] { "Создать категорию", "Выбрать категорию", "Назад" })) {
                case "Создать категорию":
                    string categoryName = Prompt.Input<string>("Введите имя категории");
                    string categoryType = Prompt.Select("Выберите тип категории", new[] { "Доход", "Расход" });
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new CreateCategoryCommand(categoryName, categoryType == "Доход" ? OperationType.Income : OperationType.Expense)).Execute(_serviceProvider));
                    break;
                case "Выбрать категорию":
                    new SelectCategoryMenu(_serviceProvider).Show();
                    break;
                case "Назад":
                    return;
            }
        }
    }
}