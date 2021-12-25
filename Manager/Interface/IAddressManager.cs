using Model;
using System.Collections.Generic;

namespace Manager.Interface
{
    public interface IAddressManager
    {
        bool AddAddress(AddressModel addressDetails);
        bool EditAddress(AddressModel addressDetails);
        List<AddressModel> GetUserAddress(int userId);
    }
}