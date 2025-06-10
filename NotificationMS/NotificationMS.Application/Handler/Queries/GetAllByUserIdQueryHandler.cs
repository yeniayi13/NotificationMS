using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NotificationMS.Application.Queries;
using NotificationMS.Common.Dtos.Response;
using NotificationMS.Infrastructure.Exceptions;
using ProductsMs.Core.Repository;
using ProductsMS.Core.Repository;

namespace NotificationMS.Application.Handler.Queries
{
    public class GetAllByUserIdQueryHandler: IRequestHandler<GetAllByUserIdQuery, List<GetNotificationDto>>
    {
        private readonly INotificationRepositoryMongo _notificationRepository;
        private readonly IMapper _mapper;

        public GetAllByUserIdQueryHandler(INotificationRepositoryMongo notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<List<GetNotificationDto>> Handle(GetAllByUserIdQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var notifications = await _notificationRepository.GetAllByUserIdAsync(request.UserId);
                if (notifications == null || !notifications.Any())
                {
                    throw new NotificationNotFoundException("No existe notificaciones para este usuario");
                }
                return _mapper.Map<List<GetNotificationDto>>(notifications);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, e.g., log it or rethrow it
                throw;
            }


           
        }
    }
}
