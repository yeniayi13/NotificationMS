//using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.IO;
using NotificationMS.Common.Dtos.Response;
using NotificationMS.Core.RabbitMQ;
using NotificationMS.Infrastructure.RabbitMQ.Consumer;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace NotificationMS.Infrastructure.RabbitMQ.Consumer
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQConsumer: IRabbitMQConsumer
    {
        private readonly IConnectionRabbbitMQ _rabbitMQConnection;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<GetNotificationDto> _collection;
        public RabbitMQConsumer(IConnectionRabbbitMQ rabbitMQConnection, IMongoCollection<GetNotificationDto> collection)
        {
            _rabbitMQConnection = rabbitMQConnection;

            //🔹 Conexión a MongoDB Atlas
            _mongoClient = new MongoClient("mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
            _database = _mongoClient.GetDatabase("NotificationMs");
            _collection = collection;

        }
        public RabbitMQConsumer() { }

        public async Task ConsumeMessagesAsync(string queueName)
        {
            var channel = _rabbitMQConnection.GetChannel();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensaje recibido: {message}");

                try
                {
                    var eventMessageD = JsonConvert.DeserializeObject<EventMessage<GetNotificationDto>>(message);
                    if (eventMessageD?.EventType == "NOTIFICATION_CREATED")
                    {
                        await _collection.InsertOneAsync(eventMessageD.Data);
                        Console.WriteLine($"Notification insertado en MongoDB: {JsonConvert.SerializeObject(eventMessageD.Data)}");
                    }
                   
                    await Task.Run(() => channel.BasicAckAsync(ea.DeliveryTag, false));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando el mensaje: {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine("Consumidor de RabbitMQ escuchando mensajes...");
        }
    }
}