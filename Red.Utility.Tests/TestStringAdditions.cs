using NUnit.Framework;
using System;
namespace Red.Utility.Tests
{
    [TestFixture()]
    public class TestStringAdditions
    {
        [Test()]
        public void TestNumericSuffix()
        {
            Assert.AreEqual("1", "Text1".NumericSuffix());
            Assert.AreEqual("001", "Text001".NumericSuffix());
            Assert.AreEqual("001", "Text123 001".NumericSuffix());
            Assert.AreEqual("1001", "Text1001".NumericSuffix());
            Assert.AreEqual("", "Text".NumericSuffix());
            Assert.AreEqual("", "".NumericSuffix());
        }

        [Test()]
        public void TestNextInSequence()
        {
            Assert.AreEqual("1", "".NextInSequence());
            Assert.AreEqual("1", "0".NextInSequence());
            Assert.AreEqual("1", "000".NextInSequence());
            Assert.AreEqual("Object1", "Object".NextInSequence());
            Assert.AreEqual("Object1", "Object0".NextInSequence());
            Assert.AreEqual("Object1", "Object000".NextInSequence());
            Assert.AreEqual("Object2", "Object1".NextInSequence());
            Assert.AreEqual("Object2", "Object0001".NextInSequence());
            Assert.AreEqual("Object124", "Object123".NextInSequence());
            Assert.AreEqual("Object124", "Object000123".NextInSequence());
        }
    }
}
