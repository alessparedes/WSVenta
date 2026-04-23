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
            var respuesta = new Respuesta();
            var connectionString =_configuration.GetConnectionString("VentaRealConnection");
            Console.WriteLine($"DEBUG ConnectionString: {connectionString}");
            try
            {
                var lst = _context.Clientes.OrderBy(x => x.Id).ToList();
                respuesta.Exito = 1;
                respuesta.Data = lst;
            }         
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
   
            return Ok(respuesta);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest model)
        {
            var respuesta = new Respuesta();
            var connectionString =_configuration.GetConnectionString("VentaRealConnection");
            Console.WriteLine($"DEBUG ConnectionString: {connectionString}");
            try
            {
                var cliente = new Cliente
                {
                    Nombre = model.nombre
                };
                _context.Add(cliente);
                _context.SaveChanges();
                respuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpPut]
        public IActionResult Edit(ClienteRequest model)
        {
            var respuesta = new Respuesta();
            try
            {
                var cliente = _context.Clientes.Find(model.Id);
                if (cliente is not null)
                {
                    cliente.Nombre = model.nombre?.Trim();
                    _context.SaveChanges();
                    respuesta.Exito = 1;
                }
                else 
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = "Cliente no encontrado";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete (int id)
        {
            var respuesta = new Respuesta();
            try
            {
                var cliente = _context.Clientes.Find(id);
                if (cliente is not null)
                {
                    _context.Clientes.Remove(cliente);
                    _context.SaveChanges();
                    respuesta.Exito = 1;
                }
                else 
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = "Cliente no encontrado";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
    }
}
