using Microsoft.EntityFrameworkCore;
using CMSAPI.Models;

namespace CMSAPI.Data
{
    // DbContext class for the CMS API
    public class CMSAPIDbContext : DbContext
    {
        // Constructor to pass options to the base DbContext class
        public CMSAPIDbContext(DbContextOptions<CMSAPIDbContext> options) : base(options) { }

        // DbSet properties for each model class to represent database tables
        public DbSet<User> Users { get; set; } // Table for users
        public DbSet<Document> Documents { get; set; } // Table for documents
        public DbSet<Folder> Folders { get; set; } // Table for folders
        public DbSet<ContentType> ContentTypes { get; set; } // Table for content types

        // Method to configure the relationships and constraints between models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring one-to-many relationship between Folder and its subfolders
            modelBuilder.Entity<Folder>()
                .HasMany(f => f.SubFolders) // A folder can have many subfolders
                .WithOne(f => f.ParentFolder) // Each subfolder has one parent folder
                .HasForeignKey(f => f.ParentFolderId); // Foreign key for the parent folder

            // Configuring one-to-many relationship between Folder and Document
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Folder) // Each document belongs to one folder
                .WithMany(f => f.Documents) // A folder can have many documents
                .HasForeignKey(d => d.FolderId); // Foreign key for the folder
        }
    }
}