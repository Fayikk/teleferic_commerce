using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO;
using teleferic_commerce_infrastructure.Extensions.TokenExtensions;
using teleferic_commerce_infrastructure.Models;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_core.ApplicationServices.Concretes
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ResponseModel<UserDTO> _responseModel; 
        public AuthService(ResponseModel<UserDTO> _responseModel, ITokenService tokenService,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            this._responseModel = _responseModel;
        }

        public async Task<ResponseModel<UserDTO>> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                _responseModel.IsSuccess = false;
                _responseModel.Message = "Invalid email or password.";
                return _responseModel;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                _responseModel.IsSuccess = false;
                _responseModel.Message = "Invalid email or password.";
                return _responseModel;
            }

            var roles = await _userManager.GetRolesAsync(user);
            _responseModel.IsSuccess = true;
            _responseModel.Data = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _tokenService.CreateToken(user, roles)
            };
            return _responseModel;
        }

        public async Task<ResponseModel<UserDTO>> Register(RegisterDTO dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                _responseModel.IsSuccess = false;
                string messages = "";
                foreach (var item in result.Errors)
                {
                    messages = messages + item.Description;
                }
                _responseModel.Message = messages;
                return _responseModel;
            }

            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            _responseModel.IsSuccess = true;
            _responseModel.Data = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _tokenService.CreateToken(user, roles)
            };
            return _responseModel;
        }
    }
}
