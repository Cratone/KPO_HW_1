using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;

namespace HSE_Bank.Menu {
    public class SelectBankAccountMenu {
        private readonly ServiceProvider _serviceProvider;

        public SelectBankAccountMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            var bankAccounts = dataBase.GetAllBankAccounts();
            if (bankAccounts.Count == 0) {
                Console.WriteLine("Банковских счетов нет");
                return;
            }
            int i = 0;
            var bankAccountStr = Prompt.Select("Выберите банковский счет", bankAccounts.Select(acc => (++i) + ". " + acc.Name + " (" + acc.Balance + ")").ToArray());
            int bankAccountIndex = int.Parse(bankAccountStr.Split('.')[0]);
            var bankAccount = bankAccounts[bankAccountIndex - 1];
            switch (Prompt.Select("Выберите опцию", new[] { "Пополнить счет", "Снять деньги", "Выбрать выполненную операцию", "Удалить счет", "Назад" })) {
                case "Пополнить счет":
                    new GetIncomeMenu(_serviceProvider, bankAccount).Show();
                    break;
                case "Снять деньги":
                    new IncureExpenseMenu(_serviceProvider, bankAccount).Show();
                    break;
                case "Выбрать выполненную операцию":
                    new SelectOperationMenu(_serviceProvider, bankAccount).Show();
                    break;
                case "Удалить счет":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new DeleteBankAccountCommand(bankAccount.Id)).Execute(_serviceProvider));
                    break;
                case "Назад":
                    return;
            }
        }
    }
}
