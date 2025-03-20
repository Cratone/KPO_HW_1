using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_Bank.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Commands
{
    public class DeleteCategoryCommand : Command
    {
        private readonly Guid _categoryId;

        public DeleteCategoryCommand(Guid categoryId)
        {
            _categoryId = categoryId;
        }
        public override string Execute(ServiceProvider serviceProvider)
        {
            IDataBase dataBase = serviceProvider.GetService<IDataBase>();
            dataBase.RemoveCategory(_categoryId);
            dataBase.RecalculateAllOperations();
            return "Категория удалена.";
        }
    }
}
