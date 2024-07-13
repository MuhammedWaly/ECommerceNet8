using ECommerceNet8.Core.DTOS.ApplicationUsers.Request;
using ECommerceNet8.Core.DTOS.ApplicationUsers.Response;
using ECommerceNet8.Infrastructure.Constants;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ECommerceNet8.Core.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ECommerceNet8.Core.Reposiatories.AuthReposaitory
{
    public class AuthReposaitory : IAuthReposaitory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthReposaitory(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<RegisterResponseDTo> RegisterAsync(RegisterRequestDTo userDto)
        {
            if(await _context.Users.FirstOrDefaultAsync(u=>u.Email == userDto.EmailAddress) != null )
            {
                return new RegisterResponseDTo() { IsSuccess = false, Message = new List<string>() { "Email is already Registered"} };
            }


            var User = new ApplicationUser()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.EmailAddress
            };
            User.UserName = userDto.EmailAddress;
            User.EmailConfirmed = false;

            var result = await _userManager.CreateAsync(User, userDto.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Roles.Customer);
                return new RegisterResponseDTo()
                {
                    IsSuccess = true,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    EmailAddress = User.Email,
                    ApplicationUser = User,
                    Message = new List<string>()
                    {
                        "Please confirim your email"
                    }
                };
            }

            List<string> errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description.ToString());
            }

            return new RegisterResponseDTo()
            {
                IsSuccess = false,
                Message = errors
            };
        }


        public async Task<RegisterResponseDTo> RegisterAdminAsync(RegisterRequestDTo userDto, int SecretKey)
        {
           if(SecretKey != 12345 )
            {
                return new RegisterResponseDTo()
                {
                    IsSuccess = false,
                    Message = new List<string>()
                    {
                        "Wrong Secret Key"
                    },
                };
            }

            var User = new ApplicationUser()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.EmailAddress
            };
            User.UserName = userDto.EmailAddress;
            User.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(User, userDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Roles.Admin);
                return new RegisterResponseDTo()
                {
                    IsSuccess = true,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    EmailAddress = User.Email
                };
            }

            List<string> errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description.ToString());
            }

            return new RegisterResponseDTo()
            {
                IsSuccess = false,
                Message = errors
            };

        }


        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.EmailAddress);
            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>()
                    {

                        "Invalid Email or password"
                    }
                };
            }

            if(!user.EmailConfirmed)
            {
                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Please Confirm your Email first"
                    }
                };
            }

            if(!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>() { "Invalid Email or password" }
                };
            }

            var token = await GenerateToken(user) ;
            var RefrehToken = await GenerateRefreshToken(user, token);
            return new LoginResponseDto()
            {
                Result = true,
                Token = token,
                RefreshToken = RefrehToken,
                Messages = new List<string>() { "Login successfully"}
            };
        }

        public async Task<LoginResponseDto> VerfiyAndGenerateToken(RequestTokenDto TokenDto)
        {
            var JwtSrcurityTokejwnHandler = new JwtSecurityTokenHandler();
            var TokenContent = JwtSrcurityTokejwnHandler.ReadJwtToken(TokenDto.JwtToken);

            var TokenIssuer = TokenContent.Issuer;
            var TokenAduience = TokenContent.Audiences.ToList();
            if (TokenIssuer != _configuration["JWT:Issuer"] || !TokenAduience.Contains(_configuration["JWT:Audience"]))
            {
                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Wrong Token"
                    }
                };
            }

            var username = TokenContent.Claims.ToList().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.Id != TokenDto.UserId)
            {

                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Wrong Token"
                    }
                };
            }

            var RefreshTokenFromDb = await _context.RefreshTokens.FirstOrDefaultAsync(rf => rf.Token == TokenDto.RefreshToken);
            if (RefreshTokenFromDb == null || RefreshTokenFromDb.JwtId == TokenContent.Id)
            {
                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Wrong RefreshToken"
                    }
                };
            }

            if (RefreshTokenFromDb.ExpiresDate < DateTime.UtcNow)
            {
                return new LoginResponseDto()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Expired RefreshToken"

                    }
                };
            }

            var newToken = await GenerateToken(user);
            var newRefrehToken = await GenerateRefreshToken(user, newToken);
            return new LoginResponseDto()
            {
                Result = true,
                Token = newToken,
                RefreshToken = newRefrehToken
            };
        }


        public async Task<bool> LogoutDeleteRefrehToken(string UserId)
        {
            var refrehToken = await _context.RefreshTokens.FirstOrDefaultAsync(rf=>rf.UserId == UserId);

            if (refrehToken == null)
                return false;
             _context.Remove(refrehToken);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            string strkey = _configuration["JWT:Key"]!;

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(strkey);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim("UId",user.Id),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["JWT:Audience"])
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWT:DurationInMin"])),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = _configuration["JWT:Audience"],
                Issuer = _configuration["JWT:Issuer"]
            };

            var token = tokenHandler.CreateToken(TokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        private async Task<string> GenerateRefreshToken(ApplicationUser user, string token)
        {
            var exsistRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rf=>rf.UserId == user.Id);
            if(exsistRefreshToken != null)
            {
                _context.RefreshTokens.Remove(exsistRefreshToken);
                await _context.SaveChangesAsync();
            }

            var JwtSrcurityTokejwnHandler = new JwtSecurityTokenHandler();
            var TokenContent = JwtSrcurityTokejwnHandler.ReadJwtToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = TokenContent.Id,
                UserId = user.Id,
                ExpiresDate = DateTime.UtcNow.AddMinutes(130),
                Token = RandomTokenGenerator(100)
            };
            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        private string RandomTokenGenerator(int length)
        {
            var random = new Random();
            var chars = "ASDFGHJKLPOIUYTREWQZXCVBNMQWERTYUIOPLKJHGFDSAZXCVBNM1234567890!@#$%^&*().,/';L][`";

            return new string(Enumerable.Repeat(chars, length)
                .Select(r => r[random.Next(r.Length)]).ToArray());
        }

        
    }
}
