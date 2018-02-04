using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace OrgBot
{
    class Program
    {
        private LoggingProvider _logger;
        private SettingsProvider _config;

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _logger = new LoggingProvider();

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();

            _config = new SettingsProvider();

            _client.Log += _logger.Log;
            _client.Ready += OnReady;

            await InstallCommandsAsync();

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

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommandAsync;
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var commandPrefix = _config.GetCommandPrefix();

            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command, based on if it starts with the commandPrefix or a mention prefix
            if (!(message.HasStringPrefix(_config.GetCommandPrefix()+" ", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            // Create a Command Context
            var context = new SocketCommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
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
