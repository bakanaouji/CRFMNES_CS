using System;
using CRFMNES.Utils;

namespace CRFMNES
{
    public delegate float ObjFunc(Vector v);
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
            muEff = 1.0f / (wRank + (1.0f / lamb)).NormPow();
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

        }

        public float GetHInv(int dim)
        {
            float f(float a, float b) => ((1.0f + a * a) * (float)Math.Exp(a * a / 2.0f) / 0.24f) - 10.0f - dim;
            float fPrime(float a) => (1.0f / 0.24f) * a * (float)Math.Exp(a * a / 2.0f) * (3.0f + a * a);
            float hI = 1.0f;
            while(Math.Abs(f(hI, dim)) > 1e-10) {
                hI = hI - 0.5f * (f(hI, dim) / fPrime(hI));
            }
            return hI;
        }

        void SortBy(float[] evals, Vector[] x, Vector[] y, Vector[] z)
        {
            Tuple<float, Vector, Vector, Vector>[] tuples = new Tuple<float, Vector, Vector, Vector>[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                tuples[i] = new Tuple<float, Vector, Vector, Vector>(evals[i], x[i], y[i], z[i]);
            }
            Array.Sort(tuples, ((a, b) => a.Item1.CompareTo(b.Item1)));
            for (int i = 0; i < lamb; ++i)
            {
                evals[i] = tuples[i].Item1;
                x[i] = tuples[i].Item2;
                y[i] = tuples[i].Item3;
                z[i] = tuples[i].Item4;
            }
        }

        public void OneIteration()
        {
            Vector[] z = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                z[i] = new Vector(dim);
            }
            Vector[] zHalf = new Vector[lamb / 2];
            for (int i = 0; i < lamb / 2; ++i)
            {
                zHalf[i] = random.RandN(dim);
            }
            for (int i = 0; i < lamb; ++i)
            {
                z[i] = i < lamb / 2 ? zHalf[i] : -zHalf[i - lamb / 2];
            }
            float normV = v.Norm();
            float normV2 = normV * normV;
            Vector vBar = v / normV;
            Vector[] y = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                y[i] = z[i] + vBar * (float)(Math.Sqrt(1.0f + normV2) - 1.0f) * (vBar & z[i]);
            }
            Vector[] x = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                x[i] = m + (y[i] * D) * sigma;
            }
            float[] evalsNoSort = new float[lamb];
            Vector[] xsNoSort = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                evalsNoSort[i] = objFunc(x[i]);
                xsNoSort[i] = x[i];
            }

            SortBy(evalsNoSort, x, y, z);
            float fBest = evalsNoSort[0];
            Vector xBest = x[0];

            noOfEvals += lamb;
            g += 1;

            int lambF = lamb;

            // evolution path p_sigma
            Vector psBase = new Vector(dim);
            for (int i = 0; i < dim; ++i)
            {
                psBase += z[i] * wRank[i];
            }
            ps = (1.0f - cSigma) * ps + (float)Math.Sqrt(cSigma * (2.0f - cSigma) * muEff) * psBase;
            float psNorm = ps.Norm();
            // distance weight
            Vector wTmp = new Vector(lamb);
            for (int i = 0; i < lamb; ++i)
            {
                wTmp[i] = wRankHat[i] * wDistHat(z[i], lambF);
            }
            Vector weightsDist = wTmp / wTmp.Sum() - 1.0f / lamb;
            // switching weights and learning rate
            Vector weights = psNorm >= chiN ? weightsDist : wRank;
            float etaSigma = psNorm >= chiN ? etaMoveSigma : (psNorm >= 0.1 * chiN ? etaStagSigma(lambF) : etaConvSigma(lambF));
            float lc = psNorm >= chiN ? 1.0f : 0.0f;
            // update pc, m
            Vector wxm = new Vector(lamb);
            for (int i = 0; i < lamb; ++i)
            {
                wxm += (x[i] - m) * weights[i];
            }
            pc = (1.0f - cc) * pc + (float)Math.Sqrt(cc * (2.0f - cc) * muEff) * wxm / sigma;
            m += etaM * wxm;
            // calculate s, t
            // step1
            float normV4 = normV2 * normV2;
            Vector[] exY = new Vector[lamb + 1];
            Vector[] yy = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                exY[i] = i < lamb ? y[i] : pc / D;
                yy[i] = exY[i] * exY[i];
            }
            float[] ipYvBar = new float[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipYvBar[i] = vBar & exY[i];
            }
            Vector[] yvBar = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                yvBar[i] = exY[i] * vBar;
            }
            float gammaV = 1.0f + normV2;
            Vector vBarBar = vBar * vBar;
            float alphaVD = (float)Math.Min(1.0f, Math.Sqrt(normV4 + (2.0f * Math.Sqrt(gammaV)) / vBarBar.Max()) / (2.0f + normV2));
            Vector[] t = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                t[i] = exY[i] * ipYvBar[i] - vBar * (ipYvBar[i] * ipYvBar[i] + gammaV) / 2.0f;
            }
            float b = -(1.0f - alphaVD * alphaVD) * normV4 / gammaV + 2.0f * alphaVD * alphaVD;
            Vector H = Vector.Fill(dim, 2.0f) - (b + 2 * alphaVD * alphaVD) * vBarBar;
            Vector invH = 1.0f / H;
            Vector[] sStep1 = new Vector[lamb + 1];
            Vector[] sStep2 = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                sStep1[i] = yy[i] - normV2 / gammaV * (yvBar[i] * ipYvBar[i]) + Vector.Fill(dim, 1.0f);
            }
            float[] ipVBarT = new float[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipVBarT[i] = vBar & t[i];
                sStep2[i] = sStep1[i] - alphaVD / gammaV * ((2.0f + normV2) * (t[i] * vBar) - normV2 * vBarBar * ipVBarT[i]);
            }
            Vector invHVBarBar = invH * vBarBar;
            float[] ipSStep2InvHVBarBar = new float[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipSStep2InvHVBarBar[i] = invHVBarBar & sStep2[i];
            }
            Vector[] s = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                s[i] = (sStep2[i] * invH) - b / (1.0f + b * (vBarBar & invHVBarBar)) * invHVBarBar * ipSStep2InvHVBarBar[i];
            }
            float[] ipSVBarBar = new float[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipSVBarBar[i] = vBarBar & s[i];
            }
            for (int i = 0; i < lamb + 1; ++i)
            {
                t[i] = t[i] - alphaVD * ((2.0f + normV2) * (s[i] * vBar) - vBar * ipSVBarBar[i]);
            }
            // update v, D
            Vector exW = new Vector(lamb + 1);
            for (int i = 0; i < lamb + 1; ++i)
            {
                exW[i] = i < lamb ? etaB(lambF) * weights[i] : lc * c1(lambF);
            }
            Vector oldV = new Vector(v);
            for (int i = 0; i < lamb + 1; ++i)
            {
                v += (t[i] * exW[i]) / normV;
            }
            Vector oldD = new Vector(D);
            for (int i = 0; i < lamb + 1; ++i)
            {
                D += (s[i] * exW[i]) * D;
            }
            // calculate detAold, detA
            float nThRootDetAOld = (float)Math.Exp(Vector.Log(oldD).Sum() / dim + Math.Log(1.0f + (oldV & oldV)) / (2.0f * dim));
            float nThRootDetA = (float)Math.Exp(Vector.Log(D).Sum() / dim + Math.Log(1.0f + (v & v)) / (2.0f * dim));
            // update s, D
            Vector tmpG = new Vector(dim);
            for (int i = 0; i < lamb; ++i) {
                tmpG += (z[i] * z[i] - Vector.Fill(dim, 1.0f)) * weights[i];
            }
            float GSigma = tmpG.Sum() / dim;
            float lSigma = psNorm >= chiN && GSigma < 0.0f ? 1.0f : 0.0f;
            sigma *= (float)Math.Exp((1.0f - lSigma) * etaSigma / 2.0f * GSigma) * nThRootDetAOld;
            D /= nThRootDetA;
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
        private RandomExp random;
    }
}
