using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests
{
    [TestFixture()]
    public class VectorTest
    {
        static bool IsEquals(double x, double y)
        {
            double eps = 1e-10;
            return Math.Abs(x - y) < eps;
        }

        [Test()]
        public void TestConstructor()
        {
            int dim = 8;
            Vector vec = new Vector(dim);
            Assert.IsTrue(vec.GetDim() == dim);
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(vec[i], 0.0));
            }
        }

        [Test()]
        public void TestConstructor2()
        {
            double x = -1.01;
            double y = 2.02;
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
            double value = -1.5;
            srcVec[index] = value;
            for (int i = 0; i < dim; ++i)
            {
                if (i != index)
                {
                    Assert.IsTrue(IsEquals(srcVec[i], tarVec[i]));
                }
                else
                {
                    Assert.IsFalse(IsEquals(srcVec[index], tarVec[index]));
                }
            }
        }

        [Test()]
        public void TestIndexer()
        {
            int index = 0;
            double value = 0.5;
            int dim = 15;
            Vector vec = new Vector(dim);
            vec[index] = value;
            for (int i = 0; i < dim; ++i)
            {
                if (i == index) {
                    Assert.IsTrue(IsEquals(vec[i], value));
                }
                else {
                    Assert.IsTrue(IsEquals(vec[i], 0.0));
                }
            }
        }

        [Test()]
        public void TestMax()
        {
            int dim = 7;
            Vector vec = new Vector(dim);
            for (int i = 0; i < dim; ++i) {
                vec[i] = i * 0.7;
            }
            Assert.IsTrue(IsEquals(vec.Max(), (dim - 1) * 0.7));
        }

        [Test()]
        public void TestSum()
        {
            int dim = 5;
            Vector vec = new Vector(dim);
            vec[0] = 0.56;
            vec[1] = 0.13;
            vec[2] = 142.432;
            vec[3] = 42.55;
            vec[4] = 781.0;
            double sum = vec[0] + vec[1] + vec[2] + vec[3] + vec[4];
            Assert.IsTrue(IsEquals(vec.Sum(), sum));
        }

        [Test()]
        public void TestNormPow()
        {
            int dim = 3;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            double normPow = 0.0;
            for (int i = 0; i < dim; ++i) {
                normPow += vec[i] * vec[i];
            }
            Assert.IsTrue(IsEquals(normPow, vec.NormPow()));
        }

        [Test()]
        public void TestNorm()
        {
            int dim = 4;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            double norm = 0.0;
            for (int i = 0; i < dim; ++i)
            {
                norm += vec[i] * vec[i];
            }
            norm = Math.Sqrt(norm);
            Assert.IsTrue(IsEquals(norm, vec.Norm()));
        }

        [Test()]
        public void TestNormalize()
        {
            int dim = 5;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            Vector oldVec = new Vector(vec);
            vec.Normalize();
            for (int i = 0; i < dim; ++i) {
                Assert.IsTrue(IsEquals(oldVec[i] / vec[i], oldVec[0] / vec[0]));
            }
            Assert.IsTrue(IsEquals(vec.Norm(), 1.0));
        }

        [Test()]
        public void TestFill()
        {
            int dim = 12;
            double value = -64.12;
            Vector vec = Vector.Fill(dim, value);
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(vec[i], value));
            }
        }

        [Test()]
        public void TestArange()
        {
            int start = 4;
            int stop = 7;
            Vector vec = Vector.Arange(start, stop);
            Assert.IsTrue(IsEquals(vec.GetDim(), stop - start));
            for (int i = 0; i < stop - start; ++i)
            {
                Assert.IsTrue(IsEquals(vec[i], start + i));
            }
        }

        [Test()]
        public void TestLog()
        {
            int dim = 14;
            Vector vec = new Vector(dim);
            for (int i = 0; i < dim; ++i) {
                vec[i] = i + 1;
            }
            Vector logVec = Vector.Log(vec);
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(logVec[i], Math.Log(i + 1)));
            }
        }

        [Test()]
        public void TestUnaryOperatorPlus()
        {
            int dim = 4;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            Vector plusVec = +vec;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(vec[i], plusVec[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorPlusVec()
        {
            int dim = 3;
            RandomExp random = new RandomExp();
            Vector vec1 = random.RandN(dim);
            Vector vec2 = random.RandN(dim);
            Vector plusVec = vec1 + vec2;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(plusVec[i], vec1[i] + vec2[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorPlusDouble()
        {
            int dim = 3;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            double value = random.RandN();
            Vector plusVec = vec + value;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(plusVec[i], vec[i] + value));
            }
            plusVec = value + vec;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(plusVec[i], vec[i] + value));
            }
        }

        [Test()]
        public void TestUnaryOperatorMinus()
        {
            int dim = 4;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            Vector minusVec = -vec;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(-vec[i], minusVec[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorMinusVec()
        {
            int dim = 3;
            RandomExp random = new RandomExp();
            Vector vec1 = random.RandN(dim);
            Vector vec2 = random.RandN(dim);
            Vector minusVec = vec1 - vec2;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(minusVec[i], vec1[i] - vec2[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorMinusDouble()
        {
            int dim = 3;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            double value = random.RandN();
            Vector minusVec = vec - value;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(minusVec[i], vec[i] - value));
            }
            minusVec = value - vec;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(minusVec[i], value - vec[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorTimesVec()
        {
            int dim = 8;
            RandomExp random = new RandomExp();
            Vector vec1 = random.RandN(dim);
            Vector vec2 = random.RandN(dim);
            Vector timesVec = vec1 * vec2;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(timesVec[i], vec1[i] * vec2[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorTimesDouble()
        {
            int dim = 4;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            double value = random.RandN();
            Vector timesVec = vec * value;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(timesVec[i], vec[i] * value));
            }
            timesVec = value * vec;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(timesVec[i], vec[i] * value));
            }
        }

        [Test()]
        public void TestBinaryOperatorDevideVec()
        {
            int dim = 8;
            RandomExp random = new RandomExp();
            Vector vec1 = random.RandN(dim);
            Vector vec2 = random.RandN(dim);
            Vector devideVec = vec1 / vec2;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(devideVec[i], vec1[i] / vec2[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorDevideDouble()
        {
            int dim = 18;
            RandomExp random = new RandomExp();
            Vector vec = random.RandN(dim);
            double value = random.RandN();
            Vector devideVec = vec / value;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(devideVec[i], vec[i] / value));
            }
            devideVec = value / vec;
            for (int i = 0; i < dim; ++i)
            {
                Assert.IsTrue(IsEquals(devideVec[i], value / vec[i]));
            }
        }

        [Test()]
        public void TestBinaryOperatorAnd()
        {
            int dim = 3;
            RandomExp random = new RandomExp();
            Vector vec1 = random.RandN(dim);
            Vector vec2 = random.RandN(dim);
            double ip = vec1 & vec2;
            double ans = 0.0;
            for (int i = 0; i < dim; ++i)
            {
                ans += vec1[i] * vec2[i];
            }
            Assert.IsTrue(IsEquals(ans, ip));
        }
    }
}
