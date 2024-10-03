using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.User;
using BookStore.Application.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace BookStore.WebAPI.Controllers
{
    [AllowAnonymous]
    public class AuthController : AppControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JWTSection _jwtConfig;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(ILogger<AuthController> logger,
                              IOptions<JWTSection> jwtConfig, 
                              IUserService userService, 
                              IEmailService emailService)
        {
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            _logger.LogInformation($"Registration attempt for {registerRequest.Email}");

            var createdDate = DateTime.Now;

            var userDto = new CreateUserDto()
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordRaw = registerRequest.Password,
                CreatedDate = createdDate,
            };

            bool result = await _userService.AddUser(userDto);

            if (!result)
            {
                return Conflict("Email is already exist");
            }

            return Accepted();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            _logger.LogInformation($"Login attempt for {loginRequest.Email}");

            UserDto? userDto = await _userService.AuthorizeUser(loginRequest.Email!, loginRequest.Password!);
            
            if (userDto is null)
            {
                return Unauthorized("Email or password is incorrect");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, loginRequest.Email!),
                new Claim(ClaimTypes.Role, userDto.Role)
            };

            string jwtToken = GenerateJwtToken(claims);

            var response = new AuthResponse()
            {
                Token = jwtToken
            };

            return response;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            _logger.LogInformation($"{forgotPasswordRequest.Email} request for changing password");

            bool isUserExist = await _userService.IsExistUser(forgotPasswordRequest.Email!);

            if (!isUserExist)
            {
                return Unauthorized("User doesn't exist");
            }

            await _emailService.SendEmailForResetPassword(forgotPasswordRequest.Email!);

            return Ok("Email sent");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest, [FromQuery] string email)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var resetPasswordDto = new ResetPasswordDto
            {
                Email = email,
                OtpCode = resetPasswordRequest.OtpCode,
                NewPasswordRaw = resetPasswordRequest.NewPassword
            };

            bool result = await _userService.ChangePassword(resetPasswordDto);
            
            if (!result)
            {
                return Unauthorized("Otp code or email doesn't correct");
            }

            return Ok("Change password success");
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var secretInBytes = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
            var securityKey = new SymmetricSecurityKey(secretInBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.Now.AddMinutes(_jwtConfig.ExpiresInMinutes),
                claims: claims,
                signingCredentials: credentials
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
