using OpenData.Domain.Entities;
using System.Data.Entity;

namespace OpenData.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<OpenDataSet> OpenDataSets { get; set; }
        public DbSet<cat_Category> Categories { get; set; }
        //public DbSet<cat_Authority> Authorities { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<Structure> Structures { get; set; }
        //public DbSet<Owner> Owners { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Authority> Authorities { get; set; }
    }
}
