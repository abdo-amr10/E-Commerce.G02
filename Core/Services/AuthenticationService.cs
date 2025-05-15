using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared;
using Shared.Dtos;
using Shared.Order_Models;

namespace Services
{
    public class AuthenticationService(UserManager<User> _userManager , IOptions<JwtOptions> options , IMapper mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user!= null;
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
            var user = await _userManager.Users.Include(e=> e.Address).FirstOrDefaultAsync(e=> e.Email == email)
                                                                       ?? throw new UserNotFoundException(email);

            return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                                          ?? throw new UserNotFoundException(email);

            return new UserResultDto(
                user.DisplayName,
                user.Email!,
                await CreateTokenAsync(user)
                );
        }

        public async Task<AddressDto> UpdateUserAddress(AddressDto addressDto, string email)
        {
             var user = await _userManager.Users.Include(e=> e.Address).FirstOrDefaultAsync(e=> e.Email == email)
                                                                       ?? throw new UserNotFoundException(email);
            
            if (user.Address != null) // Update
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
            }
            // Set Address With new Address
            else
            {
                var userAddress = mapper.Map<UserAddress>(addressDto);
                user.Address = userAddress;
            }

            await _userManager.UpdateAsync(user);

            return mapper.Map<AddressDto>(user.Address);

        }

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
            var JwtOptions = options.Value;
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create Token
            var token = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,
                audience: JwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(JwtOptions.DurationInDays),
                signingCredentials: cred
            );

            // Object Member Method => Create Object From JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
