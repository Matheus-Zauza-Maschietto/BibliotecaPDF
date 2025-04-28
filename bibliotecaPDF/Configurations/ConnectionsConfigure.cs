using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bibliotecaPDF.Context;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace bibliotecaPDF.Configurations;

public static class ConnectionsConfigure
{
    public static void ConfigurePostgresConnection(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureRedisConnection(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(options =>
        {
            var redisConfiguration = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisConnection"), true);
            redisConfiguration.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(redisConfiguration);
        });
    }
}
