using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.AppUserDtos
{
    public class EditProfileDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public string? ImageUrl { get; set; }
        public string? Password { get; set; } // Şifre değişecekse dolar
        public string? ConfirmPassword { get; set; }
        public  string? CurrentPassword { get; set; }
    }
}
