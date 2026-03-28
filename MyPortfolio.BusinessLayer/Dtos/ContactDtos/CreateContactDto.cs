using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.ContactDtos
{
    public class CreateContactDto
    {
        public required string Title { get; set; }       // Örn: Steam, Discord, İkinci Mail
        public required string Description { get; set; } // Örn: "Buradan oyun için ekleyin"
        public string? Email { get; set; }      // Varsa mail
        public string? Phone { get; set; }      // Varsa telefon
        public required string Address { get; set; }
    }
}
