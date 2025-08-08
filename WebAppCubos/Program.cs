using Application.UseCases.LoginUseCase.SignIn;
using Infrastructure.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using WebApi.Modules.Database;
using WebAppCubos.Middleware;
using WebAppCubos.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMigrator(builder.Configuration);
builder.Services.AddPostgress(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddUseCases();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerJwt();
builder.Services.AddRefit();

builder.Services.Configure<RouteOptions>(o =>
{
    o.LowercaseQueryStrings = true;
    o.LowercaseUrls = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseDatabaseAlwaysUpToDate();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
