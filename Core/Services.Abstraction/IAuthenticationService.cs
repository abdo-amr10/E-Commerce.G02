using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;
using Shared.Order_Models;

namespace Services.Abstraction
{
    public interface IAuthenticationService
    {
        public Task<UserResultDto> LoginAsync(LoginDto loginDto);
        public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        public Task<UserResultDto> GetUserByEmail(string email);
        public Task<bool> CheckEmailExist(string email);
        public Task<AddressDto> GetUserAddress(string email);
        public Task<AddressDto> UpdateUserAddress(AddressDto addressDto , string email);

    }
}
