using MyPortfolio.BusinessLayer.Dtos.ContactDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IContactService
    {
        // Sadece ilk kaydı getirir (Liste değil!)
        Task<ResultContactDto> TGetContactAsync(); 
        
        // Sadece ilk kaydı günceller
        Task TUpdateContactAsync(UpdateContactDto updateContactDto);
    }
}
