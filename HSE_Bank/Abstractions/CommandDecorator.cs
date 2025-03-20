namespace HSE_Bank.Abstractions
{
    public abstract class CommandDecorator : Command
    {
        public Command Command { get; }
        public CommandDecorator(Command command) {
            Command = command;
        }
    }
}
