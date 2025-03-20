using Microsoft.Extensions.DependencyInjection;
using HSE_Bank.Abstractions;
using HSE_Bank.Services;
using Sharprompt;
using HSE_Bank.Commands;
using HSE_Bank.Models;
using HSE_Bank.FileWorkers.Export;
using HSE_Bank.FileWorkers.Import;
using Microsoft.Extensions.Options;
using HSE_Bank.Menu;

namespace HSE_Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool is_running = true;
            ServiceProvider serviceProvider = ConfigureServices();

            while (is_running) {
                try {
                    is_running = new MainMenu(serviceProvider).Show();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }

        private static ServiceProvider ConfigureServices() {
            return new ServiceCollection()
                .AddSingleton<IDataBase, CashDateBase>()
                .Configure<CommandDecoratorSettings>(options =>
                {
                    options.UseTimeDecorator = false;
                })
                .AddTransient<CommandBlankDecoratorFactory>()
                .AddTransient<CommandTimeDecoratorFactory>()
                .AddTransient<CommandDecoratorFactory>(provider =>
                {
                    var settings = provider.GetRequiredService<IOptionsSnapshot<CommandDecoratorSettings>>().Value;

                    return settings.UseTimeDecorator
                        ? provider.GetRequiredService<CommandTimeDecoratorFactory>()
                        : provider.GetRequiredService<CommandBlankDecoratorFactory>();
                })
                .AddTransient<BankAccountFactory>()
                .AddTransient<CategoryFactory>()
                .AddTransient<OperationFactory>()
                .BuildServiceProvider();
        }
    }
}