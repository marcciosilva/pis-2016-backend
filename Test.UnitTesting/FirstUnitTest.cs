﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Test.UnitTesting
{
    [TestFixture]
    public class FirstUnitTest
    {
        [Test]
        public void positiveTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
