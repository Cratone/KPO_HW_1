using Microsoft.Extensions.DependencyInjection;
using Sharprompt;
namespace HSE_Bank.Menu {
    public class MainMenu {
        private readonly ServiceProvider _serviceProvider;

        public MainMenu(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool Show()
        {
            var option = Prompt.Select("Выберите опцию", new[] { "Банковские счета", "Категории", "Экспорт данных", "Импорт данных", "Настройки", "Выход" });

            switch (option)
            {
                case "Банковские счета":
                    new BankAccountMenu(_serviceProvider).Show();
                    break;
                case "Категории":
                    new CategoryMenu(_serviceProvider).Show();
                    break;
                case "Экспорт данных":
                    new ExportMenu(_serviceProvider).Show();
                    break;
                case "Импорт данных":
                    new ImportMenu(_serviceProvider).Show();
                    break;
                case "Настройки":
                    new SettingsMenu(_serviceProvider).Show();
                    break;
                case "Выход":
                    return false;
            }
            return true;
        }
    }
}
