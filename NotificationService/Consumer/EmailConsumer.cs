using MassTransit;
using QueueClient.Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Consumer
{
    public class EmailConsumer : IConsumer<TodoPublisherModel>
    {
        private readonly INotificationMailService notificationMailService;

        public EmailConsumer(INotificationMailService notificationMailService)
        {
            this.notificationMailService = notificationMailService;
        }

        public async Task Consume(ConsumeContext<TodoPublisherModel> context)
        {
            var payload = context.Message;

            if (payload == null)
            {
                return;
            }

            var subject = $"Remind Todo {payload.Name} | Deadline {payload.Deadline}";
            var body = $"Remind Todo {payload.Name} | Deadline {payload.Deadline}";
            var toAddress = payload.UserEmail;

            await notificationMailService.SendEmailAsync(subject, body, toAddress);
        }
    }

    public class EmailConsumerDefinition :
    ConsumerDefinition<EmailConsumer>
    {
        public EmailConsumerDefinition()
        {
            // override the default endpoint name
            EndpointName = "todo1-service";

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<EmailConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
