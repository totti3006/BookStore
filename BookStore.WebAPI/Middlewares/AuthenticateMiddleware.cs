using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BookStore.WebAPI.Middlewares
{
    public class AuthenticateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthenticateMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // by pass checking authentication if we access login method
            //if (context.Request.Path.StartsWithSegments("/Users/FakeLoginToGenerateToken"))
            //{
            //    await _next(context);
            //    return;
            //}

            if (!context.Request.Headers.ContainsKey("Authorization")) // if this key not exit we assum that they do not have jwt
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("please include Authorization key in header & value is JWT key");
                return;
            }

            try
            {
                // get the key from header
                var authHeader = context.Request.Headers["Authorization"].ToString();

                // read claim from jwt
                var claimsPrincipal = GetClaimPrincipal(authHeader);
                context.User = claimsPrincipal;
            }
            catch (Exception)
            {
                // this is where jwt token invalid
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("JWT invalid");
                return;
            }
            // if everything is okay we call next middle ware
            await _next(context);
        }

        private ClaimsPrincipal GetClaimPrincipal(string jwtToken)
        {
            string secretKey = _configuration.GetSection("AppSettings")["JWTSection:SecretKey"];
            var secretInBytes = Encoding.UTF8.GetBytes(secretKey);
            var securityKey = new SymmetricSecurityKey(secretInBytes);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // set to true if you want to validate issuer
                ValidateAudience = false, // set to true if you want to validate audience
                ValidateLifetime = true, // set to true if you want to validate expiration
                IssuerSigningKey = securityKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);
        }
    }
}
