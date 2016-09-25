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
        public string Codigo { get; set; }

        public string Clave { get; set; }

        public NombrePrioridad Prioridad { get; set; }

        public bool Activo { get; set; }

    }
}