namespace appDapper.Controllers.Entidades
{
    public class Factura
    {
        public string NoIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Barrio { get; set; }
        public List<Producto> Productos { get; set; }

    }
}
