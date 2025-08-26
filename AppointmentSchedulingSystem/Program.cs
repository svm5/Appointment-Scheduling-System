// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();
//
// app.MapGet("/", () => "Hello World!");
//
// app.Run();

using AppointmentSchedulingSystem;
using Microsoft.EntityFrameworkCore;
using Persistence;
//
// ApplicationDbContextFactory factory = new ApplicationDbContextFactory();
// await using (var db = factory.CreateDbContext(new string[0]))
// {
//     var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
//     System.Console.WriteLine("Migrations:", pendingMigrations);
//     // Remove these lines if you are running migrations from the command line
//     await db.Database.MigrateAsync();
//     System.Console.WriteLine("here");
// }
// System.Console.WriteLine("Hello World!");
//
// System.Console.WriteLine("In main");
// ApplicationDbContextFactory factory = new ApplicationDbContextFactory();
// using (var db = factory.CreateDbContext(new string[0]))
// {
//     var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
//     System.Console.WriteLine("Migrations:");
//     System.Console.WriteLine(pendingMigrations.ToString());
//     System.Console.WriteLine(pendingMigrations.Count());
//
//     // Remove these lines if you are running migrations from the command line
//     // await db.Database.EnsureDeletedAsync();
//     await db.Database.MigrateAsync();
// }

// var builder = WebApplication.CreateBuilder(args);

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>()).Build();

// await host.Services.CreateScope().UseInfrastructureDataAccess();
await host.RunAsync();
//
// var host = Host.CreateHostBuilder(args).Build();
//
// using (var scope = host.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     await db.Database.MigrateAsync();
// }
//
// host.Run();