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
        public DbSet<MaterialProcedure> MaterialProcedures { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<GroupChange> GroupChanges { get; set; }
        public DbSet<Apply> Applies { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        /*
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AttendanceChange> AttendanceChanges { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        */
        public DbSet<Processing> Processings { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FolderFile> FolderFiles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<FunctionGroup> FunctionGroups { get; set; }
        public DbSet<ProfileGroup> ProfileGroups { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonChange> PersonChanges { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<EnquiryGeneral> EnquiryGenerals { get; set; }
        public DbSet<GroupChangeGeneral> GroupChangeGenerals { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Rule> Rules { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().HasOptional(p => p.Cost).WithMany();
            modelBuilder.Entity<Profile>().HasOptional(p => p.CreatedBy).WithMany();
            modelBuilder.Entity<Profile>().HasOptional(p => p.UpdatedBy).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Change).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Certificate).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Experience).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Document).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Document).WithMany();
            modelBuilder.Entity<Group>().HasOptional(p => p.Change).WithMany();
            modelBuilder.Entity<Profile>().HasOptional(p => p.ProfileGroup).WithMany();
            modelBuilder.Entity<Person>().HasOptional(p => p.Emergency).WithMany();
            modelBuilder.Entity<Relationship>().HasOptional(p => p.Owner).WithMany();
            base.OnModelCreating(modelBuilder);
        }
    }
}