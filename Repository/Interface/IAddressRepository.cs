using Microsoft.Extensions.Configuration;
using Model;

namespace Repository.Interface
{
    public interface IAddressRepository
    {
        IConfiguration Configuration { get; }

        bool AddAddress(AddressModel addressDetails);
    }
}