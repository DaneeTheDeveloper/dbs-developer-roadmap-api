global using DeveloperRoadmapApi.DatabaseContext;
global using DeveloperRoadmapApi.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapGet("/", () => "Welcome to minimal APIs");

app.MapGet("/api/getLanguages", async (ApplicationDbContext context) => await context.Languages.ToListAsync());

//Get Todo Items by id
app.MapGet("/api/getLanguageById/{id}", async (ApplicationDbContext context, int id) => 
    await context.Languages.FindAsync(id) is Language language ? Results.Ok(language) : Results.NotFound("Language not found ./"));

//Create Todo Items 
app.MapPost("/api/addLanguage", async (ApplicationDbContext context, Language language) =>
{
    context.Languages.Add(language);
    await context.SaveChangesAsync();
    return Results.Ok(language);
});

//Updating Todo Items
app.MapPut("/api/updateLanguage/{id}", async (ApplicationDbContext context, Language language, int id) =>
{
    var languageFromDb = await context.Languages.FindAsync(id);
    
    if (languageFromDb != null)
    {
        languageFromDb.Id = language.Id;
        languageFromDb.Name = language.Name;
        
        await context.SaveChangesAsync();
        return Results.Ok(language);
    }
    return Results.NotFound("Language not found");
});


//Deleting Todo Items
app.MapDelete("/api/deleteLanguage/{id}", async (ApplicationDbContext context, int id) =>
{
    var languageFromDb = await context.Languages.FindAsync(id);

    if (languageFromDb != null)
    {
        context.Remove(languageFromDb);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound("Language not found");
});
app.Run();
