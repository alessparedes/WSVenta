using Microsoft.AspNetCore.Mvc;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthRequest loginRequest)
        {
            Respuesta respuesta = new Respuesta();
            var user = _userService.Auth(loginRequest);
            if (user == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Usuario o Contraseña incorrecta.";
                return BadRequest();
            }
            respuesta.Exito = 1;
            respuesta.Data = user;
            return Ok(respuesta);
        }
    }
}
