using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IExperienceService
    {
        // Tüm deneyimleri liste olarak getirecek görev
        Task<List<ResultExperienceDto>> TGetExperienceListAsync();
    }
}
