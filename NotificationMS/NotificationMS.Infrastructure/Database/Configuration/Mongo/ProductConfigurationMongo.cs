using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationMS.Domain.Entities;

namespace NotificationMS.Infrastructure.Database.Configuration.Mongo
{

    [ExcludeFromCodeCoverage]
    public class NotificationConfigurationMongo
    {
        public static void Configure(IMongoCollection<Notification> collection)
        {
            // Índice único en NotificationId para evitar duplicados
            var indexKeysDefinition = Builders<Notification>.IndexKeys.Ascending(n => n.NotificationId.Value);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<Notification>(indexKeysDefinition, indexOptions);
            collection.Indexes.CreateOne(indexModel);

            // Índice en NotificationUserId para mejorar búsquedas por usuario destinatario
            indexKeysDefinition = Builders<Notification>.IndexKeys.Ascending(n => n.NotificationUserId.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

            // Índice en NotificationDateTime para optimizar consultas por fecha
            indexKeysDefinition = Builders<Notification>.IndexKeys.Ascending(n => n.NotificationDateTime.Value);
            collection.Indexes.CreateOne(indexKeysDefinition);

            // Índice en NotificationStatus para mejorar consultas por estado
            indexKeysDefinition = Builders<Notification>.IndexKeys.Ascending(n => n.NotificationStatus.ToString());
            collection.Indexes.CreateOne(indexKeysDefinition);
        }
    }
}
