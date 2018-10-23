using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests.OptimizerTests
{
    [TestFixture()]
    public class RosenbrockChainTest
    {
        static double RosenbrockChain(Vector x)
        {
            double f = 0.0;
            for (int i = 0; i < x.GetDim() - 1; ++i)
            {
                f += 100.0 * (x[i + 1] - x[i] * x[i]) * (x[i + 1] - x[i] * x[i]) + (x[i] - 1.0) * (x[i] - 1.0);
            }
            return f;
        }

        [Test()]
        public void TestRosenbrockChain40()
        {
            int dim = 40;
            Vector mean = Vector.Fill(dim, 0.0);
            double sigma = 2.0;
            int lamb = 40;
            Optimizer optimizer = new Optimizer(dim, RosenbrockChain, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("RosenbrockChain40 evalCnt: {0}", evalCnt);
        }

        [Test()]
        public void TestRosenbrockChain80()
        {
            int dim = 80;
            Vector mean = Vector.Fill(dim, 0.0);
            double sigma = 2.0;
            int lamb = 64;
            Optimizer optimizer = new Optimizer(dim, RosenbrockChain, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("RosenbrockChain80 evalCnt: {0}", evalCnt);
        }
    }
}
