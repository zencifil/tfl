using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Tfl.RoadStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var appId = configuration.GetSection("ApiSettings:AppId").Value;
            var appKey = configuration.GetSection("ApiSettings:AppKey").Value;
        }
    }
}
