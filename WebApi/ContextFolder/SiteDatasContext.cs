using Microsoft.EntityFrameworkCore;

namespace WebApi.ContextFolder
{
    public class SiteDatasContext : DbContext
    {
        public DbSet<Project>? ProjectsEnt { get; set; }
        public DbSet<Blog>? BlogsEnt { get; set; }
        public DbSet<Service>? ServicesEnt { get; set; }
        public DbSet<RequestMessage>? RequestEnt { get; set; }
        public DbSet<Contact>? ContactsEnt { get; set; }
        public DbSet<Motto>? MottosEnt { get; set; }

        public SiteDatasContext()
        {
            Database.EnsureCreated();
            MottosEnt.Load();
            ProjectsEnt.Load();
            BlogsEnt.Load();
            ServicesEnt.Load();
            RequestEnt.Load();
            ContactsEnt.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DatasConnection");
            var options = optionsBuilder.UseSqlServer(connectionString)
            .Options;
        }
    }
}

