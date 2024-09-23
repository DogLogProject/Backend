using ApiProject.Model.DogModel;
using ApiProject.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<Dog> Dogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DogPhoto> DogPhotos { get; set; }

        

    }
}
