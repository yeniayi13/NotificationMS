using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using NotificationMS.Common.Dtos.Response;
using NotificationMS.Domain.Entities;
using ProductsMS.Core.Repository;

namespace NotificationMS.Infrastructure.Repositories.Mongo
{
    public class NotificationRepositoryMongo : INotificationRepositoryMongo
    {
        private readonly IMongoCollection<Notification> _collection;
        private readonly IMapper _mapper;

        public NotificationRepositoryMongo(IMongoCollection<Notification> collection, IMapper mapper)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<Notification>> GetAllByUserIdAsync(Guid userId)
        {
            var filter = Builders<Notification>.Filter.Eq("NotificationUserId", userId);
            var projection = Builders<Notification>.Projection.Exclude("_id");
            var productsDto = await _collection
                .Find(filter) // Aplicamos el filtro
                .Project<GetNotificationDto>(projection) // Convertir los datos al DTO
                .ToListAsync()
                .ConfigureAwait(false);

            if (productsDto == null || productsDto.Count == 0)
            {
                Console.WriteLine("No se encontraron productos para este usuario.");
                return new List<Notification>(); // Retorna una lista vacía en lugar de `null` para evitar errores
            }

            var notificationEntities = _mapper.Map<List<Notification>>(productsDto);

            return notificationEntities;
           
        }
    }
}
