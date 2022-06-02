using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace WebApi.Controllers
{
    /// <summary>
    /// Проверка идентификации.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Проверка идентификации.
        /// </summary>
        /// <returns>Данные Claim: User, Role</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var nameIdentifier1 = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var nameIdentifier2 = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role);

            return new string[] { nameIdentifier1?.Value, nameIdentifier2?.Value };
        }
    }
}
