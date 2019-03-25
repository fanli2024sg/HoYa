using HoYa.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;

namespace HoYa
{
    public class HoYaContext : IdentityDbContext<IdentityUser>
    {
        public HoYaContext()
            : base("HoYaContext")
        {

        }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Mission> Missions { get; set; }
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
        public DbSet<FunctionGroup> FunctionGroups { get; set; }
        public DbSet<ProfileGroup> ProfileGroups { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<EnquiryGeneral> EnquiryGenerals { get; set; }
        public DbSet<Rule> Rules { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>().HasOptional(p => p.CreatedBy).WithMany();
            modelBuilder.Entity<Profile>().HasOptional(p => p.CreatedBy).WithMany();
            modelBuilder.Entity<Change>().HasOptional(p => p.CreatedBy).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Change).WithMany();
            modelBuilder.Entity<Group>().HasOptional(p => p.Change).WithMany();
            base.OnModelCreating(modelBuilder);
        }
    }
}