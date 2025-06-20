using MediatR;
using NotificationMS.Common.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Application.Queries
{
    public class GetAllByUserIdQuery : IRequest<List<GetNotificationDto>>
    {
        public Guid UserId { get; set; }
        public GetAllByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
   
}
