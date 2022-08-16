namespace WSVenta.Models.Response
{
    public class Respuesta
    {
        public int Exito { set; get; }
        public string Mensaje { set; get; }
        public object Data { set; get; }

        public Respuesta()
        {
            this.Exito = 0;
        }
    }
}
