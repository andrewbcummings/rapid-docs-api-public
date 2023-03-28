using Microsoft.EntityFrameworkCore;
using rapid_docs_core.Authentication;
using rapid_docs_models.DbModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rapid_docs_models.DataAccess
{
    public class VidaDocsDbContext : DbContext
    {
        private readonly VidaDocsContext ctx;
        public VidaDocsDbContext(DbContextOptions<VidaDocsDbContext> options, VidaDocsContext ctx) : base(options)
        {
            this.ctx = ctx;
        }



        public DbSet<InputField> InputFields { get; set; }
        public DbSet<InputOption> InputOptions { get; set; }
        public DbSet<Signing> Signings { get; set; }
        public DbSet<SigningDocument> SigningDocuments { get; set; }
        public DbSet<SigningForm> SigningForms { get; set; }
        public DbSet<SigningFormPage> SigningFormPages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SigningRecipientMapping> SigningRecipientMappings { get; set; }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyRecipientMapping> SurveyRecipientMappings { get; set; }
        public DbSet<SurveyForm> SurveyForms { get; set; }
        public DbSet<SurveyFormPage> SurveyFormPages { get; set; }
        public DbSet<SurveyInputField> SurveyInputFields { get; set; }
        public DbSet<SurveyInputOption> SurveyInputOptions { get; set; }

        // Overridden the SaveChanges and SaveChangesAsync to update the
        // creation and updation date time whenever an entity is saved.
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                ((BaseModel)entity.Entity).UpdatedDate = DateTime.Now;
                if (this.ctx != null && this.ctx.UserId > 0)
                    ((BaseModel)entity.Entity).UpdatedBy = (int)this.ctx.UserId;
                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreatedDate = DateTime.Now;
                    if (this.ctx != null && this.ctx.UserId > 0)
                        ((BaseModel)entity.Entity).CreatedBy = (int)this.ctx.UserId;
                }
                if (entity.State == EntityState.Modified)
                {
                    entity.Property(nameof(BaseModel.CreatedDate)).IsModified = false;
                    entity.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                ((BaseModel)entity.Entity).UpdatedDate = DateTime.Now;
                if (this.ctx != null && this.ctx.UserId > 0)
                    ((BaseModel)entity.Entity).UpdatedBy = (int)this.ctx.UserId;
                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreatedDate = DateTime.Now;
                    if (this.ctx != null && this.ctx.UserId > 0)
                        ((BaseModel)entity.Entity).CreatedBy = (int)this.ctx.UserId;
                }
                if (entity.State == EntityState.Modified)
                {
                    entity.Property(nameof(BaseModel.CreatedDate)).IsModified = false;
                    entity.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
