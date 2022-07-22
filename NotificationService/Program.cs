using MassTransit;
using NotificationService;
using NotificationService.Consumer;
using Service;
using Service.Interface;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<INotificationMailService, NotificationMailService>();

        services.AddMassTransit(x =>
        {

            x.AddConsumer<EmailConsumer>(typeof(EmailConsumerDefinition));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", credintials =>
                {
                    credintials.Username("guest");
                    credintials.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });


        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
