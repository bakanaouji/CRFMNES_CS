using System;
using CRFMNES.Utils;

namespace CRFMNES
{
    public delegate double ObjFunc(Vector v);
    delegate double Int2Double(int n);
    delegate double VectorInt2Double(Vector x, int n);

    public class CRFMNES
    {
        public CRFMNES(int dim, ObjFunc objFunc, Vector m, double sigma, int lamb)
        {
            this.dim = dim;
            this.objFunc = objFunc;
            this.m = m;
            this.sigma = sigma;
            this.lamb = lamb;
            random = new RandomExp();

            v = random.RandN(dim) / Math.Sqrt(dim);
            D = Vector.Fill(dim, 1.0);

            wRankHat = Vector.Fill(lamb, Math.Log(lamb / 2.0 + 1.0)) - Vector.Log(Vector.Arange(1, lamb + 1));
            for (int i = 0; i < lamb; ++i)
            {
                if (wRankHat[i] < 0.0)
                {
                    wRankHat[i] = 0.0;
                }
            }
            wRank = wRankHat / wRankHat.Sum() - (1.0 / lamb);
            muEff = 1.0 / (wRank + (1.0 / lamb)).NormPow();
            cSigma = (muEff + 2.0) / (dim + muEff + 5.0);
            cc = (4.0 + muEff / dim) / (dim + 4.0 + 2.0 * muEff / dim);
            c1Cma = 2.0 / (Math.Pow(dim + 1.3, 2) + muEff);
            // initialization
            chiN = Math.Sqrt(dim) * (1.0 - 1.0 / (4.0 * dim) + 1.0 / (21.0 * dim * dim));
            pc = new Vector(dim);
            ps = new Vector(dim);
            // distance weight parameter
            hInv = GetHInv(dim);
            alphaDist = (lambF) => hInv * Math.Min(1.0, Math.Sqrt((double)lamb / dim)) * Math.Sqrt((double)lambF / lamb);
            wDistHat = (z, lambF) => Math.Exp(alphaDist(lambF) * z.Norm());
            // learning rate
            etaM = 1.0;
            etaMoveSigma = 1.0;
            etaStagSigma = (lambF) => Math.Tanh((0.024 * lambF + 0.7 * dim + 20.0) / (dim + 12.0));
            etaConvSigma = (lambF) => 2.0 * Math.Tanh((0.025 * lambF + 0.75 * dim + 10.0) / (dim + 4.0));
            c1 = (lambF) => c1Cma * (dim - 5.0) / 6.0 * ((double)lambF / lamb);
            etaB = (lambF) => Math.Tanh((Math.Min(0.02 * lambF, 3.0 * Math.Log(dim)) + 5.0) / (0.23 * dim + 25.0));

            g = 0;
            noOfEvals = 0;

            fBest = Double.PositiveInfinity;
            xBest = new Vector(dim);
        }

        public double GetHInv(int dim)
        {
            double f(double a, double b) => ((1.0 + a * a) * Math.Exp(a * a / 2.0) / 0.24) - 10.0 - dim;
            double fPrime(double a) => (1.0 / 0.24) * a * Math.Exp(a * a / 2.0) * (3.0 + a * a);
            double hI = 1.0;
            while (Math.Abs(f(hI, dim)) > 1e-10)
            {
                hI = hI - 0.5 * (f(hI, dim) / fPrime(hI));
            }
            return hI;
        }

        void SortBy(double[] evals, Vector[] x, Vector[] y, Vector[] z)
        {
            Tuple<double, Vector, Vector, Vector>[] tuples = new Tuple<double, Vector, Vector, Vector>[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                tuples[i] = new Tuple<double, Vector, Vector, Vector>(evals[i], x[i], y[i], z[i]);
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
            double normV = v.Norm();
            double normV2 = normV * normV;
            Vector vBar = v / normV;
            Vector[] y = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                y[i] = z[i] + vBar * (Math.Sqrt(1.0 + normV2) - 1.0) * (vBar & z[i]);
            }
            Vector[] x = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                x[i] = m + (y[i] * D) * sigma;
            }
            double[] evalsNoSort = new double[lamb];
            Vector[] xsNoSort = new Vector[lamb];
            for (int i = 0; i < lamb; ++i)
            {
                evalsNoSort[i] = objFunc(x[i]);
                xsNoSort[i] = x[i];
            }

            SortBy(evalsNoSort, x, y, z);
            double fB = evalsNoSort[0];
            Vector xB = x[0];

            noOfEvals += lamb;
            g += 1;
            if (fB < fBest)
            {
                fBest = fB;
                xBest = xB;
            }

            int lambF = lamb;

