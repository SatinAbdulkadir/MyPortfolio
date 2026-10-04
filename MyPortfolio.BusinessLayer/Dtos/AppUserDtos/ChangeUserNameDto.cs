namespace MyPortfolio.BusinessLayer.Dtos.AppUserDtos
{
    public class ChangeUserNameDto
    {
        public string? NewUserName { get; set; }

        // Kullanıcı adı da giriş bilgisidir: oturumu açık bırakılmış bir bilgisayarda
        // başkası değiştiremesin diye mevcut şifreyle onaylanır
        public string? CurrentPassword { get; set; }
    }
}
