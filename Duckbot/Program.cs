using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Duckbot
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;

        public static LinkedList<BattleEntity> battleEntities;

        static void Main(string[] args)
        {
            battleEntities = new LinkedList<BattleEntity>();

            /* // Start test code
            battleEntities.AddLast(new BattleEntity("Test 1", 5));
            battleEntities.Last.Value.Initiative = 1000;
            battleEntities.AddLast(new BattleEntity("Test 2", 5));
            battleEntities.Last.Value.Initiative = 500;
            battleEntities.AddLast(new BattleEntity("Test 3", 5));
            battleEntities.Last.Value.Initiative = 250;
            battleEntities.AddLast(new BattleEntity("Test 4", 5));
            battleEntities.Last.Value.Initiative = 125;
            // End test code */

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
