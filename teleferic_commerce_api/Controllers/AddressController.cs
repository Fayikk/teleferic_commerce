using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Address;

namespace teleferic_commerce_api.Controllers
{
    public class UploadBinaryDto
    {
        public byte[] Data { get; set; }
    }







    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDTO createAddressDTO)
        {
            var result = await _addressService.CreateAddressAsync(createAddressDTO);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetAddressById), new { id = result.Data.Id },result.Data);
            return BadRequest(result);
        }


        [HttpPost("upload-binary")]
        public IActionResult UploadBinary([FromBody] UploadBinaryDto dto)
        {
            if (dto.Data == null || dto.Data.Length == 0)
                return BadRequest("Byte data boş olamaz.");

            // dto.Data => byte[]
            return Ok(new { Size = dto.Data.Length });
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] UpdateAddressDTO updateAddressDTO)
        {
            var result = await _addressService.UpdateAddressAsync(id, updateAddressDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var result = await _addressService.GetAddressByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await _addressService.GetAllAddressesAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
