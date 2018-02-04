using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrgBot.Commands
{
    // Create a module with no prefix
    public class ConvertTime : ModuleBase<SocketCommandContext>
    {
        SettingsProvider _config = new SettingsProvider();

        // ~say hello -> hello
        [Command("time")]
        [Summary("Echos a message.")]
        public async Task SayAsync([Remainder] [Summary("The time to convert")] string param)
        {
            if(param.ToLower() == "now")
            {
                var time = DateTime.UtcNow.ToString("HHmm") + " UTC";
                var date = DateTime.UtcNow.ToString("dd MMM yyyy").ToUpper();
                await ReplyAsync($"Current time: {time} {date}");
                return;
            }
        }
    }
}
