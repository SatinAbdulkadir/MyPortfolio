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
        // 1. Program.cs'den gelecek ayarları karşılayan kapı (Eklendi)
        public MyPortfolioContext(DbContextOptions<MyPortfolioContext> options) : base(options)
        {
        }

        // 2. Tasarım araçları (Migration) için boş constructor (Eklendi)
        public MyPortfolioContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Bağlantı cümlesindeki olası hatalı karakterler temizlendi
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=MyPortfolioDb;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}