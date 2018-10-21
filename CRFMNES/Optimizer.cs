using System;
using CRFMNES.Utils;

namespace CRFMNES
{
    public delegate float ObjFunc(Vector v);
    delegate float FloatFloat2Float(float a, float b);
    delegate float Float2Float(float a);
    delegate float Int2Float(int n);
    delegate float VectorInt2Float(Vector x, int n);

    public class Optimizer
    {
        public Optimizer(int dim, ObjFunc objFunc, Vector m, float sigma, int lamb)
        {
            this.dim = dim;
            this.objFunc = objFunc;
            this.m = m;
            this.sigma = sigma;
            this.lamb = lamb;

            random = new RandomExp();

            v = random.RandN(dim) / (float)Math.Sqrt(dim);
            D = new Vector(dim) + 1.0f;

            wRankHat = Vector.Fill(lamb, (float)Math.Log(lamb / 2.0 + 1.0)) - Vector.Arange(1, lamb + 1);
            for (int i = 0; i < lamb; ++i)
            {
                if (wRankHat[i] < 0.0f)
                {
                    wRankHat[i] = 0.0f;
                }
            }
            wRank = wRankHat / wRankHat.Sum() - (1.0f / lamb);
            muEff = (float)(1.0f / Math.Pow((wRank + (1.0f / lamb)).Norm(), 2));
            cSigma = (muEff + 2.0f) / (dim + muEff + 5.0f);
            cc = (4.0f + muEff / dim) / (dim + 4.0f + 2.0f * muEff / dim);
            c1Cma = (float)(2.0f / (Math.Pow(dim + 1.3f, 2) + muEff));
            // initialization
            chiN = (float)Math.Sqrt(dim) * (1.0f - 1.0f / (4.0f * dim) + 1.0f / (21.0f * dim * dim));
            pc = new Vector(dim);
            ps = new Vector(dim);
            // distance weight parameter
            hInv = GetHInv(dim);
            alphaDist = (lambF) => (float)(hInv * Math.Min(1.0f, Math.Sqrt((float)lamb / dim)) * Math.Sqrt((float)lambF / lamb));
            wDistHat = (z, lambF) => (float)Math.Exp(alphaDist(lambF) * z.Norm());
            // learning rate
            etaM = 1.0f;
            etaMoveSigma = 1.0f;
            etaStagSigma = (lambF) => (float)Math.Tanh((0.024f * lambF + 0.7f * dim + 20.0f) / (dim + 12.0f));
            etaConvSigma = (lambF) => (float)(2.0f * Math.Tanh((0.025f * lambF + 0.75f * dim + 10.0f) / (dim + 4.0f)));
            c1 = (lambF) => c1Cma * (dim - 5.0f) / 6.0f * ((float)lambF / lamb);
            etaB = (lambF) => (float)Math.Tanh((Math.Min(0.02f * lambF, 3.0f * Math.Log(dim)) + 5.0f) / (0.23f * dim + 25.0f));

            g = 0;
            noOfEvals = 0;

            z = new Vector[lamb];
            for (int i = 0; i < lamb; ++i) {
                z[i] = new Vector(dim);
            }
        }

        public float GetHInv(int dim)
        {
            FloatFloat2Float f = (a, b) => ((1.0f + a * a) * (float)Math.Exp(a * a / 2.0f) / 0.24f) - 10.0f - (float)dim;
            Float2Float fPrime = (a) => (1.0f / 0.24f) * a * (float)Math.Exp(a * a / 2.0f) * (3.0f + a * a);
            float hI = 1.0f;
            while(Math.Abs(f(hI, (float)dim)) > 1e-10) {
                hI = hI - 0.5f * (f(hI, (float)dim) / fPrime(hI));
            }
            return hI;
        }

        private int dim;
        private ObjFunc objFunc;
        private Vector m;
        private float sigma;
        private int lamb;
        private Vector v;
        private Vector D;
        private Vector wRankHat;
        private Vector wRank;
        private float muEff;
        private float cSigma;
        private float cc;
        private float c1Cma;
        private float chiN;
        private Vector pc;
        private Vector ps;
        // distance weight parameter
        private float hInv;
        private Int2Float alphaDist;
        private VectorInt2Float wDistHat;
        // learning rate
        private float etaM;
        private float etaMoveSigma;
        private Int2Float etaStagSigma;
        private Int2Float etaConvSigma;
        private Int2Float c1;
        private Int2Float etaB;
        private int g;
        private int noOfEvals;
        private Vector[] z;
        private RandomExp random;
    }
}
