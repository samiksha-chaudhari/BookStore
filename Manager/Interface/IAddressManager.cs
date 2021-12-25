using Model;

namespace Manager.Interface
{
    public interface IAddressManager
    {
        bool AddAddress(AddressModel addressDetails);
        bool EditAddress(AddressModel addressDetails);
    }
}