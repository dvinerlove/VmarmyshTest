using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VmarmyshTest.Filters;
using VmarmyshTest.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("Server = ; Port = 5432; Database = testDB; User Id = postgres; Password = docker1234;");
}, ServiceLifetime.Scoped);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<MyExceptionHandlerMiddleware>();
app.Run();