using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TfL.RoadStatus.Service;
using TfL.RoadStatus.Service.Contract;

namespace Tfl.RoadStatus
{
    class Program
    {
        static int Main(string[] args)
        {
            string roadName;
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify the road name.");
                roadName = Console.ReadLine();
            }
            else
                roadName = args[0];

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IRoadStatusPolicy, RoadStatusPolicy>()
                .BuildServiceProvider();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var appId = configuration.GetSection("ApiSettings:AppId").Value;
            var appKey = configuration.GetSection("ApiSettings:AppKey").Value;

            var roadStatusPolicy = serviceProvider.GetService<IRoadStatusPolicy>();
            var response = roadStatusPolicy.GetRoadStatus(new GetRoadStatusRequest { RoadName = roadName, AppId = appId, AppKey = appKey }).Result;

            if (response.Result is ExceptionDto)
            {
                Console.WriteLine($"{roadName} is not a valid road.");
                return -1;
            }

            var roadStatus = (RoadStatusDto)response.Result;

            Console.WriteLine($"The status of the {roadName} is as follows");
            Console.WriteLine($"\tRoad Status is {roadStatus.StatusSeverity}");
            Console.WriteLine($"\tRoad Status Description is {roadStatus.StatusSeverityDescription}");

            return 0;
        }
    }
}
