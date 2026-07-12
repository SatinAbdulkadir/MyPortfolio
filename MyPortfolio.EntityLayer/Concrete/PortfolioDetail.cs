using MyPortfolio.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.EntityLayer.Concrete
{
    public class PortfolioDetail : BaseEntity
    {
        public required int PortfolioId { get; set; }
        public required string DetailDescription { get; set; }

        // Virgülle ayrılmış liste: "Unity, C#, Blender"
        public string? Technologies { get; set; }

        // Yüklenen mp4 yolu (/videos/x.mp4) veya YouTube linki; boşsa video bölümü görünmez
        public string? VideoUrl { get; set; }
    }
}
