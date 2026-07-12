using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.PortfolioImageDtos
{
    public class ResultPortfolioImageDto
    {
        public required int Id { get; set; }
        public required int PortfolioDetailId { get; set; }
        public required string ImageUrl { get; set; }
    }
}
