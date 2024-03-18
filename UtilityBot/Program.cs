using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using UtilityBot.Configuration;
using UtilityBot.Controllers;
using UtilityBot.Services;

namespace UtilityBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<SymbolsMessageController>();
            services.AddTransient<ResultMessageController>();

            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "Bot_Token"
            };
        }
    }
}