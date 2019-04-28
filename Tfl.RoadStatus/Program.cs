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

            var serviceProvider = RegisterDependencies();
            IConfigurationRoot configuration = GetConfiguration();

            var appId = configuration.GetSection("ApiSettings:AppId").Value;
            var appKey = configuration.GetSection("ApiSettings:AppKey").Value;

            var roadStatusPolicy = serviceProvider.GetService<IRoadStatusPolicy>();
            var response = roadStatusPolicy.GetRoadStatus(new GetRoadStatusRequest { RoadName = roadName, AppId = appId, AppKey = appKey }).Result;

            if (response is null)
                return WriteNotValidResult(roadName);

            return WriteRoadStatusResult(roadName, response);
        }

        private static int WriteRoadStatusResult(string roadName, RoadStatusResponse response)
        {
            Console.WriteLine($"The status of the {roadName} is as follows");
            Console.WriteLine($"\tRoad Status is {response.RoadStatus.StatusSeverity}");
            Console.WriteLine($"\tRoad Status Description is {response.RoadStatus.StatusSeverityDescription}");

            return 0;
        }

        private static int WriteNotValidResult(string roadName)
        {
            Console.WriteLine($"{roadName} is not a valid road.");
            return -1;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true, true)
                            .Build();
        }

        private static ServiceProvider RegisterDependencies()
        {
            return new ServiceCollection()
                            .AddSingleton<IHttpClient, HttpClient>()
                            .AddSingleton<IRoadStatusPolicy, RoadStatusPolicy>()
                            .BuildServiceProvider();
        }
    }
}
