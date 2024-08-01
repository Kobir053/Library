using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext (DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();
        }

        public DbSet<Library.Models.CategoryModel> CategoryModel { get; set; } = default!;
        public DbSet<Library.Models.ShelfModel> ShelfModel { get; set; } = default!;
        public DbSet<Library.Models.BookModel> BookModel { get; set; } = default!;
    }
}
