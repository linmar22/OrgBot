using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace OrgBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private LoggingProvider _logger;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _logger = new LoggingProvider();
            _client = new DiscordSocketClient();

            var config = new SettingsProvider();

            _client.Log += _logger.Log;

            await _client.LoginAsync(TokenType.Bot, config.GetAuthToken());
            
            await _client.StartAsync();

            await _logger.Log(new LogMessage(LogSeverity.Info, "MainAsync", "OrgBot Started"));

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

    }
}
