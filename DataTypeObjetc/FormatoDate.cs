using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataTypeObject
{
    public class FormatoDate : IsoDateTimeConverter
    {
        public FormatoDate()
        {
            base.DateTimeFormat = "yyyy-MM-dd'T'hh:mm:ss.fff";
        }
    }
}
