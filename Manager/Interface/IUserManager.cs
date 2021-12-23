using Model;

namespace Manager.Interface
{
    public interface IUserManager
    {
        bool Register(RegisterModel userData);
        string Login(LoginModel login);
        string GenerateToken(string userName);
        string ForgotPassword(string Email);
        bool ResetPassword(ResetPasswordModel resetPassword);
    }
}