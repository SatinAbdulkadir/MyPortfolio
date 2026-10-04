using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.DataAccessLayer.Context
{
    public class MyPortfolioContext : IdentityDbContext<AppUser, AppRole, int>
    {
       
        public MyPortfolioContext(DbContextOptions<MyPortfolioContext> options) : base(options)
        {
        }

      
        public MyPortfolioContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=MyPortfolioDb;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity tabloları için zorunlu: önce temel yapılandırma
            base.OnModelCreating(builder);

            // Projede navigation property kullanılmıyor; ilişkiler yine de veritabanında
            // FK olarak tanımlanır ki ara tabloda sahipsiz kayıt kalamasın.
            builder.Entity<CertificateCategoryLink>(link =>
            {
                // Aynı sertifika aynı kategoriye iki kez bağlanamaz
                link.HasIndex(x => new { x.CertificateId, x.CategoryId }).IsUnique();

                // Sertifika silinince bağlantıları da gider
                link.HasOne<Certificate>().WithMany()
                    .HasForeignKey(x => x.CertificateId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Dolu kategori silinemez (manager da engelliyor, bu son savunma hattı)
                link.HasOne<CertificateCategory>().WithMany()
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Alt kategorisi olan ana kategori silinemez
            builder.Entity<CertificateCategory>()
                .HasOne<CertificateCategory>().WithMany()
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateCategory> CertificateCategories { get; set; }
        public DbSet<CertificateCategoryLink> CertificateCategoryLinks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioDetail> PortfolioDetails { get; set; }
        public DbSet<PortfolioImage> PortfolioImages { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}