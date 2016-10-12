using System;

namespace DataTypeObject
{
    public class DtoAudio
    {        
        public int id { get; set; }

        public int id_audio { get; set; }

        public string usuario { get; set; }

        public DateTime fecha_envio { get; set; }

        public int idExtension { get; set; }
    }
}