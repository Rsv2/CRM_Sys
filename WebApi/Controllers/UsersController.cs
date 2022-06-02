using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер управления пользователями.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// Получить список пользователей.
        /// </summary>
        /// <returns>Список пользователей (Id, Имя)</returns>
        [HttpGet]
        public ActionResult<IEnumerable<AnswerUser>> Get()
        {
            List<AnswerUser> UsersList = new List<AnswerUser>();
            foreach(var users in _userManager.Users.ToList())
            {
                UsersList.Add(new AnswerUser(users.Id, users.Email));
            }
            return UsersList;
        }
        /// <summary>
        /// Создать нового пользователя (user). Возврат GetUsers();
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] AuthenticationRequest model)
        {
            User user = new User { Email = model.Name, UserName = model.Name, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
            }
        }
        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id">id пользователя.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete(string id)
        {
            if (id != "1")
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                }
            }
        }
    }
}
