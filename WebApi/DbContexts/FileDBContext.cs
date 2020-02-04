using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Database
{
    public class FileDBContext : DbContext
    {
        public FileDBContext(DbContextOptions<FileDBContext> options)
                : base(options)
        {
        }

        public DbSet<DocFile> DocFile { get; set; }
    }
}
