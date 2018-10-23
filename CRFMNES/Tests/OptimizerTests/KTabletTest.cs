using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests.OptimizerTests
{
    [TestFixture()]
    public class KTabletTest
    {
        static double KTablet(Vector x)
        {
            double f = 0.0;
            int k = (int)(x.GetDim() / 4.0);
            for (int i = 0; i < k; ++i)
            {
                f += x[i] * x[i];
            }
            for (int i = k; i < x.GetDim(); ++i)
            {
                f += (100.0 * x[i]) * (100.0 * x[i]);
            }
            return f;
        }

        [Test()]
        public void TestKTablet40()
        {
            int dim = 40;
            Vector mean = Vector.Fill(dim, 3.0);
            double sigma = 2.0;
            int lamb = 16;
            Optimizer optimizer = new Optimizer(dim, KTablet, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("KTablet40 evalCnt: {0}", evalCnt);
        }

        [Test()]
        public void TestKTablet80()
        {
            int dim = 80;
            Vector mean = Vector.Fill(dim, 3.0);
            double sigma = 2.0;
            int lamb = 16;
            Optimizer optimizer = new Optimizer(dim, KTablet, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("KTablet80 evalCnt: {0}", evalCnt);
        }
    }
}
