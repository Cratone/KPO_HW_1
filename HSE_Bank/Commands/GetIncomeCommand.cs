using System;
using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands
{
    public class GetIncomeCommand : Command
    {
        private readonly Guid _bankAccountId;
        private readonly Guid _categoryId;
        private readonly decimal _amount;
        private readonly DateTime _date;
        private readonly string? _description;

        public GetIncomeCommand(Guid bankAccountId, Guid categoryId, decimal amount, DateTime date, string? description = null)
        {
            _bankAccountId = bankAccountId;
            _categoryId = categoryId;
            _amount = amount;
            _date = date;
            _description = description;
        }

        public override string Execute(ServiceProvider serviceProvider)
        {
            OperationFactory operationFactory = serviceProvider.GetService<OperationFactory>();
            IDataBase dataBase = serviceProvider.GetService<IDataBase>();
            BankAccount? bankAccount = dataBase.GetBankAccount(_bankAccountId);
            Category? category = dataBase.GetCategory(_categoryId);
            if (bankAccount != null && category != null && category.Type == OperationType.Income)
            {
                dataBase.ChangeBalance(_bankAccountId, _amount);
                var operation = operationFactory.CreateOperation(bankAccount, category, _amount, _date, _description);
                dataBase.AddOperation(operation);
                return "Доход добавлен.";
            }
            return "Доход не добавлен.";
        }
    }
} 