using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using Concepto = WSVenta.Models.Concepto;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentaController : ControllerBase
    {
        [HttpPost]
        public IActionResult Add(VentaRequest model)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using VentaRealContext db = new VentaRealContext();
                using (var transaction = db.Database.BeginTransaction()) 
                {
                    try
                    {
                        var venta = new Venta();
                        venta.Total = model.Conceptos.Sum(x => x.Cantidad*x.PrecioUnitario);
                        venta.Fecha = DateTime.Now;
                        venta.IdCliente = model.IdCliente;
                        db.Venta.Add(venta);
                        db.SaveChanges();

                        foreach (var item in model.Conceptos)
                        {
                            var concepto = new Concepto();
                            concepto.IdProducto = item.IdProducto;
                            concepto.PrecioUnitario = item.PrecioUnitario;
                            concepto.Cantidad = item.Cantidad;
                            concepto.Importe = item.Importe;
                            concepto.IdVenta = venta.Id;
                            db.Conceptos.Add(concepto);
                            db.SaveChanges();
                        }
                        
                        transaction.Commit();
                        respuesta.Exito = 1;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        transaction.Rollback();
                    }
                }
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
