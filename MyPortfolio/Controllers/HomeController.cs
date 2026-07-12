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
        private readonly ITurnstileService _turnstileService; // G�venlik Servisi Eklendi

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
            // 0. G�venlik Katman�: Bot Kontrol� (Fail-Fast)
            bool isHuman = await _turnstileService.VerifyTokenAsync(turnstileToken);
            if (!isHuman)
            {
                _logger.LogWarning("Cloudflare Turnstile do�rulamas� ba�ar�s�z. Olas� bot engellendi. IP: {Ip}", HttpContext.Connection.RemoteIpAddress);
                return Json(new { success = false, message = "G�venlik do�rulamas� ba�ar�s�z oldu. L�tfen sayfay� yenileyip tekrar deneyin." });
            }

            // 1. Validasyon Kontrol�
            var validationResult = await _validator.ValidateAsync(createMessageDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, message = "L�tfen formdaki hatalar� d�zeltin.", errors = errors });
            }

            try
            {
                // 2. �nce Veritaban�na Kaydet
                await _messageService.AddMessageAsync(createMessageDto);

                // 3. Mail Servisi ��in Veriyi D�n��t�r
                var mailRequest = _mapper.Map<MailRequestDto>(createMessageDto);

                // 4. Asenkron Mail G�nderimi
                await _mailService.SendEmailAsync(mailRequest);

                return Json(new { success = true, message = "Mesaj�n�z ba�ar�yla iletildi. En k�sa s�rede d�n�� yapaca��m." });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "AutoMapper DTO d�n���m hatas�.");
                return Json(new { success = false, message = "Sistemsel bir veri hatas� olu�tu." });
            }
            catch (Exception ex)
            {
                // KES�N KURAL: Sunucu hatas� UI'a bas�lmaz, loglan�r.
                _logger.LogError(ex, "Mesaj i�lenirken kritik bir hata olu�tu. Ziyaret�i maili: {Email}", createMessageDto.Email);

                return Json(new
                {
                    success = false,
                    message = "Mesaj�n�z veritaban�na kaydedildi ancak e-posta bildiriminde ge�ici bir sorun olu�tu."
                });
            }
        }
    }
}