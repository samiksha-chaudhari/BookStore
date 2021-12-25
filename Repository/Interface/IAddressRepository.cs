using Microsoft.Extensions.Configuration;
using Model;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IAddressRepository
    {
        bool AddAddress(AddressModel addressDetails);
        bool EditAddress(AddressModel addressDetails);
        List<AddressModel> GetUserAddress(int userId);
    }
}