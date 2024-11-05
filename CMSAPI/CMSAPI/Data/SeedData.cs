using Microsoft.EntityFrameworkCore;
using CMSAPI.Models;

namespace CMSAPI.Data
{
    public static class SeedData
    {
        // Method to initialize the database with sample data
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new CMSAPIDbContext(
                       serviceProvider.GetRequiredService<DbContextOptions<CMSAPIDbContext>>());

            // Check if the Users table already has data
            if (context.Users.Any())
            {
                return; // Database has already been seeded
            }

            // Seed ContentTypes
            var contentTypes = new[]
            {
                new ContentType { Type = "Text" },
                new ContentType { Type = "Url" }
            };

            context.ContentTypes.AddRange(contentTypes);

            // Create a sample user
            var user = new User
            {
                Username = "testuser",
                Password = "password",
                Email = "test@example.com"
            };

            // Create a root folder associated with the sample user
            var folder = new Folder
            {
                Name = "Root",
                User = user
            };

            // Create a sample document associated with the user and folder
            var document = new Document
            {
                Title = "Sample Doc",
                Content = "This is a sample document.",
                ContentType = contentTypes[0].Type, // Use the "Text" ContentType
                CreatedDate = DateTime.Now,
                User = user,
                Folder = folder
            };

            // Add the sample user, folder, document, and content types to the context
            context.Users.Add(user);
            context.Folders.Add(folder);
            context.Documents.Add(document);

            // Save the changes to the database
            context.SaveChanges();
        }
    }
}
