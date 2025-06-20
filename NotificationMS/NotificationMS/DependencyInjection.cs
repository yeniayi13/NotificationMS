using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NotificationMS.Application.Handler.Command;
using NotificationMS.Application.Handler.Queries;
using NotificationMS.Core;
using NotificationMS.Core.Database;
using NotificationMS.Core.Service.User;
using NotificationMS.Infrastructure;
using NotificationMS.Infrastructure.Database.Context.Postgres;
using NotificationMS.Infrastructure.Repositories.Mongo;
using NotificationMS.Infrastructure.Repositories.Postgres;
using NotificationMS.Infrastructure.Services.User;
using ProductsMs.Core.Repository;
using ProductsMS.Core.Repository;


namespace NotificationMS
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithAuth(configuration);
           // services.KeycloakConfiguration(configuration);

             //Sin los Scope no funciona!!
           // services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationRepositoryMongo, NotificationRepositoryMongo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            //Registro de handlers 
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateNotificationCommandHandler).Assembly));
            
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetAllByUserIdQueryHandler).Assembly));
           
         
            
              services.AddHttpClient<UserService>(
                client =>
                {
                    client.BaseAddress = new Uri("https://localhost:18084");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
            );
            return services;
        }
    }
}