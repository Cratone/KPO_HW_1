using System;
using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands
{
    public class CreateBankAccountCommand : Command
    {
        private readonly string _name;

        public CreateBankAccountCommand(string name)
        {
            _name = name;
        }

        public override string Execute(ServiceProvider serviceProvider)
        {
            BankAccountFactory bankAccountFactory = serviceProvider.GetService<BankAccountFactory>();
            IDataBase dataBase = serviceProvider.GetService<IDataBase>();
            var bankAccount = bankAccountFactory.CreateBankAccount(_name);
            if (bankAccount != null)
            {
                dataBase.AddBankAccount(bankAccount);
                return "Банковский счет создан.";
            }
            return "Банковский счет не создан.";
        }
    }
} 