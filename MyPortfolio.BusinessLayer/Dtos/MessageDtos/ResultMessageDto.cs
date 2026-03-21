using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.MessageDtos
{
    public class ResultMessageDto
    {
        public required string NameSurname { get; set; }
        public required string Subject { get; set; }
        public required string Email { get; set; }
        public required string MessageDetail { get; set; }

        public bool IsRead { get; set; }
    }
}
