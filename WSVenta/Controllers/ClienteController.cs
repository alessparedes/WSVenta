using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;
using WSVenta.Models.Response;
using WSVenta.Models.Request;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                using VentaRealContext db = new VentaRealContext();
                var lst = db.Clientes.OrderByDescending(x => x.Id).ToList();
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
            try
            {
                using VentaRealContext db = new VentaRealContext();
                Cliente oCliente = new Cliente();
                oCliente.Nombre = oModel.Name;
                db.Add(oCliente);
                db.SaveChanges();
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
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
                using VentaRealContext db = new VentaRealContext();
                Cliente oCliente = db.Clientes.Find(oModel.Id);
                oCliente.Nombre = oModel.Name.Trim();
                db.Entry(oCliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
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
                using VentaRealContext db = new VentaRealContext();
                Cliente oCliente = db.Clientes.Find(Id);
                db.Remove(oCliente);
                db.SaveChanges();
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
