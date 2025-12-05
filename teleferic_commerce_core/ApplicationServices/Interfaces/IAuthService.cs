using teleferic_commerce_core.DTO;
using teleferic_commerce_infrastructure.Models;

namespace teleferic_commerce_core.ApplicationServices.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel<UserDTO>> Register(RegisterDTO dto);
        Task<ResponseModel<UserDTO>> Login(LoginDTO dto);
    }
}
