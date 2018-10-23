using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests.OptimizerTests
{
    [TestFixture()]
    public class EllipsoidTest
    {
        static double Ellipsoid(Vector x)
        {
            double f = 0.0;
            for (int i = 0; i < x.GetDim(); ++i)
            {
                double tmp = Math.Pow(1000.0, i / (x.GetDim() - 1.0)) * x[i];
                f += tmp * tmp;
            }
            return f;
        }

        [Test()]
        public void TestEllipsoid40()
        {
            int dim = 40;
            Vector mean = Vector.Fill(dim, 3.0);
            double sigma = 2.0;
            int lamb = 16;
            Optimizer optimizer = new Optimizer(dim, Ellipsoid, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("Ellipsoid40 evalCnt: {0}", evalCnt);
        }

        [Test()]
        public void TestEllipsoid80()
        {
            int dim = 80;
            Vector mean = Vector.Fill(dim, 3.0);
            double sigma = 2.0;
            int lamb = 16;
            Optimizer optimizer = new Optimizer(dim, Ellipsoid, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("Ellipsoid80 evalCnt: {0}", evalCnt);
        }
    }
}
