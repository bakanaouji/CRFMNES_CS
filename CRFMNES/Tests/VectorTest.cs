using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests
{
    [TestFixture()]
    public class VectorTest
    {
        static bool IsEquals(float x, float y) {
            float eps = (float)(1e-5);
            return Math.Abs(x - y) < eps;
        }

        [Test()]
        public void TestConstructor()
        {
            int dim = 5;
            Vector vec = new Vector(dim);
            Assert.IsTrue(vec.GetDim() == dim);
            for (int i = 0; i < dim; ++i) {
                Assert.IsTrue(IsEquals(vec[i], 0.0f));
            }
        }

        [Test()]
        public void TestConstructor2()
        {
            float x = -1.01f;
            float y = 2.02f;
            Vector vec = new Vector(x, y);
            Assert.IsTrue(IsEquals(vec[0], x));
            Assert.IsTrue(IsEquals(vec[1], y));
        }

        [Test()]
        public void TestCopyConstructor()
        {
            int dim = 10;
            RandomExp random = new RandomExp();
            Vector srcVec = random.RandN(dim);
            Vector tarVec = new Vector(srcVec);
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(srcVec[i], tarVec[i]));
            }
            int index = 2;
            float value = -1.5f;
            srcVec[index] = value;
            for (int i = 0; i < dim; ++i)
            {
                if (i != index) {
                    Assert.IsTrue(IsEquals(srcVec[i], tarVec[i]));
                } else {
                    Assert.IsFalse(IsEquals(srcVec[index], tarVec[index]));
                }
            }
        }
    }
}
