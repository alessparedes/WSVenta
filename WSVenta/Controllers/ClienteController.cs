using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;
using WSVenta.Models.Response;
using WSVenta.Models.Request;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly VentaRealContext _context;

        public ClienteController(IConfiguration configuration, VentaRealContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta oRespuesta = new Respuesta();
            var connectionString =_configuration.GetConnectionString("VentaRealConnection");
            Console.WriteLine($"DEBUG ConnectionString: {connectionString}");
            try
            {
                var lst = _context.Clientes.OrderBy(x => x.Id).ToList();
                oRespuesta.Exito = 1;
                oRespuesta.Data = lst;
            }         
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
   
            return Ok(oRespuesta);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest oModel)
        {
            Respuesta oRespuesta = new Respuesta();
            var connectionString =_configuration.GetConnectionString("VentaRealConnection");
            Console.WriteLine($"DEBUG ConnectionString: {connectionString}");
            try
            {
                Cliente oCliente = new Cliente();
                oCliente.Nombre = oModel.nombre;
                _context.Add(oCliente);
                _context.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Exito = 0;
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPut]
        public IActionResult Edit(ClienteRequest oModel)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                Cliente? oCliente = _context.Clientes.Find(oModel.Id);
                oCliente.Nombre = oModel.nombre.Trim();
                _context.Entry(oCliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete (int Id)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                Cliente oCliente = _context.Clientes.Find(Id);
                _context.Remove(oCliente);
                _context.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
    }
}
