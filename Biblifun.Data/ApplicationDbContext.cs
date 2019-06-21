// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using Biblifun.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Biblifun.Data.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblifun.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public string CurrentUserId { get; set; }

        public DbSet<BibleBook> BibleBooks { get; set; }

        public DbSet<BibleBookName> BibleBookNames { get; set; }

        public DbSet<BibleChapter> BibleChapter { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ScriptureSet> ScriptureSets { get; set; }

        public DbSet<ScriptureSetItem> ScriptureSetItems { get; set; }

        public DbSet<ScriptureSetCategory> ScriptureSetCategories { get; set; }

        public DbSet<ScriptureSetItemCategory> ScriptureSetItemCategories { get; set; }

        public DbSet<VerseCache> CachedVerses { get; set; }

        public DbSet<VerseSummary> VerseSummaries { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            // disable identity column on BibleBook table
            builder.Entity<BibleBook>().Property(bb => bb.BibleBookId)
                   .ValueGeneratedNever();

            // ScriptureSetCategory 
            builder.Entity<ScriptureSetCategory>()
                   .HasKey(ss => new { ss.ScriptureSetId, ss.CategoryId });
            builder.Entity<ScriptureSetCategory>()
                   .HasOne(ssc => ssc.ScriptureSet)
                   .WithMany(ss => ss.ScriptureSetCategories)
                   .HasForeignKey(ssc => ssc.ScriptureSetId);
            builder.Entity<ScriptureSetCategory>()
                   .HasOne(ssc => ssc.Category)
                   .WithMany(c => c.ScriptureSetCategories)
                   .HasForeignKey(ssc => ssc.CategoryId);

            // ScriptureSetItemCategory 
            builder.Entity<ScriptureSetItemCategory>()
                   .HasKey(ss => new { ss.ScriptureSetItemId, ss.CategoryId });
            builder.Entity<ScriptureSetItemCategory>()
                   .HasOne(ssc => ssc.ScriptureSetItem)
                   .WithMany(ss => ss.ScriptureSetItemCategories)
                   .HasForeignKey(ssc => ssc.ScriptureSetItemId);
            builder.Entity<ScriptureSetItemCategory>()
                   .HasOne(ssc => ssc.Category)
                   .WithMany(c => c.ScriptureSetItemCategories)
                   .HasForeignKey(ssc => ssc.CategoryId);

        }


        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
