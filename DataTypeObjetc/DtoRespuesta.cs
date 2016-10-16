namespace DataTypeObject
{
    public class DtoRespuesta
    {
        //// Cod 0: Ok.
        //// Cod 1: Usuario/Password inválido.
        //// Cod 2: Error, usuario no autenticado.
        //// Cod 3: Se ha seleccionado más de un recurso.
        //// Cod 4: Se han seleccionado zonas y recursos simultáneamente.
        //// Cod 5: El usuario tiene una operación no finalizada.
        //// Cod 7: Dispositivo ya registrado.

        public int cod { get; set; }

        public object response { get; set; }

        public DtoRespuesta(int codigo, object respuesta)
        {
            this.cod = codigo;
            this.response = respuesta;
        }
    }
}