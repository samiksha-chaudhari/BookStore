using Model;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        string register(RegisterModel userData);
    }
}