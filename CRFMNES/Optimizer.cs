using System;
using CRFMNES.Utils;

namespace CRFMNES
{
    public delegate float ObjFunc(Vector x);

    public class Optimizer
    {
        public Optimizer(int dim, ObjFunc func, Vector m, float sigma, int lamb)
        {
            this.dim = dim;
        }

        private int dim;
    }
}
