namespace DotNetCore.Web.Models.Account
{
    public class AuthModel
    {
        public LoginModel LoginModel { get; set; } = new LoginModel();

        public RegisterModel RegisterModel { get; set; } = new RegisterModel();
    }
}
