namespace DataTypeObject
{
    public class data
    {
        public data(string cod, int ev, int ext, int z, string zN)
        {
            code = cod;
            eventId = ev;
            extensionId = ext;
            zoneId = z;
            zoneName = zN;
        }

        public string code { get; set; }

        public int eventId { get; set; }

        public int extensionId { get; set; }

        public int zoneId { get; set; }

        public string zoneName { get; set; }
    }
}
