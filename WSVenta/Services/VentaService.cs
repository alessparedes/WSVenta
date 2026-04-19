using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services;

public class VentaService : IVentaService
{
    public void Add(VentaRequest model)
    {
        using var db = new VentaRealContext();
        var transaction = db.Database.BeginTransaction();
        try
        {
            var venta = new Venta
            {
                Total = model.Conceptos.Sum(x => x.Cantidad*x.PrecioUnitario),
                Fecha = DateTime.Now,
                IdCliente = model.IdCliente
            };
            db.Venta.Add(venta);
            db.SaveChanges();

            foreach (var item in model.Conceptos)
            {
                var concepto = new Models.Concepto
                {
                    IdProducto = item.IdProducto,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad,
                    Importe = item.Importe,
                    IdVenta = venta.Id
                };
                db.Conceptos.Add(concepto);
                db.SaveChanges();
            }
                    
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw new Exception("Error al registrar la venta");
        }
        finally
        {
            transaction.Dispose();
        }
    }
}