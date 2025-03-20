using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_Bank.Models;

namespace HSE_Bank.Services
{
    public class OperationFactory
    {
        public Operation? CreateOperation(BankAccount bankAccount, Category category, decimal amount, DateTime date, string? description = null)
        {
            if (amount <= 0)
            {
                return null;
            }
            return new Operation(bankAccount.Id, category.Id, category.Type, amount, date, description);
        }
    }
}
