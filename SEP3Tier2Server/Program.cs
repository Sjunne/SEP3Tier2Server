using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MainServerAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SEP3Tier2Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Model model = new Model();
            Thread t1 = new Thread(() =>
            {

                while (true)
                {
                    model.FindMatches();
                    Thread.Sleep(600000); 
                }
            });
            t1.Start();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}