using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
using HSE_Bank.Abstractions;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.Services;
using Microsoft.Extensions.Options;
namespace HSE_Bank.Menu {
    public class SettingsMenu {
        private readonly ServiceProvider _serviceProvider;
        public SettingsMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            switch (Prompt.Select("Выберите опцию", new[] { "Считать время выполнения команд", "Назад" })) {
                case "Считать время выполнения команд":
                    if (Prompt.Confirm("Считать время выполнения команд?")) {
                        _serviceProvider.GetService<IOptionsSnapshot<CommandDecoratorSettings>>().Value.UseTimeDecorator = true;
                    }
                    else {
                        _serviceProvider.GetService<IOptionsSnapshot<CommandDecoratorSettings>>().Value.UseTimeDecorator = false;
                    }
                    break;
                case "Назад":
                    return;
            }
        }
    }
}