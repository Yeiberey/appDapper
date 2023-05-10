namespace appDapper.Controllers.Entidades
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string DescripcionLarga { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int CantidadSalida { get; set; }
        public decimal Total { get; set; }
        public decimal TarifaIvaVenta { get; set; }
        public bool EsBolsa { get; set; }
        public int EquipoId { get; set; }
    }
}
