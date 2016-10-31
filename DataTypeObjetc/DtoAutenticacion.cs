﻿using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoAutenticacion
    {
        /// <summary>
        /// Constructor con parametros para crear un Data Type Object de Autenticacion utilizado para generar la respuesta al Login.
        /// </summary>
        /// <param name="token">Token generado por la logica de un usuario autenticado.</param>
        /// <param name="mensaje">Mensaje que se desea enviar.</param>
        public DtoAutenticacion(string token, string mensaje)
        {
            this.accessToken = token;
            this.msg = mensaje;
        }

        [JsonProperty(PropertyName = "accessToken")]
        public string accessToken { get; set; }

        public string msg { get; set; }
    }
}
