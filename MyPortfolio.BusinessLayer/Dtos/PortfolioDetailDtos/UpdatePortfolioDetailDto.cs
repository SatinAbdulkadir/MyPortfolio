using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos
{
    public class UpdatePortfolioDetailDto : IPortfolioDetailFormDto
    {
        public required int Id { get; set; }
        public required int PortfolioId { get; set; }
        public required string DetailDescription { get; set; }
        public string? Technologies { get; set; }
        public string? VideoUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? VideoFile { get; set; }

        [DataType(DataType.Upload)]
        public List<IFormFile>? GalleryFiles { get; set; }
    }
}
