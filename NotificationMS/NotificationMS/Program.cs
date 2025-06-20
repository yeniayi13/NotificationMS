using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NotificationMS;
using NotificationMS.Application;
using NotificationMS.Common.AutoMapper;
using NotificationMS.Common.Dtos.Response;
using NotificationMS.Core.Database;
using NotificationMS.Core.RabbitMQ;
using NotificationMS.Core.Service.User;
using NotificationMS.Domain.Entities;
using NotificationMS.Infrastructure;
using NotificationMS.Infrastructure.Database.Context.Mongo;
using NotificationMS.Infrastructure.RabbitMQ;
using NotificationMS.Infrastructure.RabbitMQ.Consumer;
using NotificationMS.Infrastructure.Services.User;
using NotificationMS.Infrastructure.Settings;
using Notifications.Infrastructure.RabbitMQ.Connection;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

// Registrar el serializador de GUID
BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
// Registro de los perfiles de AutoMapper
var profileTypes = new[]
{
    typeof(NotificationProfile),
    
};

foreach (var profileType in profileTypes)
{
    builder.Services.AddAutoMapper(profileType);
}

builder.Services.AddSingleton<IApplicationDbContextMongo>(sp =>
{
    const string connectionString = "mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
    var databaseName = "NotificationMs";
    return new ApplicationDbContextMongo(connectionString, databaseName);
});

builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
builder.Services.AddSingleton<IConnectionRabbbitMQ, RabbitMQConnection>();

builder.Services.AddHostedService<RabbitMQBackgroundService>();

builder.Services.AddScoped(sp =>
{
    var dbContext = sp.GetRequiredService<IApplicationDbContextMongo>();
    return dbContext.Database.GetCollection<Notification>("Notifications"); // Nombre de la colección en MongoDB
});

//RabbitMQ Configuration

builder.Services.AddSingleton<IConnectionFactory>(provider =>
{
    return new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "guest",
        Password = "guest",
    };
});

// ?? Registrar `RabbitMQConnection` pasando `IConnectionFactory` en el constructor
builder.Services.AddSingleton<IConnectionRabbbitMQ>(provider =>
{
    var connectionFactory = provider.GetRequiredService<IConnectionFactory>();
    var rabbitMQConnection = new RabbitMQConnection(connectionFactory);
    rabbitMQConnection.InitializeAsync().Wait(); // ?? Ejecutar inicialización antes de inyectarlo
    return rabbitMQConnection;
}); builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

// ?? Ahora los Producers pueden usar `RabbitMQConnection`

builder.Services.AddSingleton<IEventBus<GetNotificationDto>>(provider =>
{
    var rabbitMQConnection = provider.GetRequiredService<IConnectionRabbbitMQ>();
    return new RabbitMQProducer<GetNotificationDto>(rabbitMQConnection);
});

builder.Services.AddSingleton<IMongoCollection<GetNotificationDto>>(provider =>
{
    var mongoClient = new MongoClient("mongodb+srv://yadefreitas19:08092001@cluster0.owy2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    var database = mongoClient.GetDatabase("NotificationMs");
    return database.GetCollection<GetNotificationDto>("Notifications");
});

// ?? Registrar `RabbitMQConsumer` solo una vez
builder.Services.AddSingleton<RabbitMQConsumer>(provider =>
{

    var rabbitMQConnection = provider.GetRequiredService<IConnectionRabbbitMQ>();
    var productCollection = provider.GetRequiredService<IMongoCollection<GetNotificationDto>>();
    return new RabbitMQConsumer(rabbitMQConnection, productCollection);
});


// Iniciar el consumidor automáticamente con `RabbitMQBackgroundService`
builder.Services.AddHostedService<RabbitMQBackgroundService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();

var _appSettings = new AppSettings();
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
_appSettings = appSettingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.Configure<HttpClientUrl>(
    builder.Configuration.GetSection("HttpClientAddress"));



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IUserService, UserService>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Connected!");

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();


