using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Controllers
{
    internal class SymbolsMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        public SymbolsMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var str = message.Text;
            if (str == null) 
                return;
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"{str.Length}", cancellationToken: ct);

        }
    }
}
