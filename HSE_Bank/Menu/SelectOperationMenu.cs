using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;
using Sharprompt;

namespace HSE_Bank.Menu {
    public class SelectOperationMenu {
        private readonly ServiceProvider _serviceProvider;
        private readonly BankAccount _bankAccount;
        public SelectOperationMenu(ServiceProvider serviceProvider, BankAccount bankAccount)
        {
            _serviceProvider = serviceProvider;
            _bankAccount = bankAccount;
        }

        public void Show()
        {
            IDataBase dataBase = _serviceProvider.GetService<IDataBase>();
            var operations = dataBase.GetAllOperations().Where(op => op.BankAccountId == _bankAccount.Id).ToList();
            if (operations.Count == 0) {
                Console.WriteLine("Операций нет");
                return;
            }
            int i = 0;
            var operationStr = Prompt.Select("Выберите операцию", operations.Select(op => (++i) + ". " + op.Time + " " + op.Amount + " " + op.Type + " " + op.Description).ToArray());
            int operationIndex = int.Parse(operationStr.Split('.')[0]);
            var operation = operations[operationIndex - 1];
            switch (Prompt.Select("Выберите опцию", new[] { "Удалить операцию", "Назад" })) {
                case "Удалить операцию":
                    Console.WriteLine(_serviceProvider.GetService<CommandDecoratorFactory>().CreateCommandDecorator(new DeleteOperationCommand(operation.Id)).Execute(_serviceProvider));
                    break;
                case "Назад":
                    return;
            }
        }
    }
}
