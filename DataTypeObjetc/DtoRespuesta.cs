namespace DataTypeObject
{

    public class DtoRespuesta
    {

        public int cod { get; set; }

        public object response { get; set; }

        public DtoRespuesta(int codigo, object respuesta)
        {
            cod = codigo;
            response = respuesta;
        }

    }
}