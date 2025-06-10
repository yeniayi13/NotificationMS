
using Microsoft.Extensions.Hosting;
using NotificationMS.Core.RabbitMQ;

namespace Notifications.Infrastructure.RabbitMQ.Connection
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IRabbitMQConsumer _rabbitMQConsumer;

        public RabbitMQBackgroundService(IRabbitMQConsumer rabbitMQConsumer)
        {
            _rabbitMQConsumer = rabbitMQConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine(" Esperando la inicialización de RabbitMQ...");

            await Task.Delay(3000); // Pequeño retraso para asegurar la inicialización
            await _rabbitMQConsumer.ConsumeMessagesAsync("notificationQueue");

            Console.WriteLine(" Consumidor de RabbitMQ iniciado.");
        }
    }
}
