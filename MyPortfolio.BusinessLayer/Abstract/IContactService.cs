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
        Task<ResultContactDto> TGetContactAsync();
    }
}
