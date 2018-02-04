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

        public string GetClientId() {
            //_logger.Log(new LogMessage(LogSeverity.Info, "SettingsProvider", $"ClientID={_conf["ClientId"]}"));
            return _conf["ClientId"];
        }

        public string GetClientSecret()
        {
            //_logger.Log(new LogMessage(LogSeverity.Info, "SettingsProvider", $"ClientID={_conf["ClientSecret"]}"));
            return _conf["ClientSecret"];
        }

        public string GetAuthToken()
        {
            return _conf["AuthToken"];
        }
    }
}
