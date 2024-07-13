using ECommerceNet8.Core.DTOS.ApplicationUsers.Request;
using ECommerceNet8.Core.DTOS.ApplicationUsers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.AuthReposaitory
{
    public interface IAuthReposaitory
    {
        Task<RegisterResponseDTo> RegisterAsync(RegisterRequestDTo userDto);
        Task<RegisterResponseDTo> RegisterAdminAsync(RegisterRequestDTo userDto, int SecretKey);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto userDto);

        Task<LoginResponseDto> VerfiyAndGenerateToken(RequestTokenDto TokenDto);

        Task<bool> LogoutDeleteRefrehToken(string UserId);
    }
}
