

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Domain;
using Application;
using InfraStructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var PathDatabase = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "InfraStructure", "Data", "database.db"));
Console.WriteLine(PathDatabase);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={PathDatabase}"));
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();