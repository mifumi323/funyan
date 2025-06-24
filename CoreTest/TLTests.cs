using System;
using MifuminSoft.funyan.Core;
using NUnit.Framework;

namespace MifuminSoft.funyan.CoreTest
{
    public class TLTests
    {
        [TestCase(2, 1, 2, 3)]
        [TestCase(1, 1, 1, 3)]
        [TestCase(3, 1, 3, 3)]
        [TestCase(1, 1, 0, 3)]
        [TestCase(3, 1, 4, 3)]
        public void SaturateIntTest(int expected, int min, int num, int max)
        {
            TL.Saturate(min, ref num, max);
            Assert.AreEqual(expected, num);
        }

        [TestCase(2.5f, 1.5f, 2.5f, 3.5f)]
        [TestCase(1.5f, 1.5f, 1.5f, 3.5f)]
        [TestCase(3.5f, 1.5f, 3.5f, 3.5f)]
        [TestCase(1.5f, 1.5f, 0.5f, 3.5f)]
        [TestCase(3.5f, 1.5f, 4.5f, 3.5f)]
        public void SaturateFloatTest(float expected, float min, float num, float max)
        {
            TL.Saturate(min, ref num, max);
            Assert.AreEqual(expected, num);
        }

        [TestCase(true, 1, 2, 3)]
        [TestCase(true, 1, 1, 3)]
        [TestCase(true, 1, 3, 3)]
        [TestCase(false, 1, 0, 3)]
        [TestCase(false, 1, 4, 3)]
        public void IsInIntTest(bool expected, int min, int num, int max)
        {
            var actual = TL.IsIn(min, num, max);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(true, 1.5f, 2.5f, 3.5f)]
        [TestCase(true, 1.5f, 1.5f, 3.5f)]
        [TestCase(true, 1.5f, 3.5f, 3.5f)]
        [TestCase(false, 1.5f, 0.5f, 3.5f)]
        [TestCase(false, 1.5f, 4.5f, 3.5f)]
        public void IsInFloatTest(bool expected, float min, float num, float max)
        {
            var actual = TL.IsIn(min, num, max);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1, 5, 0)]
        [TestCase(2, 1, 5, 1)]
        [TestCase(3, 1, 5, 2)]
        [TestCase(5, 1, 5, 4)]
        [TestCase(5, 1, 5, 5)]
        [TestCase(5, 5, 1, 0)]
        [TestCase(4, 5, 1, 1)]
        [TestCase(3, 5, 1, 2)]
        [TestCase(1, 5, 1, 4)]
        [TestCase(1, 5, 1, 5)]
        public void BringCloseIntTest(int expected, int from, int to, int step)
        {
            TL.BringClose(ref from, to, step);
            Assert.AreEqual(expected, from);
        }

        [TestCase(1.5f, 1.5f, 5.5f, 0.0f)]
        [TestCase(2.5f, 1.5f, 5.5f, 1.0f)]
        [TestCase(3.5f, 1.5f, 5.5f, 2.0f)]
        [TestCase(5.5f, 1.5f, 5.5f, 4.0f)]
        [TestCase(5.5f, 1.5f, 5.5f, 5.0f)]
        [TestCase(5.5f, 5.5f, 1.5f, 0.0f)]
        [TestCase(4.5f, 5.5f, 1.5f, 1.0f)]
        [TestCase(3.5f, 5.5f, 1.5f, 2.0f)]
        [TestCase(1.5f, 5.5f, 1.5f, 4.0f)]
        [TestCase(1.5f, 5.5f, 1.5f, 5.0f)]
        public void BringCloseFloatTest(float expected, float from, float to, float step)
        {
            TL.BringClose(ref from, to, step);
            Assert.AreEqual(expected, from);
        }

        class DeleteSafeTestClass : IDisposable
        {
            public bool Disposed { get; private set; } = false;
            public void Dispose()
            {
                Disposed = true;
            }
        }

        [Test]
        public void DeleteSafeTest()
        {
            var disposable = new DeleteSafeTestClass();
            var disposableRef = disposable;
            Assert.IsNotNull(disposable);
            Assert.IsFalse(disposableRef.Disposed);

            TL.DELETE_SAFE(ref disposable);
            Assert.IsNull(disposable);
            Assert.IsTrue(disposableRef.Disposed);
        }

        [TestCase(1, 1, 1, 1)]
        [TestCase(2, 1, 1, 2)]
        public void SwapTest(int expectedA, int expectedB, int a, int b)
        {
            TL.swap(ref a, ref b);
            Assert.AreEqual(expectedA, a);
            Assert.AreEqual(expectedB, b);
        }
    }
}
