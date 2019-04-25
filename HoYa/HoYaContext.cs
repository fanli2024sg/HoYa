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

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<PersonChange> PersonChanges { get; set; }
        public DbSet<PersonDefinition> PersonDefinitions { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<RecipeChange> RecipeChanges { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FolderFile> FolderFiles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StepGroup> StepGroups { get; set; }
        public DbSet<FunctionGroup> FunctionGroups { get; set; }
        public DbSet<ProfileGroup> ProfileGroups { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<InquiryGeneral> InquiryGenerals { get; set; }
        public DbSet<Rule> Rules { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelInstanceer)
        {
            modelInstanceer.Entity<Folder>().HasOptional(p => p.CreatedBy).WithMany();
            modelInstanceer.Entity<Option>().HasOptional(p => p.CreatedBy).WithMany();
            modelInstanceer.Entity<PersonChange>().HasOptional(p => p.CreatedBy).WithMany();
            modelInstanceer.Entity<Person>().HasOptional(p => p.CreatedBy).WithMany();
            modelInstanceer.Entity<Option>().HasOptional(p => p.Parent).WithMany();
            base.OnModelCreating(modelInstanceer);
        }

        [Authorize]
        public override async Task<int> SaveChangesAsync()
        {
            string userId = HttpContext.Current.User.Identity.Name;
            Guid profileId = (await this.Profiles.FirstOrDefaultAsync(x => x.UserId == userId)).Id;
            ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList().ForEach(e =>
            {
                foreach (var property in e.Entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime?)))
                {
                    if (property.GetValue(e.Entity) == null) continue;
                    property.SetValue(e.Entity, ((DateTime)property.GetValue(e.Entity)).ToLocalTime());
                }
                e.Entity.GetType().GetProperty("CreatedById").SetValue(e.Entity, profileId);
            });

            ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList().ForEach(e =>
            {
                foreach (var property in e.Entity.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime?)))
                {
                    if (property.GetValue(e.Entity) == null) continue;
                    property.SetValue(e.Entity, ((DateTime)property.GetValue(e.Entity)).ToLocalTime());
                }
                if (e.Entity.GetType().GetProperty("ArchivedById") != null) e.Entity.GetType().GetProperty("ArchivedById").SetValue(e.Entity, profileId);
            });




            return base.SaveChanges();
        }
    }
}