using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class AboutManager :IAboutService
    {
        private readonly IGenericDal<About> _aboutDal;
        public AboutManager(IGenericDal<About> aboutDal)
        {
            _aboutDal = aboutDal;
        }

        // UI Tarafı İçin: Hakkımda bilgisini getirir
        public async Task<ResultAboutDto> TGetAboutAsync()
        {
            var values = await _aboutDal.GetListAsync();
            var data = values.FirstOrDefault(); // Genelde tek bir "About" kaydı olur

            if (data != null)
            {
                // Manuel Mapping (AutoMapper gelene kadar böyle devam)
                return new ResultAboutDto
                {
                    AboutId = data.Id,
                    Title = data.Title,
                    SubDescription = data.SubDescription,
                    Details = data.Details
                };
            }
            return null; // Veri yoksa null dönmek iyidir, UI katmanında kontrol ederiz
        }

        // Admin Paneli İçin: Hakkımda bilgisini günceller
        public async Task TUpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            // Önce mevcut kaydı veritabanından çekelim (Veya direkt DTO'dan Entity yapalım)
            var existingData = await _aboutDal.GetByIdAsync(updateAboutDto.AboutId);

            if (existingData != null)
            {
                existingData.Title = updateAboutDto.Title;
                existingData.SubDescription = updateAboutDto.SubDescription;
                existingData.Details = updateAboutDto.Details;

                await _aboutDal.UpdateAsync(existingData);
            }
        }

        // Statü Değiştirme (Gerekirse Aktif/Pasif Yapmak İçin)
        public async Task TChangeAboutStatusAsync(int id)
        {
            var data = await _aboutDal.GetByIdAsync(id);
            if (data != null)
            {
                data.IsActive = !data.IsActive; // Durumu tersine çevir
                await _aboutDal.UpdateAsync(data);
            }
        }
}
}
