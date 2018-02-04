using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
// Requires NuGet package
// Microsoft.Extensions.Configuration.Json
using Microsoft.Extensions.Configuration;

namespace OrgBot
{
    public class SettingsProvider
    {
        private static IConfiguration _conf { get; set; }

        public SettingsProvider()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            _conf = builder.Build();
        }

        

        public string GetClientId() {
            return _conf["ClientId"];
        }

        public string GetClientSecret()
        {
            return _conf["ClientSecret"];
        }
    }
}
