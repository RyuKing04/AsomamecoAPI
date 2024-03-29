using Microsoft.EntityFrameworkCore;
using AsomamecoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(op => { op.AddPolicy("Cors", cors => { cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AsomamecoContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("Cors");

app.MapControllers();

app.Run();

