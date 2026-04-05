using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos
{
    public class CreateSocialMediaDto
    {
       
        public required string Title { get; set; }
        public required string Url { get; set; }
        public required string Icon { get; set; }
    }
}
