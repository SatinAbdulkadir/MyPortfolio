using Microsoft.AspNetCore.Mvc.Filters;
using MyPortfolio.WebUI.Services;

namespace MyPortfolio.WebUI.Filters
{
    // Herhangi bir Admin* controller'ına başarılı bir POST gelirse içerik değişmiş demektir:
    // cache sürüm damgasını yeniler. Global filtre olduğu için gelecekte eklenecek
    // admin ekranları da otomatik kapsanır — tek tek controller'lara kod eklenmez.
    public class AdminContentCacheBustFilter : IActionFilter
    {
        private readonly ContentCacheVersion _cacheVersion;

        public AdminContentCacheBustFilter(ContentCacheVersion cacheVersion)
        {
            _cacheVersion = cacheVersion;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null) return;

            var isPost = HttpMethods.IsPost(context.HttpContext.Request.Method);
            var isAdmin = context.Controller.GetType().Name.StartsWith("Admin", StringComparison.Ordinal);

            if (isPost && isAdmin)
            {
                _cacheVersion.Bump();
            }
        }
    }
}
