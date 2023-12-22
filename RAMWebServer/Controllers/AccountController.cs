using Microsoft.AspNetCore.Mvc;
using RAMWebServer.Models;
using System.Threading.Tasks;

namespace RAMWebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly DatabaseContext context;
        public AccountController(DatabaseContext context)
        {
            this.context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            //Проверка валидации и обработки ошибок
            var newUser = new User
            {
                UserName = model.UserName,
                Password = model.Password,
            };
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return Ok(new { Message = "Пользователь успешно зарегестрирован." });
        }
    }
}
