using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PruebaSubirImagenes.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // ahora tengo que mandar las imagenes de ListaImagenes (el archivo en si)
           
                var filepath = AppDomain.CurrentDomain.BaseDirectory + "Multimedia\\" + "adelante.jpeg";
                HttpContent fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filepath));

                using (var client = new HttpClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(fileContent, "file", "adelante.jpeg");

                        //call service
                        var response = client.PostAsync("http://localhost:2956/api/Multimedia", formData).Result;
                    }
                }

                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}