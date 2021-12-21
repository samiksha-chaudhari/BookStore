using Model;

namespace Manager.Interface
{
    public interface IUserManager
    {
        string register(RegisterModel userData);
    }
}