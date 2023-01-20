using Domain.Aggregates.Worlds;
using FluentValidation;
using Infractructure.Repositories;
using Net7MinimalApi.Endpoints;
using Net7MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWorldRepository, WorldRepository>();
builder.Services.AddSingleton<IValidator<WorldModel>, WorldModelValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();

public partial class Program
{
}
