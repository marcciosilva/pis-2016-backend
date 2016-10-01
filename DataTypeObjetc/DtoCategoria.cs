namespace DataTypeObject
{
    public enum NombrePrioridad
    {
        Alta,
        Media,
        Baja
    }

    public class DtoCategoria
    {
        public int id { get; set; }

        public string codigo { get; set; }

        public string clave { get; set; }

        public NombrePrioridad prioridad { get; set; }

        public bool activo { get; set; }

    }
}