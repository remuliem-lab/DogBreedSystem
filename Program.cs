using Microsoft.EntityFrameworkCore;
using DogBreedSystem.Data;
using DogBreedSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("DogDb"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 2. Render Port Configuration (CRITICAL for deployment)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

app.UseHttpsRedirection();
app.MapControllers();

// 3. API ROUTES

// GET ALL BREEDS
app.MapGet("/api/breeds", async (AppDbContext db) => 
    await db.Breeds.ToListAsync());

// ADD A NEW BREED
app.MapPost("/api/breeds", async (AppDbContext db, Breed breed) => {
    db.Breeds.Add(breed);
    await db.SaveChangesAsync();
    return Results.Created($"/api/breeds/{breed.Id}", breed);
});

// DELETE A BREED BY ID
app.MapDelete("/api/breeds/{id}", async (AppDbContext db, int id) => {
    var breed = await db.Breeds.FindAsync(id);
    if (breed is null) return Results.NotFound();
    db.Breeds.Remove(breed);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// 4. Start the App
app.Run($"http://0.0.0.0:{port}");