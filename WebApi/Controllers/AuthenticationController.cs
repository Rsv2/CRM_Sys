using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер аутентификации.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        /// <summary>
        /// Инициализация контроллера.
        /// </summary>
        /// <param name="userManager">Управление пользователями</param>
        /// <param name="signInManager">Проверка входа</param>
        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="authRequest">Форма авторизации</param>
        /// <param name="signingEncodingKey">Шифрование</param>
        /// <param name="encryptingEncodingKey">Шифрование</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> PostAsync([FromBody] AuthenticationRequest authRequest,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey,
            [FromServices] IJwtEncryptingEncodingKey encryptingEncodingKey)
        {
            if ((await _signInManager.PasswordSignInAsync(authRequest.Name, authRequest.Password, false, false)).Succeeded)
            {
                User user = await _userManager.FindByNameAsync(authRequest.Name);
                string role;
                if (await _userManager.IsInRoleAsync(user, "admin")) { role = "admin"; }
                else if (await _userManager.IsInRoleAsync(user, "user")) { role = "user"; }
                else { role = "uncknown"; }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, authRequest.Name),
                    new Claim(ClaimTypes.Role, role)
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                    issuer: "DiplomApi",
                    audience: "DiplomClient",
                    subject: new ClaimsIdentity(claims),
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(5),
                    issuedAt: DateTime.Now,
                    signingCredentials: new SigningCredentials(
                        signingEncodingKey.GetKey(),
                        signingEncodingKey.SigningAlgorithm),
                    encryptingCredentials: new EncryptingCredentials(
                        encryptingEncodingKey.GetKey(),
                        encryptingEncodingKey.SigningAlgorithm,
                        encryptingEncodingKey.EncryptingAlgorithm));

                var jwtToken = tokenHandler.WriteToken(token);
                return jwtToken;
            }
            else
            {
                return "Unauthorize";
            }
        }
    }
}
