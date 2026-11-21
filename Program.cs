using Microsoft.EntityFrameworkCore;
using Testing.Data;
using Testing.Entities;
using Testing.Business;
using Testing.Controllers;
using Testing.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//data
builder.Services.AddScoped<CanchaData>();
builder.Services.AddScoped<ClienteData>();
builder.Services.AddScoped<TurnoData>();
//services
builder.Services.AddScoped<CanchaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<TurnoService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
