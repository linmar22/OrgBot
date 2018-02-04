using Discord;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrgBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private LoggingProvider _logger;
        private SettingsProvider _config;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _logger = new LoggingProvider();
            _client = new DiscordSocketClient();

            _config = new SettingsProvider();

            _client.Log += _logger.Log;
            _client.Ready += OnReady;

            await _client.LoginAsync(TokenType.Bot, _config.GetAuthToken());
            
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            await SetOwnNickname();
            await _logger.Log(new LogMessage(LogSeverity.Info, "MainAsync", "OrgBot Startup complete."));
        }

        private async Task SetOwnNickname()
        {
            try
            {
                var guild = _client.GetGuild(_config.GetMainServerId());
                var user = guild.GetUser(_config.GetClientId());
                var nick = user.Nickname;

                if (string.IsNullOrWhiteSpace(nick) || !nick.Equals(_config.GetNickname()))
                {
                    await _logger.Log(new LogMessage(LogSeverity.Info, "MainAsync.SetOwnNickname", $"Changing bot Nickname, {user.Nickname} -> {_config.GetNickname()}"));
                    await user.ModifyAsync(x =>
                    {
                        x.Nickname = _config.GetNickname();
                    });
                }
            }
            catch(Exception e)
            {
                await _logger.Log(new LogMessage(LogSeverity.Error, "MainAsync.SetOwnNickname", $"Failed to change nickname.", e));
            }
        }

    }
}
