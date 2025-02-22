using Telegram.Bot;
using Telegram.Bot.Types;
using MyTelegramBot.secure;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace HomeWork24
{
    internal class Program
    {
        private static MyAppConfig appConfig = new MyAppConfig();

        private static void OnHandleUpdateStarted(string message)
        {
            Console.WriteLine($"Началась обработка сообщения '{message}'");
        }

        private static void OnHandleUpdateCompleted(string message)
        {
            Console.WriteLine($"Закончилась обработка сообщения '{message}'");
        }

        static async Task Main()
        {
            if (!appConfig.ReadConfig())
            {
                return;
            }

            var cts = new CancellationTokenSource();
            var botClient = new TelegramBotClient(appConfig.telegramApiKey);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
                DropPendingUpdates = true
            };
            var handler = new UpdateHandler();
            handler.OnHandleUpdateStarted += OnHandleUpdateStarted;
            handler.OnHandleUpdateCompleted += OnHandleUpdateCompleted;
            try 
            {
                botClient.StartReceiving(handler, receiverOptions, cts.Token);

                var me = await botClient.GetMe();
                Console.WriteLine($"{me.FirstName} запущен!");

                string a = "";
                while (true)
                {
                    Console.WriteLine("Нажмите клавишу A для выхода");
                    a = Console.ReadLine();
                    if (a == "A")
                    {
                        await cts.CancelAsync(); 
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"{me.Id} работает!");
                    }
                }
                //await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
            }
            finally
            {
                handler.OnHandleUpdateStarted -= OnHandleUpdateStarted;
                handler.OnHandleUpdateCompleted -= OnHandleUpdateCompleted;
            }

        }

    }
}
