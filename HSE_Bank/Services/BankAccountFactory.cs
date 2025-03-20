using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_Bank.Models;

namespace HSE_Bank.Services
{
    public class BankAccountFactory
    {
        public BankAccount? CreateBankAccount(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            return new BankAccount(name);
        }
    }
}
