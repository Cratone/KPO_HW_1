using HSE_Bank.Abstractions;
using HSE_Bank.Commands;

namespace HSE_Bank.Services
{
    public class CommandTimeDecoratorFactory : CommandDecoratorFactory
    {
        public override CommandDecorator CreateCommandDecorator(Command command)
        {
            return new CommandTimeDecorator(command);
        }
    }
}
