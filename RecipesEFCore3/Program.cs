using Microsoft.EntityFrameworkCore;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using RecipesEFCore3.Models;
using RecipesEFCore3.Endpoints;
using RecipesEFCore3.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connString = builder.Configuration.GetConnectionString("Data");

builder.Services.AddDbContext<RecipesEFCoreDbContext>(options =>
    options.UseNpgsql(connString)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.MapRecipeEndpoints();

app.Run();