using System;
using CRFMNES.Utils;

namespace CRFMNES
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int dim = 3;
            double f(Vector x) => ((x & x));
            Vector mean = Vector.Fill(dim, 0.5);
            double sigma = 0.2;
            int lamb = 6;
            Optimizer optimizer = new Optimizer(dim, f, mean, sigma, lamb);
            for (int i = 0; i < 100; ++i) {
                optimizer.OneIteration();
            }
            Console.WriteLine(optimizer.XBest);
            Console.WriteLine(optimizer.FBest);
        }
    }
}
