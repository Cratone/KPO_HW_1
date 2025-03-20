using HSE_Bank.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace HSE_Bank.Commands
{
    public class CommandBlankDecorator : CommandDecorator
    {
        public CommandBlankDecorator(Command command) : base(command) { }

        public override string Execute(ServiceProvider serviceProvider)
        {
            return Command.Execute(serviceProvider);
        }

    }
}
