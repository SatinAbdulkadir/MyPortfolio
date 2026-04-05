using MyPortfolio.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.EntityLayer.Concrete
{
    public class Skill:BaseEntity
    {
        public required string Title { get; set; }
        public   int?  Value { get; set; }

        public string? Icon { get; set; }

    }
}
