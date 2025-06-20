using MediatR;
using NotificationMS.Common.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Application.Command
{
    public  class CreateNotificationCommand: IRequest<Guid>
    {
        public CreateNotificationDto Notification { get; set; }
        public CreateNotificationCommand(CreateNotificationDto notification)
        {
            Notification = notification;
        }

    }
}
