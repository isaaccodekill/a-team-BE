using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Searchify.DynamoDb;
using System.Threading.Tasks;

namespace Searchify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                await MainAsync(args);
            }).GetAwaiter().GetResult();
            CreateHostBuilder(args).Build().Run();
        }

        static async Task MainAsync(string[] args)
        {
            DbClient.CreateClient(true);
            await DbClient.CreateTables();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
