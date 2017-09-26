using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Duckbot
{
    public class Commands
    {
        const string FilledHealth = ":heart:";
        const string EmptyHealth = ":broken_heart:";

        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"Hello, {ctx.User.Mention}!");
        }

        [Command("roll")]
        public async Task Dice(CommandContext ctx, string commands = "1d1000")
        {
            int numDice;
            int numSides;
            bool isNumeric;
            bool isValid;

            // Check to see if string is numeric
            isNumeric = int.TryParse(commands, out numSides);
            if (isNumeric) {
                numDice = 1;
                isValid = true;
            }
            else
            {
                // If not numeric, parse for a dice value
                char[] delimiterChars = { 'd', 'D' };
                string[] diceString = commands.Split(delimiterChars);
                isValid = true;  // Will be reset to false if invalid argument is found

                // Parse for number of dice
                isNumeric = int.TryParse(diceString[0], out numDice);
                if (!isNumeric) isValid = false;

                // Parse for number of sides, but only if valid
                if (isValid)
                {
                    isNumeric = int.TryParse(diceString[1], out numSides);
                    if (!isNumeric) isValid = false;
                }
            }

            if(isValid)
            {
                int total = 0;
                var rnd = new Random();
                for(int count = numDice; count > 0; count--)
                {
                    total += rnd.Next(numSides + 1);
                }
                await ctx.RespondAsync($"{ctx.User.Mention} rolled {total} on {numDice}d{numSides}.");
            }
            else
            {
                await ctx.RespondAsync($"Invalid dice roll.");
            }

        }

        [Command("quack")] // Quack command will be rewritten and renamed for Starlight
        public async Task Quack(CommandContext ctx)
        {
            await ctx.RespondAsync($"Quack command is currently disabled.");
//            await ctx.RespondAsync($"<@&276427868592930828>!  {ctx.User.Mention} is looking for you!");
        }

        [Command("battle")]
        public async Task Battle(CommandContext ctx, string commands = "", string subcommands = "", int number = 0)
        {
            BattleEntity currentEntity;
            LinkedListNode<BattleEntity> compareEntity;
            switch (commands)
            {
                case "clear":
                    Program.battleEntities.Clear();
                    await ctx.RespondAsync($"Battle list cleared.");
                    break;
                case "add":
                    currentEntity = new BattleEntity(subcommands, number);
                    if (Program.battleEntities.Count == 0)
                    {
//                        await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} added to empty list.");
                        Program.battleEntities.AddFirst(currentEntity);
                    }
                    else if (Program.battleEntities.Count == 1)
                    {
                        if (currentEntity.Initiative > Program.battleEntities.First.Value.Initiative) // which is also the last
                        {
                            //await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} added before singleton {Program.battleEntities.First.Value.Initiative}.");
                            Program.battleEntities.AddFirst(currentEntity);
                        }
                        else
                        {
                            //await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} added after singleton {Program.battleEntities.First.Value.Initiative}.");
                            Program.battleEntities.AddLast(currentEntity);
                        }
                    }
                    else
                    {
                        compareEntity = Program.battleEntities.First;
                        if (currentEntity.Initiative < Program.battleEntities.Last.Value.Initiative)
                        {
                            //await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} added after lowest {Program.battleEntities.Last.Value.Initiative}.");
                            Program.battleEntities.AddLast(currentEntity);
                        }
                        else if (currentEntity.Initiative > Program.battleEntities.First.Value.Initiative)
                        {
                            //await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} added before highest {Program.battleEntities.First.Value.Initiative}.");
                            Program.battleEntities.AddFirst(currentEntity);
                        }
                        else
                        {
                            while (currentEntity.Initiative < compareEntity.Next.Value.Initiative)
                            {
                                //                          await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} < {compareEntity.Next.Value.Initiative}");
                                compareEntity = compareEntity.Next;
                            }
                            //                    await ctx.RespondAsync($"Test message: Current init {currentEntity.Initiative} added between {compareEntity.Value.Initiative} and {compareEntity.Next.Value.Initiative}.");
                            Program.battleEntities.AddAfter(compareEntity, currentEntity);
                        }
                    }
                    await ctx.RespondAsync($"{currentEntity.EntityName} added to battle list.");
                    break;
                case "list":
                    if (Program.battleEntities.Count == 0)
                        await ctx.RespondAsync($"Battle list is empty.");
                    else
                    {
                        compareEntity = Program.battleEntities.First;
                        string healthValue;
                        do
                        {
                            if (compareEntity.Value.CurrentHealth <= 0)
                            {
                                await ctx.RespondAsync($"{compareEntity.Value.EntityName}.  Initiative: {compareEntity.Value.Initiative}.  Health: :skull: Defeated :skull:");
                                compareEntity = compareEntity.Next;

                            }
                            else
                            {
                                healthValue = " ";
                                for (int i = 1; i <= compareEntity.Value.CurrentHealth; i++)
                                {
                                    healthValue = healthValue + FilledHealth + " ";
                                }
                                for (int i = compareEntity.Value.CurrentHealth; i < compareEntity.Value.TotalHealth; i++)
                                {
                                    healthValue = healthValue + EmptyHealth + " ";
                                }
                                await ctx.RespondAsync($"{compareEntity.Value.EntityName}.  Initiative: {compareEntity.Value.Initiative}.  Health: {healthValue}");
                                compareEntity = compareEntity.Next;
                            }
                        } while (compareEntity != Program.battleEntities.Last);
                        // Since the last was not caught by the do loop, rerun the loop one more time
                        healthValue = " ";
                        for (int i = 1; i <= compareEntity.Value.CurrentHealth; i++)
                        {
                            healthValue = healthValue + FilledHealth + " ";
                        }
                        for (int i = compareEntity.Value.CurrentHealth; i < compareEntity.Value.TotalHealth; i++)
                        {
                            healthValue = healthValue + EmptyHealth + " ";
                        }
                        await ctx.RespondAsync($"{compareEntity.Value.EntityName}.  Initiative: {compareEntity.Value.Initiative}.  Health: {healthValue}");
                        compareEntity = compareEntity.Next;
                    }
                    break;
                default:
                    await ctx.RespondAsync($"Test message: Default case hit.");
                    break;
            }
        }
    }
}
