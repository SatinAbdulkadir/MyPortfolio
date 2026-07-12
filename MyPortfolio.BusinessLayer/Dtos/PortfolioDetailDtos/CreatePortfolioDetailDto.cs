using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos
{
    public class CreatePortfolioDetailDto : IPortfolioDetailFormDto
    {
        public required int PortfolioId { get; set; }
        public required string DetailDescription { get; set; }

        // Virgülle ayrılmış liste: "Unity, C#, Blender"
        public string? Technologies { get; set; }

        // Yüklenen mp4 yolu veya YouTube linki
        public string? VideoUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? VideoFile { get; set; }

        // Galeri: birden fazla resim tek formdan yüklenebilir
        [DataType(DataType.Upload)]
        public List<IFormFile>? GalleryFiles { get; set; }
    }
}
