using System;
using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands
{
   public class DeleteBankAccountCommand: Command
    {
        private readonly Guid _bankAccountId;

        public DeleteBankAccountCommand(Guid bankAccountId)
        {
            _bankAccountId = bankAccountId;
        }

        public override string Execute(ServiceProvider serviceProvider)
        {
            IDataBase dataBase = serviceProvider.GetService<IDataBase>();
            dataBase.RemoveBankAccount(_bankAccountId);
            return "Банковский счет удален.";
        }
    }
}
