using NUnit.Framework;
using System;
namespace Red.Utility.Tests
{
    [TestFixture()]
    public class TestDateTimeAdditions
    {
        [Test()]
        public void TestFirstSecondOnDay()
        {
            DateTime aDate = new DateTime(2000, 1, 1, 15, 23, 12);
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 0, 0), aDate.FirstSecondOnDay());
        }

        [Test()]
        public void TestLastSecondOnDay()
        {
            DateTime aDate = new DateTime(2000, 1, 1, 15, 23, 12);
            Assert.AreEqual(new DateTime(2000, 1, 1, 23, 59, 59), aDate.LastSecondOnDay());
        }
    }
}
