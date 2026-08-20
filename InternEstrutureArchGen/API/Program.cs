using Domain;
using Application;
using InfraStructure;
using Microsoft.EntityFrameworkCore;
string PathDB = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "InfraStructure", "Data", "ArchGenExample.db"));
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlite($"Data Source={PathDB}");
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.MapControllers();
app.UseHttpsRedirection();
app.Run();
