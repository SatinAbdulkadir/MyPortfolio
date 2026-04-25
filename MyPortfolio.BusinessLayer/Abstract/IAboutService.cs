using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IAboutService
    {
        
        Task<ResultAboutDto> TGetAboutAsync();

        
        Task TUpdateAboutAsync(UpdateAboutDto updateAboutDto);

        
        Task TChangeAboutStatusAsync(int id);
    }
}
