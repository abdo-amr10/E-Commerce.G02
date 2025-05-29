using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;
using Shared.Order_Models;

namespace Presentation
{
    public class AuthenticationController(IServiceManger serviceManger) : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
           => Ok(await serviceManger.AuthenticationService.LoginAsync(loginDto));

        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
        => Ok(await serviceManger.AuthenticationService.RegisterAsync(registerDto));

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
            => Ok(await serviceManger.AuthenticationService.CheckEmailExist(email));

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.AuthenticationService.GetUserByEmail(email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.AuthenticationService.GetUserAddress(email);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.AuthenticationService.UpdateUserAddress(address, email);
            return Ok(result);
        }
    }
}
