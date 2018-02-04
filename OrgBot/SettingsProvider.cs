using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
// Requires NuGet package
// Microsoft.Extensions.Configuration.Json
using Microsoft.Extensions.Configuration;
using Discord;

namespace OrgBot
{
    public class SettingsProvider
    {

        private static IConfiguration _conf { get; set; }
        private LoggingProvider _logger;


        public SettingsProvider()
        {
            _logger = new LoggingProvider();
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            _conf = builder.Build();
        }

        public ulong GetClientId() {
            //_logger.Log(new LogMessage(LogSeverity.Info, "SettingsProvider", $"ClientID={_conf["ClientId"]}"));
            return ulong.Parse(_conf["ClientId"]);
        }

        public string GetClientSecret()
        {
            //_logger.Log(new LogMessage(LogSeverity.Info, "SettingsProvider", $"ClientSecret={_conf["ClientSecret"]}"));
            return _conf["ClientSecret"];
        }

        public string GetAuthToken()
        {
            return _conf["AuthToken"];
        }

        public string GetNickname()
        {
            //_logger.Log(new LogMessage(LogSeverity.Info, "SettingsProvider.GetMainServerId", $"Nickname={_conf["Nickname"]}"));
            return _conf["Nickname"];
        }

        public ulong GetMainServerId()
        {
            //_logger.Log(new LogMessage(LogSeverity.Info, "SettingsProvider.GetMainServerId", $"MainServerId={_conf["MainServerId"]}"));
            return ulong.Parse(_conf["MainServerId"]);
        }

        public string GetCommandPrefix()
        {
            return _conf["CommandPrefix"];
        }
    }
}
