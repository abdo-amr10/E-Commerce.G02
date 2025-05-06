using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared.Dtos;

namespace Services
{
    public class AuthenticationService(UserManager<User> _userManager) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UnAuthorizedException($"Email {loginDto.Email} doesn't Exist");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new UnAuthorizedException();

            return new UserResultDto
                (
                user.DisplayName,
                user.Email!,
                await CreateTokenAsync(user)
                );
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user , registerDto.Password);

            if(!result.Succeeded )
            {
                var errors = result.Errors.Select(e=> e.Description).ToList();
                throw new ValidationException(errors);

            }

            return new UserResultDto
                (
                user.DisplayName,
                user.Email,
                await CreateTokenAsync(user)
                );
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Add Roles To Claims If Exist
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            // Create Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "e245fc82912c024ece59ac9f7b49834605545aab17e6176c3e38" +
                "a6dca3f6f34ab3c66ab0cd7d09e4a1edf9bf891b298fd101d40f203" +
                "c00b9f74dd9cbec47d8fdc859e51f1e2b3a7ea7b1860f1ad96c3829ab046" +
                "e827dbc3e3ac66804b4ae3e36c66d195050a872de84a71edbc2a6d4d7d6d1d5b2" +
                "bcee5608ddd08a28d85e24940a7a38c152cb17467e803f7c57416f232f7da87b87d" +
                "303a44cfb3b3b3b0d5588e2ae4d344f1cad3de7c3dbdfe01af7ac3995468634c0d24ac4" +
                "241731b78c478425c118fe5fe92430630fccb2cc356124e02a820b0795f37eea7ac803877" +
                "87271b63040922fa65f9d82bdf7fba49ce6306be18e701c837c0deb9012f4a771b5b7"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create Token
            var token = new JwtSecurityToken(
                issuer: "https://localhost:7186/",
                audience: "My Audience",
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: cred
            );

            // Object Member Method => Create Object From JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
