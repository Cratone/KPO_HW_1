using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HSE_Bank_Tests;

public class CommandTests
{
    [Fact]
    public void CommandBlankDecoratorFactory_CreateCommandDecorator_ShouldReturnCommandDecorator()
    {
        var factory = new CommandBlankDecoratorFactory();
        var mockCommand = new Mock<Command>();

        var decorator = factory.CreateCommandDecorator(mockCommand.Object);

        Assert.NotNull(decorator);
        Assert.IsType<CommandBlankDecorator>(decorator);
    }

    [Fact]
    public void CommandTimeDecoratorFactory_CreateCommandDecorator_ShouldReturnCommandDecorator()
    {
        var factory = new CommandTimeDecoratorFactory();
        var mockCommand = new Mock<Command>();

        var decorator = factory.CreateCommandDecorator(mockCommand.Object);

        Assert.NotNull(decorator);
        Assert.IsType<CommandTimeDecorator>(decorator);
    }

    private class TestCommandDecoratorFactory : CommandDecoratorFactory
    {
        private readonly IOptionsSnapshot<CommandDecoratorSettings> _options;
        private readonly CommandTimeDecoratorFactory _timeFactory;
        private readonly CommandBlankDecoratorFactory _blankFactory;

        public TestCommandDecoratorFactory(
            IOptionsSnapshot<CommandDecoratorSettings> options,
            CommandTimeDecoratorFactory timeFactory,
            CommandBlankDecoratorFactory blankFactory)
        {
            _options = options;
            _timeFactory = timeFactory;
            _blankFactory = blankFactory;
        }

        public override CommandDecorator CreateCommandDecorator(Command command)
        {
            return _options.Value.UseTimeDecorator
                ? _timeFactory.CreateCommandDecorator(command)
                : _blankFactory.CreateCommandDecorator(command);
        }
    }

    private class TestCommand : Command
    {
        public override string Execute(Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider)
        {
            return "Test command executed";
        }
    }

    private class TestCommandDecorator : CommandDecorator
    {
        public TestCommandDecorator(Command command) : base(command) { }

        public override string Execute(Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider)
        {
            return $"Test decorator: {Command.Execute(serviceProvider)}";
        }
    }

    [Fact]
    public void CommandDecoratorFactory_WhenTimeDecoratorIsEnabled_ShouldUseTimeDecorator()
    {
        var mockOptions = new Mock<IOptionsSnapshot<CommandDecoratorSettings>>();
        mockOptions.Setup(o => o.Value).Returns(new CommandDecoratorSettings { UseTimeDecorator = true });
        
        var timeFactory = new CommandTimeDecoratorFactory();
        var blankFactory = new CommandBlankDecoratorFactory();
        var factory = new TestCommandDecoratorFactory(mockOptions.Object, timeFactory, blankFactory);
        
        var command = new TestCommand();
        var decorator = factory.CreateCommandDecorator(command);
        
        Assert.NotNull(decorator);
        Assert.IsType<CommandTimeDecorator>(decorator);
    }

    [Fact]
    public void CommandDecoratorFactory_WhenTimeDecoratorIsDisabled_ShouldUseBlankDecorator()
    {
        var mockOptions = new Mock<IOptionsSnapshot<CommandDecoratorSettings>>();
        mockOptions.Setup(o => o.Value).Returns(new CommandDecoratorSettings { UseTimeDecorator = false });
        
        var timeFactory = new CommandTimeDecoratorFactory();
        var blankFactory = new CommandBlankDecoratorFactory();
        var factory = new TestCommandDecoratorFactory(mockOptions.Object, timeFactory, blankFactory);
        
        var command = new TestCommand();
        var decorator = factory.CreateCommandDecorator(command);
        
        Assert.NotNull(decorator);
        Assert.IsType<CommandBlankDecorator>(decorator);
    }
}

public class CommandDecoratorSettings
{
    public bool UseTimeDecorator { get; set; }
} 