using Microsoft.EntityFrameworkCore;
using DogBreedSystem.Models;

namespace DogBreedSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Breed> Breeds { get; set; }
}