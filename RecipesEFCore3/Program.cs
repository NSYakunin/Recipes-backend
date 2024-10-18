using Microsoft.EntityFrameworkCore;
using RecipesEFCore.DataAccess.SQLServer;
using RecipesEFCore3;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();


var connString = builder.Configuration.GetConnectionString("Data");

builder.Services.AddDbContext<RecipesEFCoreDbContext>(options => options.UseSqlServer(connString));
builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();
var app = builder.Build();

app.UseExceptionHandler();


app.MapGet("/", () => "Hello World!");

app.MapPost("/Recipe", async (Recipe recipe, RecipesEFCoreDbContext db) =>
{

    await db.Recipes.AddAsync(recipe);
    await db.SaveChangesAsync();
    return recipe;
});

app.Run();

