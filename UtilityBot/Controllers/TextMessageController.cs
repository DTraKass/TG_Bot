using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace UtilityBot.Controllers
{
    internal class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Сумма" , $"res"),
                        InlineKeyboardButton.WithCallbackData($" Кол-во символов" , $"sym")
                    });

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот умеет:</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Складывать числа, если написать ему их через пробел,{Environment.NewLine}"+
                        $"{Environment.NewLine}Считать количество символов в предложении{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
            }
        }
    }
    
}
