using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration["ConnectionStrings:PostgreSQLConnection"] = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__PostgreSQLConnection");

// DbContext Registration
builder.Services.AddDbContext<PersonFinanceSysDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Handlers
builder.Services.AddScoped<RegisterHandler>();


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
