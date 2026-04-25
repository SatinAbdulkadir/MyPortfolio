using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.ContactDtos
{
    public class CreateContactDto
    {
        public required string Title { get; set; }       
        public required string Description { get; set; } 
        public string? Email { get; set; }     
        public string? Phone { get; set; }     
        public required string Address { get; set; }
    }
}