            // evolution path p_sigma
            Vector psBase = new Vector(dim);
            for (int i = 0; i < lamb; ++i)
            {
                psBase += z[i] * wRank[i];
            }
            ps = (1.0 - cSigma) * ps + Math.Sqrt(cSigma * (2.0 - cSigma) * muEff) * psBase;
            double psNorm = ps.Norm();
            // distance weight
            Vector wTmp = new Vector(lamb);
            for (int i = 0; i < lamb; ++i)
            {
                wTmp[i] = wRankHat[i] * wDistHat(z[i], lambF);
            }
            Vector weightsDist = wTmp / wTmp.Sum() - 1.0 / lamb;
            // switching weights and learning rate
            Vector weights = psNorm >= chiN ? weightsDist : wRank;
            double etaSigma = psNorm >= chiN ? etaMoveSigma : (psNorm >= 0.1 * chiN ? etaStagSigma(lambF) : etaConvSigma(lambF));
            double lc = psNorm >= chiN ? 1.0 : 0.0;
            // update pc, m
            Vector wxm = new Vector(dim);
            for (int i = 0; i < lamb; ++i)
            {
                wxm += (x[i] - m) * weights[i];
            }
            pc = (1.0 - cc) * pc + Math.Sqrt(cc * (2.0 - cc) * muEff) * wxm / sigma;
            m += etaM * wxm;
            // calculate s, t
            // step1
            double normV4 = normV2 * normV2;
            Vector[] exY = new Vector[lamb + 1];
            Vector[] yy = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                exY[i] = i < lamb ? y[i] : pc / D;
                yy[i] = exY[i] * exY[i];
            }
            double[] ipYvBar = new double[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipYvBar[i] = vBar & exY[i];
            }
            Vector[] yvBar = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                yvBar[i] = exY[i] * vBar;
            }
            double gammaV = 1.0 + normV2;
            Vector vBarBar = vBar * vBar;
            double alphaVD = Math.Min(1.0, Math.Sqrt(normV4 + (2.0 * Math.Sqrt(gammaV)) / vBarBar.Max()) / (2.0 + normV2));
            Vector[] t = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                t[i] = exY[i] * ipYvBar[i] - vBar * (ipYvBar[i] * ipYvBar[i] + gammaV) / 2.0;
            }
            double b = -(1.0 - alphaVD * alphaVD) * normV4 / gammaV + 2.0 * alphaVD * alphaVD;
            Vector H = Vector.Fill(dim, 2.0) - (b + 2 * alphaVD * alphaVD) * vBarBar;
            Vector invH = 1.0 / H;
            Vector[] sStep1 = new Vector[lamb + 1];
            Vector[] sStep2 = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                sStep1[i] = yy[i] - normV2 / gammaV * (yvBar[i] * ipYvBar[i]) - Vector.Fill(dim, 1.0);
            }
            double[] ipVBarT = new double[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipVBarT[i] = vBar & t[i];
                sStep2[i] = sStep1[i] - alphaVD / gammaV * ((2.0 + normV2) * (t[i] * vBar) - normV2 * vBarBar * ipVBarT[i]);
            }
            Vector invHVBarBar = invH * vBarBar;
            double[] ipSStep2InvHVBarBar = new double[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipSStep2InvHVBarBar[i] = invHVBarBar & sStep2[i];
            }
            Vector[] s = new Vector[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                s[i] = (sStep2[i] * invH) - b / (1.0 + b * (vBarBar & invHVBarBar)) * invHVBarBar * ipSStep2InvHVBarBar[i];
            }
            double[] ipSVBarBar = new double[lamb + 1];
            for (int i = 0; i < lamb + 1; ++i)
            {
                ipSVBarBar[i] = vBarBar & s[i];
            }
            for (int i = 0; i < lamb + 1; ++i)
            {
                t[i] = t[i] - alphaVD * ((2.0 + normV2) * (s[i] * vBar) - vBar * ipSVBarBar[i]);
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
            double nThRootDetAOld = Math.Exp(Vector.Log(oldD).Sum() / dim + Math.Log(1.0 + (oldV & oldV)) / (2.0 * dim));
            double nThRootDetA = Math.Exp(Vector.Log(D).Sum() / dim + Math.Log(1.0 + (v & v)) / (2.0 * dim));
            // update s, D
            Vector tmpG = new Vector(dim);
            for (int i = 0; i < lamb; ++i)
            {
                tmpG += (z[i] * z[i] - Vector.Fill(dim, 1.0)) * weights[i];
            }
            double GSigma = tmpG.Sum() / dim;
            double lSigma = psNorm >= chiN && GSigma < 0.0 ? 1.0 : 0.0;
            sigma *= Math.Exp((1.0 - lSigma) * etaSigma / 2.0 * GSigma) * nThRootDetA / nThRootDetAOld;
            D /= nThRootDetA;
        }

        public double FBest
        {
            get { return fBest; }
        }

        public Vector XBest
        {
            get { return xBest; }
        }

        private int dim;
        private ObjFunc objFunc;
        private Vector m;
        private double sigma;
        private int lamb;
        private Vector v;
        private Vector D;
        private Vector wRankHat;
        private Vector wRank;
        private double muEff;
        private double cSigma;
        private double cc;
        private double c1Cma;
        private double chiN;
        private Vector pc;
        private Vector ps;
        // distance weight parameter
        private double hInv;
        private Int2Double alphaDist;
        private VectorInt2Double wDistHat;
        // learning rate
        private double etaM;
        private double etaMoveSigma;
        private Int2Double etaStagSigma;
        private Int2Double etaConvSigma;
        private Int2Double c1;
        private Int2Double etaB;
        private int g;
        private int noOfEvals;
        private RandomExp random;

        private double fBest;
        private Vector xBest;
    }
}
