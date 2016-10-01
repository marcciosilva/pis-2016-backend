using DataTypeObjetc;

namespace DataTypeObject
{
    public class GenericResponse
    {
        public int cod { get; set; }

        public Response response { get; set; }

        public GenericResponse(int codigo, Response r) {
            cod = codigo;
            response = r;
        }
    }
}