using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;

namespace HSE_Bank.Menu {
    public class GetIncomeMenu {
        private readonly ServiceProvider _serviceProvider;
        private readonly BankAccount _bankAccount;
        public GetIncomeMenu(ServiceProvider serviceProvider, BankAccount bankAccount)
        {
            _serviceProvider = serviceProvider;
            _bankAccount = bankAccount;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            var categories = dataBase.GetAllCategories().Where(cat => cat.Type == OperationType.Income).ToList();
            if (categories.Count == 0) {
                Console.WriteLine("Нет категорий для пополнения");
                return;
            }
            int i = 0;
            var categoryStr = Prompt.Select("Выберите категорию", categories.Select(cat => (++i) + ". " + cat.Name).ToArray());
            int categoryIndex = int.Parse(categoryStr.Split('.')[0]);
            var category = categories[categoryIndex - 1];
            decimal amount = Prompt.Input<decimal>("Введите сумму");
            string description = null;
            if (Prompt.Confirm("Хотите добавить описание?")) {
                description = Prompt.Input<string>("Введите описание");
            }
            Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new GetIncomeCommand(_bankAccount.Id, category.Id, amount, DateTime.Now, description)).Execute(_serviceProvider));
        }
    }
}
