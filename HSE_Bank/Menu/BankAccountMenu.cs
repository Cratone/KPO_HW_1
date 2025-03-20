using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;

namespace HSE_Bank.Menu {
    public class BankAccountMenu {
        private readonly ServiceProvider _serviceProvider;

        public BankAccountMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            var option = Prompt.Select("Выберите опцию", new[] { "Создать банковский счет", "Выбрать банковский счет", "Назад" });
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();

            switch (option)
            {
                case "Создать банковский счет":
                    string accountName = Prompt.Input<string>("Введите имя банковского счета");
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new CreateBankAccountCommand(accountName)).Execute(_serviceProvider));
                    break;
                case "Выбрать банковский счет":
                    new SelectBankAccountMenu(_serviceProvider).Show();
                    break;
                case "Назад":
                    return;
            }
        }
    }
}
