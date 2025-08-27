using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

#region Database
string connectionString = builder.Configuration["ConnectionStrings:WineAndFoodDBConnectionString"]!;
var connection = new SqliteConnection(connectionString);
connection.Open();
builder.Services.AddDbContext<WineDBContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));
#endregion


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
#endregion

;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
