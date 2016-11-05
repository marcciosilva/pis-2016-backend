namespace DataTypeObject
{
    public class DtoUsuario
    {
        public string username { get; set; }

        public string password { get; set; }

        public DtoRol roles { get; set; }
    }
}
