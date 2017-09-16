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

        [Command("quack")]
        public async Task Quack(CommandContext ctx)
        {

            await ctx.RespondAsync($"<@&276427868592930828>!  {ctx.User.Mention} is looking for you!");
        }
    }
}
