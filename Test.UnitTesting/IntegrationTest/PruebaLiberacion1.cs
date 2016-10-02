using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypeObject;
using NUnit.Framework;
using Servicios.Controllers;
using System.Net.Http;
using System.Web.Http;

namespace Test.UnitTesting
{
    [TestFixture]
    public class PruebaLiberacion1
    {
        [Test]
        public void positiveTest()
        {
            var controller = new LoginController();
            var response= controller.Login("A","A");
            var respuesta=  response.Result;
            Assert.IsTrue(respuesta.cod==0);
            var token = respuesta.response;
            //Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            //controller.Request = new HttpRequestMessage();
            //controller.Request.Headers.Add("auth", "A");
            //controller.Configuration = new HttpConfiguration();
        }
    }
}
