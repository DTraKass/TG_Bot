using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Controllers
{
    internal class ResultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        public ResultMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            float[] str = null;
            if (float.TryParse(message.Text, out float j))
            {
                str = message.Text.Split().Select(float.Parse).ToArray();
            }
            float res = 0;

            if(str == null)
                return;

            for(int i = 0; i < str.Length; i++)
            {
                res += str[i];
            }
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $" Сумма чисел: {res}", cancellationToken: ct);

        }
    }
}
