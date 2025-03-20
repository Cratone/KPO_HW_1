using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_Bank.Models;

namespace HSE_Bank.Services
{
    public class CategoryFactory
    {
        public Category? CreateCategory(string name, OperationType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            return new Category(name, type);
        }
    }
}
