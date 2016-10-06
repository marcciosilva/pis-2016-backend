using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Microsoft.AspNet.Identity;
using Utils.Login;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;

namespace UnitTest
{
    /// <summary>
    /// Summary description for AutenticacionTest
    /// </summary>
    [TestClass]
    public class AutenticacionTest
    {
        public AutenticacionTest()
        {
        }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


    }
}
