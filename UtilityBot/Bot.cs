using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Controllers;
using UtilityBot.Services;

namespace UtilityBot
{
    internal class Bot : BackgroundService
    {
        private readonly IStorage _memoryStorage;

        private ITelegramBotClient _telegramClient;
        private TextMessageController _textMessageController;
        private DefaultMessageController _defaultMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private SymbolsMessageController _symbolsMessageController;
        private ResultMessageController _resultMessageController;

        public Bot(ITelegramBotClient telegramClient,
            TextMessageController textMessageController,
            DefaultMessageController defaultMessageController,
            InlineKeyboardController inlineKeyboardController,
            SymbolsMessageController symbolsMessageController,
            IStorage memoryStorage,
            ResultMessageController resultMessageController)
        {
            _telegramClient = telegramClient;

            _memoryStorage = memoryStorage;

            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;
            _inlineKeyboardController = inlineKeyboardController;
            _symbolsMessageController = symbolsMessageController;
            _resultMessageController = resultMessageController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } },
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);

                        string _operation = _memoryStorage.GetSession(update.Message.Chat.Id).Operation;
                        switch (_operation)
                        {
                            case "res":
                                await _resultMessageController.Handle(update.Message, cancellationToken);
                                break;
                            case "sym":
                                await _symbolsMessageController.Handle(update.Message, cancellationToken); 
                                break;
                        }
                        break;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        break;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}
