using MassTransit;
using MongoDB.Driver;
using Steer.Api.Data;
using Steer.Api.Data.IRepos;
using Steer.Api.Data.Repos;
using Steer.Api.JsonConverters;
using Steer.Api.MessageContracts;
using Steer.Consumer;
using Steer.Consumer.Consumers;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<AddPointConsumer>();
                    x.AddConsumer<SetPointConsumer>();
                    x.AddConsumer<RemovePointConsumer>();
                    
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.ConfigureJsonSerializerOptions(opts =>
                        {
                            opts.Converters.Add(new ObjectIdJsonConverter());
                            return opts;
                        });
                        cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(2)));
                        cfg.ConfigureEndpoints(context);
                    });
                });
                services.AddScoped(q =>
                {
                    return new MongoClient(hostContext.Configuration["ConnectionStrings:Default"]);
                });

                

                services.AddScoped(q =>
                {
                    return (new MongoClient(hostContext.Configuration["ConnectionStrings:Default"])).GetDatabase("Steer");
                });

                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped(typeof(IClanRepository), typeof(ClanRepository));
                services.AddHostedService<Worker>();
            });
}