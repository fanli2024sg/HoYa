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
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HoYa
{
    public class HoYaContext : IdentityDbContext<IdentityUser>
    {
        public HoYaContext()
            : base("HoYaContext")
        {
            Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);
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
        public DbSet<Activity> Activities { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProfileGroup> ProfileGroups { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<InventoryGroup> InventoryGroups { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
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
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Input> Inputs { get; set; }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<WorkPlan> WorkPlans { get; set; }

        public DbSet<WorkPlanRecord> WorkPlanRecords { get; set; }

        public DbSet<WorkOrder> WorkOrders { get; set; }

        public DbSet<WorkEvent> WorkEvents { get; set; }

        public DbSet<WorkEventInventory> WorkEventInventories { get; set; }


        public DbSet<StationRecord> StationRecords { get; set; }















        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(38, 18)); 
            modelBuilder.Entity<WorkOrder>().HasOptional(p => p.Start).WithMany();
            modelBuilder.Entity<WorkOrder>().HasOptional(p => p.End).WithMany();
            modelBuilder.Entity<Item>().HasOptional(p => p.Recipe).WithMany();
            modelBuilder.Entity<Participate>().HasOptional(p => p.Owner).WithMany();
            modelBuilder.Entity<WorkPlan>().HasOptional(p => p.WorkPlanRecord).WithMany();
            modelBuilder.Entity<Process>().HasOptional(p => p.Previous).WithMany();
            modelBuilder.Entity<Activity>().HasOptional(p => p.Previous).WithMany();
            modelBuilder.Entity<Activity>().HasOptional(p => p.ArchivedParticipate).WithMany();
            modelBuilder.Entity<Option>().HasOptional(p => p.CreatedBy).WithMany();
            modelBuilder.Entity<Process>().HasOptional(p => p.Current).WithMany();
            modelBuilder.Entity<Option>().HasOptional(p => p.Parent).WithMany();
            modelBuilder.Entity<Inventory>().HasOptional(p => p.Item).WithMany();
            modelBuilder.Entity<Item>().HasOptional(p => p.Recipe).WithMany();

            base.OnModelCreating(modelBuilder);
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