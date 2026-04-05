using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.SkillDtos
{
    public class ResultSkillDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public  int? Value { get; set; }

        public string? Icon { get; set; }

    }
}
