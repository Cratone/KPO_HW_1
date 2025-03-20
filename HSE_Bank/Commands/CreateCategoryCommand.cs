using System;
using HSE_Bank.Abstractions;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands
{
    public class CreateCategoryCommand : Command
    {
        private readonly string _name;
        private readonly OperationType _type;

        public CreateCategoryCommand(string name, OperationType type)
        {
            _name = name;
            _type = type;
        }

        public override string Execute(ServiceProvider serviceProvider)
        {
            CategoryFactory categoryFactory = serviceProvider.GetService<CategoryFactory>();
            IDataBase dataBase = serviceProvider.GetService<IDataBase>();
            Category? category = categoryFactory.CreateCategory(_name, _type);
            if (category != null)
            {
                dataBase.AddCategory(category);
                return "Категория создана.";
            }
            return "Категория не создана.";
        }
    }
}