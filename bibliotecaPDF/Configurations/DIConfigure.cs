using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B2Net;
using B2Net.Models;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services;
using bibliotecaPDF.Services.Interfaces;

namespace bibliotecaPDF.Configurations;

public static class DIConfigure
{
    public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<JsonWebTokensService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBackBlazeService, BackBlazeService>();
        services.AddScoped<IBackBlazeRepository, BackBlazeRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IGenericRepository, GenericRepository>();
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IConnectionMappingService, ConnectionMappingService>();
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
        services.AddSingleton<IRedisObserverService, RedisObserverService>();
        services.AddScoped<IB2Client, B2Client>(p =>
        {
            return new B2Client(new B2Options()
            {
                KeyId = configuration["BackBlaze:KeyId"],
                ApplicationKey = configuration["BackBlaze:ApplicationKey"],
                BucketId = configuration["BackBlaze:BucketId"] 
            });;
        });
    }
}
