using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands
{
    public class DeleteOperationCommand : Command
    {
        private readonly Guid _operationId;

        public DeleteOperationCommand(Guid operationId)
        {
            _operationId = operationId;
        }
        public override string Execute(ServiceProvider serviceProvider)
        {
            IDataBase dataBase = serviceProvider.GetService<IDataBase>();
            Operation? operation = dataBase.GetOperation(_operationId);
            if (operation == null)
            {
                return "Операция не удалена.";
            }
            BankAccount? bankAccount = dataBase.GetBankAccount(operation.BankAccountId);
            if (bankAccount == null)
            {
                return "Операция не удалена.";
            }
            if (operation.Type == OperationType.Income)
            {
                dataBase.ChangeBalance(bankAccount.Id, -operation.Amount);
            }
            else
            {
                dataBase.ChangeBalance(bankAccount.Id, operation.Amount);
            }
            dataBase.RemoveOperation(_operationId);
            return "Операция удалена.";
        }
    }
}
