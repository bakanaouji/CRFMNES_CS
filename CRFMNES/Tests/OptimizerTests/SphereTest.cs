using NUnit.Framework;
using System;
using CRFMNES.Utils;

namespace CRFMNES.Tests.OptimizerTests
{
    [TestFixture()]
    public class SphereTest
    {
        static double Sphere(Vector x) => x & x;

        [Test()]
        public void TestSphere40()
        {
            int dim = 40;
            Vector mean = Vector.Fill(dim, 10.0);
            double sigma = 2.0;
            int lamb = 16;
            Optimizer optimizer = new Optimizer(dim, Sphere, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("Sphere40 evalCnt: {0}", evalCnt);
        }

        [Test()]
        public void TestSphere80()
        {
            int dim = 80;
            Vector mean = Vector.Fill(dim, 10.0);
            double sigma = 2.0;
            int lamb = 16;
            Optimizer optimizer = new Optimizer(dim, Sphere, mean, sigma, lamb);
            int evalCnt = 0;
            while (optimizer.FBest >= 1e-12)
            {
                optimizer.OneIteration();
                evalCnt += lamb;
            }
            Console.WriteLine("Sphere80 evalCnt: {0}", evalCnt);
        }
    }
}
