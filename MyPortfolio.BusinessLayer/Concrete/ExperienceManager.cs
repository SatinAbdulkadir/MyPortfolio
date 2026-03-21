using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class ExperienceManager:IExperienceService
    {
        private readonly IGenericDal<Experience> _experienceDal;

        public ExperienceManager(IGenericDal<Experience> experienceDal)
        {
            _experienceDal = experienceDal;
        }

        public async Task<List<ResultExperienceDto>> TGetExperienceListAsync()
        {
            // Veritabanından ham listeyi çekiyoruz
            var values = await _experienceDal.GetListAsync();

            // Ham listeyi (Entity), kargo paketine (DTO) çeviriyoruz
            return values.Select(x => new ResultExperienceDto
            {
                ExperienceId = x.Id, // BaseEntity'den gelen Id
                Head = x.Head,
                Title = x.Title,
                DatePeriod = x.DatePeriod,
                Description = x.Description
            }).ToList();
        }
    }
}
