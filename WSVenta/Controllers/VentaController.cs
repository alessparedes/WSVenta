using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _venta;
        
        public VentaController(IVentaService venta)
        {
            _venta = venta;
        }
        
        [HttpPost]
        public IActionResult Add(VentaRequest model)
        {
            var respuesta = new Respuesta();
            try
            {
                _venta.Add(model);
                respuesta.Exito = 1;
            }
            catch (Exception e)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = e.Message;
            }
            
            return Ok(respuesta);
        }
    }
}
