using Microsoft.AspNetCore.Mvc;
using CGC.Models;
using CGC.Services;
using CGC.Helpers;
using CGC.Funkcje.UserFuncFolder.UserReturn;

namespace CGC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private IUserService _userService;
        UserBaseModify userBasemodify = new UserBaseModify();

        public ValuesController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            userBasemodify.Insert_token(response.Login, response.Token);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
