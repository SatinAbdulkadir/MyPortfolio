using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyPortfolio.BusinessLayer.Dtos.PortfolioDtos
{
    public class CreatePortfolioDto
    {
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public  string? ImageUrl { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
    }
}
