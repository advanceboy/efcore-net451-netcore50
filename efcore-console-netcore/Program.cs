using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace efcore_console {
    // https://github.com/aspnet/EntityFramework/blob/1.0.0-rc2/src/Microsoft.EntityFrameworkCore.Tools.Core/Design/Internal/StartupInvoker.cs
    // StartupInvoker の仕様をみるに、 $"Startup{environment}", "Startup", "Program", "App" の順番でクラスを探し、
    // 最初に見つかったクラスから、 "ConfigureServices", $"Configure{environment}Services" メソッドを探す。
    // static メソッドなら そのまま呼び出し、 そうでなければ コンストラクタで初期化してから
    // (.net Core の場合は、コンストラクタの引数で IHostingEnvironment が受け取れる)
    public class Program {
        public static IConfigurationRoot Configuration { get; }

        static Program() {
            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

#if DEBUG
            builder.AddJsonFile("appsettings.Development.json", true);
#endif

            Configuration = builder.Build();
        }
        
        // EF Core Tools から呼び出される
        public static void ConfigureServices(IServiceCollection services) {
            services
                .AddLogging()
                .AddDbContext<BloggingContext>(options => {
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), sqliteOptions => {
                    });
                });
        }

        public static void Configure(ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        }

        public static void Main(string[] args) {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            Configure(serviceProvider.GetService<ILoggerFactory>());


            using (var db = ActivatorUtilities.CreateInstance<BloggingContext>(serviceProvider)) {
                db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blogs) {
                    Console.WriteLine(" - {0}", blog.Url);
                }
            }
        }
    }
}
