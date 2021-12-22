using Model;

namespace Manager.Interface
{
    public interface IUserManager
    {
        bool Register(RegisterModel userData);
    }
}