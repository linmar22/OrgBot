using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrgBot
{
    class LoggingProvider
    {

        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public Task LogToChannel(LogMessage msg)
        {
            return null;
        }
    }
}
