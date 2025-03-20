using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;
using Sharprompt;

namespace HSE_Bank.Menu {
    public class IncureExpenseMenu {
        private readonly ServiceProvider _serviceProvider;
        private readonly BankAccount _bankAccount;
        public IncureExpenseMenu(ServiceProvider serviceProvider, BankAccount bankAccount)
        {
            _serviceProvider = serviceProvider;
            _bankAccount = bankAccount;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            List<Category> categories = dataBase.GetAllCategories().Where(cat => cat.Type == OperationType.Expense).ToList();
            if (categories.Count == 0) {
                Console.WriteLine("Нет категорий для снятия денег");
                return;
            }
            int i = 0;
            string categoryStr = Prompt.Select("Выберите категорию", categories.Select(cat => (++i) + ". " + cat.Name).ToArray());
            int categoryIndex = int.Parse(categoryStr.Split('.')[0]);
            Category category = categories[categoryIndex - 1];
            decimal amount = Prompt.Input<decimal>("Введите сумму");
            string description = null;
            if (Prompt.Confirm("Хотите добавить описание?")) {
                description = Prompt.Input<string>("Введите описание");
            }
            Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new IncurExpenseCommand(_bankAccount.Id, category.Id, amount, DateTime.Now, description)).Execute(_serviceProvider));
        }
    }
}
