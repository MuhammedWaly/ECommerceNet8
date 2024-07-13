using ECommerceNet8.Core.DTOS.ApplicationUsers.Request;
using ECommerceNet8.Core.DTOS.ApplicationUsers.Response;
using ECommerceNet8.Core.Reposiatories.AuthReposaitory;
using ECommerceNet8.Core.Services;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly  IAuthReposaitory _authReposaitory;
        private readonly  UserManager<ApplicationUser> _userManager;
        private readonly IMailingService _mailingService;

        public AuthenticationController(IAuthReposaitory authReposaitory, UserManager<ApplicationUser> userManager, IMailingService mailingService)
        {
            _authReposaitory = authReposaitory;
            _userManager = userManager;
            _mailingService = mailingService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseDTo>> Register(RegisterRequestDTo userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest((new RegisterResponseDTo() { IsSuccess = false, Message = new List<string>() { "Something went wrong"} }));

            var result = await _authReposaitory.RegisterAsync(userDto);
            if(result.IsSuccess == false)
            {
                return BadRequest(new RegisterResponseDTo() { IsSuccess = false , Message = result.Message }); 
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(result.ApplicationUser);
            var callbackUrl = Request.Scheme + "://" + Request.Host + 
                Url.Action("ConfirmEmail", "Authentication", new {userId = result.ApplicationUser.Id, code = code });

            string Body = "Dear " + userDto.FirstName + " " + userDto.LastName + "\n" +
              $" Here is your Confirmation link: <a href =\"{callbackUrl}\">Click here</a>.";



            await _mailingService.SendEmailAsync(userDto.EmailAddress,"Email Confirmation" , Body);

            return Ok(result);
        }
        
        [HttpPost("RegisterAsAdmin")]
        public async Task<IActionResult> RegisterAsAdmin(RegisterRequestDTo userDto,int secretKey)
        {

            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _authReposaitory.RegisterAdminAsync(userDto,secretKey);

            return Ok(result);
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail (string UserId, string code)
        {
            if(UserId == null || code == null)
            {
                return BadRequest(new ConfirmEmailResponse() { IsSuccess = false, Message = "Wrong Confirmation link" }); 
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return BadRequest(new ConfirmEmailResponse() { IsSuccess = false, Message= "Wrong User Id" });

            var result = await _userManager.ConfirmEmailAsync(user, code);
            var status = result.Succeeded ? "Email Confirmed" : "Something went wrong please try again later";
            
            return Ok(new ConfirmEmailResponse () { IsSuccess = true, Message = status });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new LoginResponseDto() { Result = false, Messages = new List<string>() { "Invalid Input"} });
            }

            var Login = await _authReposaitory.LoginAsync(loginDto);
            if (Login.Result == false)
                return BadRequest(Login);

            return Ok(Login);

        }

        [HttpPost("GenerateTokens")]
        public async Task<IActionResult> GenerateTokens(RequestTokenDto tokenDto)
        {
            var authResponse = await _authReposaitory.VerfiyAndGenerateToken(tokenDto);
            if(authResponse.Result == false)
            {
                return BadRequest(authResponse);
            }
            return Ok(authResponse);
        }
        
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            string userId = HttpContext.User.FindFirstValue("UId");

            var logout = await _authReposaitory.LogoutDeleteRefrehToken(userId);
            if(logout == false)
            {
                return BadRequest("Already Logout");
            }
            return Ok("Logout successful");
        }

        [HttpGet("ResetPassword/{Email}")]
        public async Task<IActionResult> ResetPasswordEmail([FromRoute] string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return BadRequest("No user found");

            if (user.EmailConfirmed == false)
                return BadRequest("Email Not Confirmed");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var EncodedToken = Encoding.UTF8.GetBytes(token);
            var ValidToken = WebEncoders.Base64UrlEncode(EncodedToken);

            var callbackUrl = Request.Scheme + "://" + Request.Host +
                $"/ResetPassword?Email={Email}&Token={ValidToken}";

            

            string Body = "Dear " + user.FirstName + " " + user.LastName + "\n" +
              $"Please Clik here to resest your password: <a href =\"{callbackUrl}\">Click here</a>.";

              await _mailingService.SendEmailAsync(user.Email!, "Email Confirmation", Body);
            return Ok("Reset link sent to your email");

        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user is null)
                return BadRequest("wrong Email");

            var Decodetoken = WebEncoders.Base64UrlDecode(resetPasswordRequest.Token);
            var normalizeToken = Encoding.UTF8.GetString(Decodetoken);

            var result = await _userManager.ResetPasswordAsync(user, normalizeToken, resetPasswordRequest.NewPassword);
            if(result.Succeeded)
            {
                return Ok(new PasswordResetResponse()
                {
                    IsSuccess = true,
                    Message = "passwoed cahnged successfully"
                });
            }

            List<string> Errors = new List<string>();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.Description);
            }

            return BadRequest(new PasswordResetResponse()
            {
                IsSuccess = false,
                Message = "faild to change password",
                Errors = Errors
            });
        }
    }
}
