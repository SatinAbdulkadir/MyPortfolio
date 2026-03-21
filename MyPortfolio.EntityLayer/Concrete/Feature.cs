using MyPortfolio.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.EntityLayer.Concrete
{
    public class Feature:BaseEntity
    {
       
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
