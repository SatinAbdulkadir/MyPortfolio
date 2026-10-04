using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.WebUI.Services;

namespace MyPortfolio.WebUI.Controllers
{
    // Arama motorları için robots.txt ve sitemap.xml.
    // Statik dosya yerine controller: sitemap projelerden otomatik üretilir (yeni proje eklenince
    // Google'a kendiliğinden bildirilir), adresler de tek kaynaktan (SeoOptions.BaseUrl) gelir.
    public class SeoController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly SeoOptions _seo;

        public SeoController(IPortfolioService portfolioService, IOptions<SeoOptions> seo)
        {
            _portfolioService = portfolioService;
            _seo = seo.Value;
        }

        [HttpGet("/robots.txt")]
        [ResponseCache(Duration = 86400)]
        public IActionResult Robots()
        {
            // Admin, giriş ve hesap sayfaları taranmaz (ayrıca o sayfalarda noindex etiketi de var)
            var content = new StringBuilder()
                .AppendLine("User-agent: *")
                .AppendLine("Disallow: /Admin")
                .AppendLine("Disallow: /Login")
                .AppendLine("Disallow: /Profile")
                .AppendLine()
                .AppendLine($"Sitemap: {_seo.Absolute("/sitemap.xml")}")
                .ToString();

            return Content(content, "text/plain", Encoding.UTF8);
        }

        [HttpGet("/sitemap.xml")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Sitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            XElement Url(string path, string priority) => new(ns + "url",
                new XElement(ns + "loc", _seo.Absolute(path)),
                new XElement(ns + "priority", priority));

            var urls = new List<XElement>
            {
                Url("/", "1.0"),
                Url("/Certificate/Index", "0.8")
            };

            var projects = await _portfolioService.TGetPortfolioListAsync();
            urls.AddRange(projects.Select(p => Url($"/Portfolio/Detail/{p.Id}", "0.7")));

            var document = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement(ns + "urlset", urls));

            // XDocument.ToString() XML bildirimini yazmaz; Declaration elle başa eklenir
            return Content(document.Declaration + Environment.NewLine + document, "application/xml", Encoding.UTF8);
        }
    }
}
