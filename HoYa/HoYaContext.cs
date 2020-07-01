using HoYa.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Threading;

namespace HoYa
{
    public class HoYaContext : IdentityDbContext<IdentityUser>
    {
        public HoYaContext()
            : base("HoYaContext")
        {
            Database.Log =(log)=> System.Diagnostics.Debug.WriteLine(log);
        }
        public DbSet<StepRelationship> StepRelationships { get; set; }
        public DbSet<Participate> Participates { get; set; }
        public DbSet<Judgment> Judgments { get; set; }
        public DbSet<RedirectCondition> RedirectConditions { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Redirect> Redirects { get; set; }
        public DbSet<Relationship> Relationships { get; set; } 
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FolderFile> FolderFiles { get; set; }
        public DbSet<StepGroup> StepGroups { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Segmentation> Segmentations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryAttribute> InventoryAttributes { get; set; }
        public DbSet<Entities.Attribute> Attributes { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<ItemAttribute> ItemAttributes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelInstanceer)
        {
            modelInstanceer.Entity<Participate>().HasOptional(p => p.Owner).WithMany(); 
            modelInstanceer.Entity<Process>().HasOptional(p => p.Previous).WithMany();
            modelInstanceer.Entity<Activity>().HasOptional(p => p.Previous).WithMany();
            modelInstanceer.Entity<Activity>().HasOptional(p => p.ArchivedParticipate).WithMany(); 
            modelInstanceer.Entity<Option>().HasOptional(p => p.CreatedBy).WithMany();
            modelInstanceer.Entity<Process>().HasOptional(p => p.Current).WithMany();
            modelInstanceer.Entity<Option>().HasOptional(p => p.Parent).WithMany();
            modelInstanceer.Entity<Inventory>().HasOptional(p => p.Item).WithMany();
            modelInstanceer.Entity<Recipe>().HasOptional(p => p.CreatedBy).WithMany(); 
            base.OnModelCreating(modelInstanceer);
        }

        [Authorize]
        public override async Task<int> SaveChangesAsync()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                string userId = HttpContext.Current.User.Identity.Name;
                Guid? inventoryId = (await this.Inventories.FirstOrDefaultAsync(x => x.UserId == userId))?.Id;
                if (inventoryId != null)
                {
                    ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList().ForEach(e =>
                    {
                        foreach (var property in e.Entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime?)))
                        {
                            if (property.GetValue(e.Entity) == null) continue;
                            property.SetValue(e.Entity, ((DateTime)property.GetValue(e.Entity)).ToLocalTime());
                        }
                        e.Entity.GetType().GetProperty("CreatedById").SetValue(e.Entity, inventoryId);
                    });

                    ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList().ForEach(e =>
                    {
                        foreach (var property in e.Entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime?)))
                        {
                            if (property.GetValue(e.Entity) == null) continue;
                            property.SetValue(e.Entity, ((DateTime)property.GetValue(e.Entity)).ToLocalTime());
                        }
                        if (e.Entity.GetType().GetProperty("ArchivedById") != null)
                        {
                            if (e.Entity.GetType().GetProperty("ArchivedDate").GetValue(e.Entity) != null) e.Entity.GetType().GetProperty("ArchivedById").SetValue(e.Entity, inventoryId);

                        }
                    });
                }
            }
            return base.SaveChanges();
        }
    }
}