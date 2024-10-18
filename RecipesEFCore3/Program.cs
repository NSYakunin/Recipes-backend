using Microsoft.EntityFrameworkCore;
using RecipesEFCore.DataAccess.SQLServer;
using System;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("Data");

builder.Services.AddDbContext<RecipesEFCoreDbContext>(options => options.UseSqlServer(connString));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
