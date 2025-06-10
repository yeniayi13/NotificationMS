

using NotificationMS.Common.Dtos.Product.Response;

namespace NotificationMS.Core.Service.User
{
    public interface IUserService
    {
        Task<GetUser> BidderExists(Guid userId);

    }
}
