using Model;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        bool Register(RegisterModel userData);
    }
}