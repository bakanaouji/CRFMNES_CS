using System;
using CRFMNES.Utils;

namespace CRFMNES
{
    class MainClass
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

        static double Ellipsoid(Vector x)
        {
            double f = 0.0;
            for (int i = 0; i < x.GetDim(); ++i)
            {
                double tmp = Math.Pow(1000.0, i / (x.GetDim() - 1.0)) * x[i];
                f += tmp * tmp;
            }
            return f;
        }

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

        public static void RosenbrockChain40Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 40;
                Vector mean = Vector.Fill(dim, 0.0);
                double sigma = 2.0;
                int lamb = 40;
                CRFMNES optimizer = new CRFMNES(dim, RosenbrockChain, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void RosenbrockChain80Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 80;
                Vector mean = Vector.Fill(dim, 0.0);
                double sigma = 2.0;
                int lamb = 64;
                CRFMNES optimizer = new CRFMNES(dim, RosenbrockChain, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void Ellipsoid40Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 40;
                Vector mean = Vector.Fill(dim, 3.0);
                double sigma = 2.0;
                int lamb = 16;
                CRFMNES optimizer = new CRFMNES(dim, Ellipsoid, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void Ellipsoid80Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 80;
                Vector mean = Vector.Fill(dim, 3.0);
                double sigma = 2.0;
                int lamb = 16;
                CRFMNES optimizer = new CRFMNES(dim, Ellipsoid, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void KTablet40Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 40;
                Vector mean = Vector.Fill(dim, 3.0);
                double sigma = 2.0;
                int lamb = 16;
                CRFMNES optimizer = new CRFMNES(dim, KTablet, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void KTablet80Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 80;
                Vector mean = Vector.Fill(dim, 3.0);
                double sigma = 2.0;
                int lamb = 16;
                CRFMNES optimizer = new CRFMNES(dim, KTablet, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void Rastrigin40Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 40;
                Vector mean = Vector.Fill(dim, 3.0);
                double sigma = 2.0;
                int lamb = 1130;
                CRFMNES optimizer = new CRFMNES(dim, Rastrigin, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void Rastrigin80Test()
        {
            int iteNum = 50;
            Vector evalCnts = new Vector(iteNum);
            for (int ite = 0; ite < iteNum; ++ite)
            {
                int dim = 80;
                Vector mean = Vector.Fill(dim, 3.0);
                double sigma = 2.0;
                int lamb = 1600;
                CRFMNES optimizer = new CRFMNES(dim, Rastrigin, mean, sigma, lamb);
                int evalCnt = 0;
                while (optimizer.FBest >= 1e-12)
                {
                    optimizer.OneIteration();
                    evalCnt += lamb;
                }
                evalCnts[ite] = evalCnt;
                Console.WriteLine("evalCnt: {0}", evalCnt);
            }
            double meanEvalCnt = evalCnts.Sum() / iteNum;
            double sigmaEvalCnt = (evalCnts - meanEvalCnt).Norm() / Math.Sqrt(iteNum);
            Console.WriteLine("meanEvalCnt: {0}", meanEvalCnt);
            Console.WriteLine("sigmaEvalCnt: {0}", sigmaEvalCnt);
        }

        public static void Main(string[] args)
        {
            //RosenbrockChain40Test();
            //RosenbrockChain80Test();        
            //Ellipsoid40Test();
            //Ellipsoid80Test();
            //KTablet40Test();
            //KTablet80Test();
            //Rastrigin40Test();
            Rastrigin80Test();
        }
    }
}
