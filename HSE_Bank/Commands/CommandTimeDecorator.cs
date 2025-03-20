using HSE_Bank.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace HSE_Bank.Commands
{
    public class CommandTimeDecorator : CommandDecorator
    {
        public CommandTimeDecorator(Command command) : base(command) { }

        public override string Execute(ServiceProvider serviceProvider)
        {
            var startTime = DateTime.Now;
            var result = Command.Execute(serviceProvider);
            var endTime = DateTime.Now;
            var executionTime = endTime - startTime;
            return result + $" Время выполнения команды: {executionTime}.";
        }
    }
}
