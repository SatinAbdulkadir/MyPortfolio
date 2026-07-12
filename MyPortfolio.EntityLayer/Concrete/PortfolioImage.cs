using MyPortfolio.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.EntityLayer.Concrete
{
    public class PortfolioImage : BaseEntity
    {
        public required int PortfolioDetailId { get; set; }
        public required string ImageUrl { get; set; }
    }
}
