using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Commands;

namespace HSE_Bank.Menu
{
    public class SelectCategoryMenu
    {
        private readonly ServiceProvider _serviceProvider;
        public SelectCategoryMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }   
        
        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            var categories = dataBase.GetAllCategories();
            if (categories.Count == 0) {
                Console.WriteLine("Категорий нет");
                return;
            }
            int i = 0;
            var categoryStr = Prompt.Select("Выберите категорию", categories.Select(cat => (++i) + ". " + cat.Name + " (" + (cat.Type == OperationType.Income ? "Доход" : "Расход") + ")").ToArray());
            int categoryIndex = int.Parse(categoryStr.Split('.')[0]);
            var category = categories[categoryIndex - 1];
            switch (Prompt.Select("Выберите опцию", new[] { "Удалить категорию", "Назад" })) {
                case "Удалить категорию":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new DeleteCategoryCommand(category.Id)).Execute(_serviceProvider));
                    break;
                case "Назад":
                    return;
            }
        }
    }
}
