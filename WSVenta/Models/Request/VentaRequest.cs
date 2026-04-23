using System.ComponentModel.DataAnnotations;

namespace WSVenta.Models.Request;

public class VentaRequest
{
    [Required]
    [Range(1, Double.MaxValue, ErrorMessage = "Valor de idCliente debe ser mayor a 0.")]
    [ExisteCliente(ErrorMessage = "El cliente no existe.")]
    public int IdCliente { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Debe agregar al menos un concepto.")]
    public List<Concepto> Conceptos { get; set; } = [];
}

public class Concepto
{
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Importe { get; set; }
    public int IdProducto { get; set; }
}

#region Validaciones

public class ExisteClienteAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var idcliente = Convert.ToInt32(value);
        // Pedimos el DBContext al sistema de servicios de .NET
        // Esto usa la configuración que ya tienes en Program.cs (Docker o Local)
        var db = (VentaRealContext)validationContext.GetService(typeof(VentaRealContext))!;
        
        return db.Clientes.Any(c => c.Id == idcliente) ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? "El cliente no existe.");
    }
}
#endregion