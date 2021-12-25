using Microsoft.Extensions.Configuration;
using Model;

namespace Repository.Interface
{
    public interface IAddressRepository
    {
        bool AddAddress(AddressModel addressDetails);
        bool EditAddress(AddressModel addressDetails);
    }
}