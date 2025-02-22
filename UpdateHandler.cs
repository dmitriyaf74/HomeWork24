using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Telegram.Bot.TelegramBotClient;

namespace HomeWork24
{
    public delegate void MessageHandler(string message);
    record CatFactDto(string Fact, int length);

    internal class UpdateHandler : IUpdateHandler
    {
        public MessageHandler? OnHandleUpdateStarted;
        public MessageHandler? OnHandleUpdateCompleted;

        async Task IUpdateHandler.HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task IUpdateHandler.HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            OnHandleUpdateStarted?.Invoke(update.Message.Text);
            _=botClient.SendMessage(update.Message.Chat.Id, "Сообщение успешно принято", cancellationToken: token);

            if (update.Message.Text == "/cat")
            {
                var cts = new CancellationTokenSource();
                using var client = new HttpClient();
                object? catFact_ = await client.GetFromJsonAsync<CatFactDto>("https://catfact.ninja/fact", cts.Token);
                if (catFact_ is CatFactDto catFact)
                {
                    _=botClient.SendMessage(update.Message.Chat.Id, $"Fact: {catFact.Fact}   length: {catFact.length}", cancellationToken: token);
                }
                await cts.CancelAsync();
            }
            OnHandleUpdateCompleted?.Invoke(update.Message.Text);
        }
    }
}
