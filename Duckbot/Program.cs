using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace Duckbot
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "MzU4Njc5MzM2MzY4NDcyMDY1.DJ79xQ.nSYXYLEopXHAJF-vno_OzOvcNgA",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping")) await e.Message.RespondAsync("pong!");
            };
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "~"
            });
            commands.RegisterCommands<Commands>();
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
