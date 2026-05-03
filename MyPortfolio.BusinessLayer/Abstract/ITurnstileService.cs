namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ITurnstileService
    {
        Task<bool> VerifyTokenAsync(string token);
    }
}