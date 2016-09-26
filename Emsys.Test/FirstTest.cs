using NUnit.Framework;

namespace Emsys.Test
{
    [TestFixture]
    class FirstTest
    {
        [Test]
        public void PositiveTest()
        {
            Assert.AreEqual(7, 7);
        }

        [Test]
        public void NegativeTest()
        {
            Assert.Fail("Shitty testing class");
        }
    }
}
