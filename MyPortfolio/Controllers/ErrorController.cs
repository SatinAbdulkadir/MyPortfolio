using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Controllers
{
    // Hata sayfaları herkese açık olmalı; aksi halde 403 sayfası bile yetki ister.
    // IgnoreAntiforgeryToken şart: POST isteği hata alınca (örn. 429) StatusCodePages
    // isteği buraya AYNI metodla (POST) yeniden yürütür; global CSRF filtresi token
    // bulamayıp 400 basar ve asıl hata kodunu ezerdi. Hata sayfası işlem yapmaz, token gerekmez.
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class ErrorController : Controller
    {
        // UseStatusCodePagesWithReExecute("/Error/{0}") ve UseExceptionHandler("/Error/500")
        // buraya yönlenir. Sayfa DB'ye dokunmaz; veritabanı çökse bile hata sayfası açılır.
        [Route("Error/{statusCode:int}")]
        public IActionResult Index(int statusCode)
        {
            var (title, message) = statusCode switch
            {
                404 => ("Sayfa Bulunamadı", "Aradığın sayfa taşınmış, silinmiş ya da hiç var olmamış olabilir. Adresi kontrol edip tekrar dene."),
                403 => ("Erişim Engellendi", "Bu sayfayı görüntüleme yetkin bulunmuyor."),
                429 => ("Çok Fazla İstek", "Kısa sürede çok fazla istek gönderildi. Lütfen bir dakika bekleyip tekrar dene."),
                500 => ("Sunucu Hatası", "Beklenmeyen bir hata oluştu. Sorun kayıt altına alındı, en kısa sürede düzeltilecek."),
                _ => ("Bir Şeyler Ters Gitti", "Beklenmeyen bir durum oluştu. Ana sayfaya dönüp tekrar deneyebilirsin.")
            };

            ViewBag.StatusCode = statusCode;
            ViewBag.ErrorTitle = title;
            ViewBag.ErrorMessage = message;

            Response.StatusCode = statusCode;
            return View("Index");
        }
    }
}
