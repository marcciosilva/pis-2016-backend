using System;
using Newtonsoft.Json;

namespace Emsys.ServiceLayer
{
    // yyyy-MM-dd'T'hh:mm:ss.SSS
    internal class MyDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {           
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("yyyy'-'MM'-'dd'T'hh':'mm':'ss'.'SSS"));
        }
    }
}