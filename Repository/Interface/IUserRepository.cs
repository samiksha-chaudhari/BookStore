using Model;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        bool Register(RegisterModel userData);
        string Login(LoginModel login);
        string GenerateToken(string userName);
        string ForgotPassword(string Email);
        bool ResetPassword(ResetPasswordModel resetPassword);
    }
}