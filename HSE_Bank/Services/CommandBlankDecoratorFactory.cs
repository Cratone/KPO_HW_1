using HSE_Bank.Abstractions;
using HSE_Bank.Commands;

namespace HSE_Bank.Services
{
    public class CommandBlankDecoratorFactory : CommandDecoratorFactory
    {
        public override CommandDecorator CreateCommandDecorator(Command command)
        {
            return new CommandBlankDecorator(command);
        }
    }
}
