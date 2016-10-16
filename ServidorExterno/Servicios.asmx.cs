using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ServidorExterno
{
    /// <summary>
    /// Summary description for Servicios
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Servicios : System.Web.Services.WebService
    {
        [WebMethod]
        public List<List<string>> Servicio1(string param1, string param2, string param3)
        {
            List<List<string>> result = new List<List<string>>();
            List<string> item1 = new List<string>();
            item1.Add(param1);
            item1.Add(param2);
            item1.Add(param3);
            item1.Add("DDD");
            item1.Add("EEE");
            item1.Add("FFF");
            item1.Add("GGG");
            item1.Add("HHH");
            item1.Add("III");
            item1.Add("JJJ");

            List<string> item2 = new List<string>();
            item2.Add(param1);
            item2.Add(param2);
            item2.Add(param3);
            item2.Add("444");
            item2.Add("555");
            item2.Add("666");
            item2.Add("777");
            item2.Add("888");
            item2.Add("999");
            item2.Add("XXX");

            result.Add(item1);
            result.Add(item2);

            return result;
        }
    }
}
