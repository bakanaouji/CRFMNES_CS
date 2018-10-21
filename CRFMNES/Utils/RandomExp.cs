using System;
namespace CRFMNES.Utils
{
    public class RandomExp
    {
        public RandomExp()
        {
            random = new Random();
        }

        public double RandN()
        {
            double rand1 = random.NextDouble();
            double rand2 = random.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(rand1)) * Math.Cos(2.0 * Math.PI * rand2);
        }

        public Vector RandN(int dim)
        {
            Vector vec = new Vector(dim);
            for (int i = 0; i < dim; ++i) {
                vec[i] = RandN();
            }
            return vec;
        }


        private Random random;
    }
}
