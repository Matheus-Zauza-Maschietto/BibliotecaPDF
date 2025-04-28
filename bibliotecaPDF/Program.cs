using bibliotecaPDF.Context;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services;
using bibliotecaPDF.Services.Interfaces;
using bibliotecaPDF.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using B2Net;
using B2Net.Models;
using bibliotecaPDF.Controllers;
using bibliotecaPDF.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Logging.AddConsole();

builder.Services.ConfigurePostgresConnection(builder.Configuration);

builder.Services.ConfigureRedisConnection(builder.Configuration);

builder.Services.ConfigureUser();

builder.Services.ConfigureAutorization(builder.Configuration);

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDTOValidator>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.ConfigureSwagger();

builder.Services.ConfigureCors();

var app = builder.Build();

app.ConfigureInitialMigration();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();
