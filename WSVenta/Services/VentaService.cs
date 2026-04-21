using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services;

public class VentaService : IVentaService
{
    private readonly VentaRealContext _db;
    
    public VentaService(VentaRealContext db)
    {
        _db = db;
    }
    
    public void Add(VentaRequest model)
    {
        var transaction = _db.Database.BeginTransaction();
        try
        {
            var venta = new Venta
            {
                Total = model.Conceptos.Sum(x => x.Cantidad*x.PrecioUnitario),
                Fecha = DateTime.Now,
                IdCliente = model.IdCliente
            };
            _db.Venta.Add(venta);
            _db.SaveChanges();

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
                _db.Conceptos.Add(concepto);
                _db.SaveChanges();
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