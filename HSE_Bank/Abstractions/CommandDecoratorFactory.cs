using HSE_Bank.Abstractions;
using HSE_Bank.Commands;

namespace HSE_Bank.Abstractions
{
    public abstract class CommandDecoratorFactory
    {
        public abstract CommandDecorator CreateCommandDecorator(Command command);
    }
}