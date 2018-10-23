using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests.OptimizerTests
{
    [TestFixture()]
    public class RastriginTest
    {
        static double Rastrigin(Vector x)
        {
            double f = 0.0;
            f += 10.0 * x.GetDim();
            for (int i = 0; i < x.GetDim(); ++i)
            {
                f += x[i] * x[i] - 10.0 * Math.Cos(2.0 * Math.PI * x[i]);
            }
            return f;
        }

        [Test()]
        public void TestRastrigin40()
        {
            int dim = 40;
            Vector mean = Vector.Fill(dim, 3.0);
            double sigma = 2.0;
            int lamb = 1130;
            Optimizer optimizer = new Optimizer(dim, Rastrigin, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("Rastrigin40 evalCnt: {0}", evalCnt);
        }

        [Test()]
        public void TestRastrigin80()
        {
            int dim = 80;
            Vector mean = Vector.Fill(dim, 3.0);
            double sigma = 2.0;
            int lamb = 1600;
            Optimizer optimizer = new Optimizer(dim, Rastrigin, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("Rastrigin80 evalCnt: {0}", evalCnt);
        }
    }
}
