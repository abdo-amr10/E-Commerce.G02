using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;

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


    }
}
