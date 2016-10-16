using System.Collections;

namespace DataTypeObject
{
    public class DtoRespuestaExterna
    {
        public string field1 { get; set; }

        public string field2 { get; set; }

        public string field3 { get; set; }

        public string field4 { get; set; }

        public string field5 { get; set; }

        public string field6 { get; set; }

        public string field7 { get; set; }

        public string field8 { get; set; }

        public string field9 { get; set; }

        public string field10 { get; set; }

        public DtoRespuestaExterna(string param1, string param2, string param3, string param4, string param5, string param6, string param7, string param8, string param9, string param10)
        {
            this.field1 = param1;
            this.field2 = param2;
            this.field3 = param3;
            this.field4 = param4;
            this.field5 = param5;
            this.field6 = param6;
            this.field7 = param7;
            this.field8 = param8;
            this.field9 = param9;
            this.field10 = param10;
        }
    }
}