using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teleferic_commerce_core.DTO.Address;
using teleferic_commerce_infrastructure.Models;

namespace teleferic_commerce_core.ApplicationServices.Interfaces
{
    public interface IAddressService
    {
        Task<ResponseModel<AddressDTO>> CreateAddressAsync(CreateAddressDTO createAddressDTO);
        Task<ResponseModel<bool>> UpdateAddressAsync(Guid id, UpdateAddressDTO updateAddressDTO);
        Task<ResponseModel<bool>> DeleteAddressAsync(Guid id);
        Task<ResponseModel<AddressDTO>> GetAddressByIdAsync(Guid id);
        Task<ResponseModel<List<AddressDTO>>> GetAllAddressesAsync();
    }
}
