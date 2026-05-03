using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.MessageDtos;
using MyPortfolio.EntityLayer.Concrete;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMessageDto> _validator;
        private readonly ILogger<HomeController> _logger;
        private readonly ITurnstileService _turnstileService; // Güvenlik Servisi Eklendi

        public HomeController(
            IMessageService messageService,
            IMailService mailService,
            IMapper mapper,
            IValidator<CreateMessageDto> validator,
            ILogger<HomeController> logger,
            ITurnstileService turnstileService)
        {
            _messageService = messageService;
            _mailService = mailService;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _turnstileService = turnstileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(
            [FromBody] CreateMessageDto createMessageDto,
            [FromHeader(Name = "cf-turnstile-response")] string turnstileToken) // Token Header'dan okunur
        {
            // 0. Güvenlik Katmaný: Bot Kontrolü (Fail-Fast)
            bool isHuman = await _turnstileService.VerifyTokenAsync(turnstileToken);
            if (!isHuman)
            {
                _logger.LogWarning("Cloudflare Turnstile doðrulamasý baþarýsýz. Olasý bot engellendi. IP: {Ip}", HttpContext.Connection.RemoteIpAddress);
                return Json(new { success = false, message = "Güvenlik doðrulamasý baþarýsýz oldu. Lütfen sayfayý yenileyip tekrar deneyin." });
            }

            // 1. Validasyon Kontrolü
            var validationResult = await _validator.ValidateAsync(createMessageDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, message = "Lütfen formdaki hatalarý düzeltin.", errors = errors });
            }

            try
            {
                // 2. Önce Veritabanýna Kaydet
                await _messageService.AddMessageAsync(createMessageDto);

                // 3. Mail Servisi Ýçin Veriyi Dönüþtür
                var mailRequest = _mapper.Map<MailRequestDto>(createMessageDto);

                // 4. Asenkron Mail Gönderimi
                await _mailService.SendEmailAsync(mailRequest);

                return Json(new { success = true, message = "Mesajýnýz baþarýyla iletildi. En kýsa sürede dönüþ yapacaðým." });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "AutoMapper DTO dönüþüm hatasý.");
                return Json(new { success = false, message = "Sistemsel bir veri hatasý oluþtu." });
            }
            catch (Exception ex)
            {
                // KESÝN KURAL: Sunucu hatasý UI'a basýlmaz, loglanýr.
                _logger.LogError(ex, "Mesaj iþlenirken kritik bir hata oluþtu. Ziyaretçi maili: {Email}", createMessageDto.Email);

                return Json(new
                {
                    success = false,
                    message = "Mesajýnýz veritabanýna kaydedildi ancak e-posta bildiriminde geçici bir sorun oluþtu."
                });
            }
        }
    }
}