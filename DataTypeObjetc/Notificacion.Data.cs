namespace DataTypeObject
{
    public class data
    {
        public data(string cod, string pk)
        {
            this.code = cod;
            this.primarykey = pk;    
        }

        public string code { get; set; }

        public string primarykey { get; set; }
    }
}
