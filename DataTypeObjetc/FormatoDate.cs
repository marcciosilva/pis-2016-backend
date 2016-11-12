using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataTypeObject
{
    public class FormatoDate : IsoDateTimeConverter
    {
        /// <summary>
        /// Constructor de Formato de fechas.
        /// </summary>
        public FormatoDate()
        {
            base.DateTimeFormat = "yyyy-MM-dd'T'hh:mm:ss.fff";
        }
    }
}
