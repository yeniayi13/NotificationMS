using AutoMapper;
using NotificationMS.Common.Dtos.Response;
using NotificationMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMS.Common.AutoMapper
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, GetNotificationDto>()
                .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.NotificationId.Value))
                .ForMember(dest => dest.NotificationMessage, opt => opt.MapFrom(src => src.NotificationMessage.Value))
                .ForMember(dest => dest.NotificationSubject, opt => opt.MapFrom(src => src.NotificationSubject.Value))
                .ForMember(dest => dest.NotificationUserId, opt => opt.MapFrom(src => src.NotificationUserId.Value))
                .ForMember(dest => dest.NotificationDateTime, opt => opt.MapFrom(src => src.NotificationDateTime.Value))
                .ForMember(dest => dest.NotificationStatus, opt => opt.MapFrom(src => src.NotificationStatus.ToString())) // Convertir Enum a string
                .ReverseMap();
        }
    }
}
