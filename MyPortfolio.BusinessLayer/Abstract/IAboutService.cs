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
        // UI Katmanı için: Sadece "Hakkımda" verisini getirir.
        // Genellikle tek bir kayıt olduğu için List değil, direkt nesne döner.
        Task<ResultAboutDto> TGetAboutAsync();

        // Admin Paneli için: Bilgileri günceller.
        Task TUpdateAboutAsync(UpdateAboutDto updateAboutDto);

        // Eğer sistemde birden fazla Hakkımda yazısı olup 
        // biri aktif edilecekse bu da gerekebilir:
        Task TChangeAboutStatusAsync(int id);
    }
}
