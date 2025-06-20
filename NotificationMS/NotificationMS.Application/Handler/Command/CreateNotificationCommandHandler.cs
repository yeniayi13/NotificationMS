using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NotificationMS.Application.Command;
using NotificationMS.Common.Dtos.Response;
using NotificationMS.Core;
using NotificationMS.Core.RabbitMQ;
using NotificationMS.Core.Service.User;
using NotificationMS.Domain.Entities;
using NotificationMS.Domain.Entities.Enum;
using NotificationMS.Domain.Entities.ValueObjects;
using NotificationMS.Infrastructure.Exceptions;
using ProductsMs.Core.Repository;

namespace NotificationMS.Application.Handler.Command
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly IEventBus<GetNotificationDto> _eventBus;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IMapper mapper,
            IUserService userService, IEmailSender emailSender, IEventBus<GetNotificationDto> eventBus)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _userService = userService;
            _emailSender = emailSender;
            _eventBus = eventBus;
        }

        public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.BidderExists(request.Notification.NotificationUserId);
                if (user == null)
                {
                    throw new UserNotFoundException("Usuario no existe");
                }

                var notification = new Notification
                (
                    NotificationId.Create(Guid.NewGuid()),
                    NotificationMessage.Create(request.Notification.NotificationMessage),
                    NotificationSubject.Create(request.Notification.NotificationSubject),
                    NotificationUserId.Create(request.Notification.NotificationUserId),
                    NotificationDateTime.Create(DateTime.UtcNow),
                    NotificationStatus.Enviado
                );

                var notiDto = _mapper.Map<GetNotificationDto>(notification);
                await _notificationRepository.AddAsync(notification);
                await _eventBus.PublishMessageAsync(notiDto, "notificationQueue", "NOTIFICATION_CREATED");
                await _emailSender.SendEmailAsync(user.UserEmail, request.Notification.NotificationSubject,
                    request.Notification.NotificationMessage);
                
                return notification.NotificationId.Value;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw;
            }
        }
    }
}


