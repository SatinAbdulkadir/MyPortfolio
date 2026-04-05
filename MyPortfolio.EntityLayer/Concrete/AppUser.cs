using Microsoft.AspNetCore.Identity;

namespace MyPortfolio.EntityLayer.Concrete
{
    // IdentityUser'dan miras alıyoruz, ID tipini int yapıyoruz (varsayılan string'dir)
    public class AppUser : IdentityUser<int>
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? ImageUrl { get; set; }

    }

   
}