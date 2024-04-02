namespace Hospital.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;
    using System.Threading.Tasks;

    using Hospital.Data.Common.Models;
    using Hospital.Data.Models;
    using Hospital.Data.Models.Hospitals;
    using Hospital.Data.Models.Hospitals.People;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Illness> Illnesses { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<IllnessPatient> IllnessPatient { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<ApplicationUser>()
            .HasOne(u => u.Director)
            .WithOne()
            .HasForeignKey<Director>(d => d.Id);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Doctor)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.Id);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Patient)
                .WithOne()
                .HasForeignKey<Patient>(p => p.Id);

            // Configure Clinic to Doctor relationship
            builder.Entity<Department>()
                .HasMany(c => c.Doctors)
                .WithOne(d => d.Department)
                .HasForeignKey(d => d.DepartmentId);

            // Configure Clinic to Boss relationship
            builder.Entity<Department>()
                .HasOne(c => c.Boss)
                .WithOne()
                .HasForeignKey<Department>(c => c.BossId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Doctor to Department relationship
            builder.Entity<Doctor>()
                .HasOne(d => d.Department)
                .WithMany(dept => dept.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .IsRequired(false); // Adjust as per your requirements

            builder.Entity<Hospital>()
                .HasOne(h => h.Director)
                .WithOne(h => h.Hospital)
                .HasForeignKey<Hospital>(h => h.DirectorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<IllnessPatient>().HasKey(x => new { x.PatientId, x.IllnessId });
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
