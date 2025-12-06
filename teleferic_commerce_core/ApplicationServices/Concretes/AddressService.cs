using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Address;
using teleferic_commerce_infrastructure.Models;
using teleferic_commerce_infrastructure.UoW;

namespace teleferic_commerce_core.ApplicationServices.Concretes
{
    public class AddressService : IAddressService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly string UserId;

        public AddressService(IMapper mapper,IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<AddressDTO>> CreateAddressAsync(CreateAddressDTO createAddressDTO)
        {

            if (createAddressDTO.IsDefault)
            {
                var existingDefault = await unitOfWork.Addresses.FindAsync(x => x.UserId == UserId && x.IsDefault);
                foreach (var adrs in existingDefault)
                {
                    adrs.IsDefault = false;
                }
            }

            var address = mapper.Map<teleferic_core_domain.Entities.Address>(createAddressDTO);
            address.UserId = UserId;
            await unitOfWork.Addresses.AddAsync(address);
            await unitOfWork.SaveAsync();
            var addressDTO = mapper.Map<AddressDTO>(address);
            return new ResponseModel<AddressDTO>
            {
                IsSuccess = true,
                Data = addressDTO,
                Message = "Address created successfully."
            };

        }

        public async Task<ResponseModel<bool>> DeleteAddressAsync(Guid id)
        {
            var address = await unitOfWork.Addresses.FindAsync(x => x.Id == id || x.UserId == UserId);
            if (address is null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Address not found."
                };
            }

            await unitOfWork.Addresses.SoftDeleteAsync(id);
            await unitOfWork.SaveAsync();
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "Address deleted successfully."
            };

        }

        public async Task<ResponseModel<AddressDTO>> GetAddressByIdAsync(Guid id)
        {
            var addresses = unitOfWork.Addresses.FindAsync(x => x.Id == id && x.UserId == UserId);
            if (addresses is null)
            {
                return new ResponseModel<AddressDTO>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Address not found."
                };
            }
            var addressDTO = mapper.Map<AddressDTO>(addresses);
            return new ResponseModel<AddressDTO>
            {
                IsSuccess = true,
                Data = addressDTO,
                Message = "Address retrieved successfully."
            };
        }

        public async Task<ResponseModel<List<AddressDTO>>> GetAllAddressesAsync()
        {
            var addreses =  unitOfWork.Addresses.FindAsync(x => x.UserId == UserId).Result
                .OrderByDescending(x => x.IsDefault).ThenByDescending(x => x.CreatedAt).ToList();
            var addressDTOs = mapper.Map<List<AddressDTO>>(addreses);
            return new ResponseModel<List<AddressDTO>>
            {
                IsSuccess = true,
                Data = addressDTOs,
                Message = "Addresses retrieved successfully."
            };
        }

        public async Task<ResponseModel<bool>> UpdateAddressAsync(Guid id, UpdateAddressDTO updateAddressDTO)
        {
            var address = unitOfWork.Addresses.FindAsync(x => x.Id == id && x.UserId == UserId).Result;
            if (address is null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Address not found."
                };
            }
            if (updateAddressDTO.IsDefault)
            {
                var existingDefault = unitOfWork.Addresses.FindAsync(x => x.UserId == UserId && x.IsDefault).Result;
                foreach (var adrs in existingDefault)
                {
                    adrs.IsDefault = false;
                }
            }
            mapper.Map(updateAddressDTO, address);
            var adres = address.FirstOrDefault();
            await unitOfWork.Addresses.Update(adres);
            unitOfWork.SaveAsync().Wait();
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "Address updated successfully."
            };
        }
    }
}
