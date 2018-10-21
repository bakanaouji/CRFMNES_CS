using System;
using System.Diagnostics.Contracts;

namespace CRFMNES.Utils
{
    public class Vector
    {
        // コンストラクタ
        public Vector(int dim)
        {
            value = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                value[i] = 0.0;
            }
        }

        // 二次元コンストラクタ
        public Vector(double x, double y)
        {
            value = new double[2];
            value[0] = x;
            value[1] = y;
        }

        // コピーコンストラクタ
        public Vector(Vector v)
        {
            value = new double[v.value.Length];
            for (int i = 0; i < v.value.Length; ++i)
            {
                value[i] = v.value[i];
            }
        }

        // インデクサ
        public double this[int index]
        {
            set
            {
                this.value[index] = value;
            }
            get
            {
                return value[index];
            }
        }

        // 次元数
        public int GetDim()
        {
            return value.Length;
        }

        // 最大値を取得
        public double Max()
        {
            double max = value[0];
            for (int i = 1; i < value.Length; ++i)
            {
                if (max < value[i])
                {
                    max = value[i];
                }
            }
            return max;
        }

        // 和を取得
        public double Sum()
        {
            double sum = 0.0;
            for (int i = 0; i < value.Length; ++i)
            {
                sum += value[i];
            }
            return sum;
        }

        // L2ノルムの二乗を取得
        public double NormPow()
        {
            return (this & this);
        }

        // L2ノルムを取得
        public double Norm()
        {
            return Math.Sqrt(NormPow());
        }

        // 正規化
        public void Normalize()
        {
            double norm = Norm();
            for (int i = 0; i < value.Length; ++i)
            {
                value[i] /= norm;
            }
        }

        // 文字列
        public override string ToString()
        {
            string str = "Vector(";
            for (int i = 0; i < value.Length; ++i)
            {
                str += value[i].ToString();
                if (i < value.Length - 1)
                {
                    str += ", ";
                }
            }
            str += ")";
            return str;
        }

        // fill
        public static Vector Fill(int dim, double value)
        {
            return new Vector(dim) + value;
        }

        // arange
        public static Vector Arange(int start, int stop)
        {
            Contract.Ensures(Contract.Result<Vector>() != null);
            int dim = stop - start;
            Vector vec = new Vector(dim);
            for (int i = 0; i < dim; ++i)
            {
                vec[i] = (double)start + i;
            }
            return vec;
        }

        // log
        public static Vector Log(Vector v)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.GetDim(); ++i) {
                vec[i] = Math.Log(vec[i]);
            }
            return vec;
        }

        // +演算子オーバーロード
        public static Vector operator +(Vector v)
        {
            return new Vector(v);
        }

        // +演算子オーバーロード
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector vec = new Vector(v1);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] += v2.value[i];
            }
            return vec;
        }

        // +演算子オーバーロード
        public static Vector operator +(Vector v, double x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] += x;
            }
            return vec;
        }

        // +演算子オーバーロード
        public static Vector operator +(double x, Vector v)
        {
            return v + x;
        }

        // -演算子オーバーロード
        public static Vector operator -(Vector v)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] = -vec.value[i];
            }
            return vec;
        }

        // -演算子オーバーロード
        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector vec = new Vector(v1);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] -= v2.value[i];
            }
            return vec;
        }

        // -演算子オーバーロード
        public static Vector operator -(Vector v, double x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] -= x;
            }
            return vec;
        }

        // -演算子オーバーロード
        public static Vector operator -(double x, Vector v)
        {
            return -v + x;
        }

        // *演算子オーバーロード
        public static Vector operator *(Vector v1, Vector v2)
        {
            Vector vec = new Vector(v1);
            for (int i = 0; i < v2.value.Length; ++i)
            {
                vec.value[i] *= v2.value[i];
            }
            return vec;
        }

        // *演算子オーバーロード
        public static Vector operator *(Vector v, double x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] *= x;
            }
            return vec;
        }

        // *演算子オーバーロード
        public static Vector operator *(double x, Vector v)
        {
            return v * x;
        }

        // /演算子オーバーロード
        public static Vector operator /(Vector v1, Vector v2)
        {
            Vector vec = new Vector(v1);
            for (int i = 0; i < v2.value.Length; ++i)
            {
                vec.value[i] /= v2.value[i];
            }
            return vec;
        }

        // /演算子オーバーロード
        public static Vector operator /(Vector v, double x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] /= x;
            }
            return vec;
        }

        // /演算子オーバーロード
        public static Vector operator /(double x, Vector v)
        {
            return Fill(v.value.Length, x) / v;
        }

        // &演算子オーバーロード
        public static double operator &(Vector v1, Vector v2)
        {
            double ip = 0.0;
            for (int i = 0; i < v1.value.Length; ++i)
            {
                ip += v1.value[i] * v2.value[i];
            }
            return ip;
        }

        // 要素
        private double[] value;
    }
}
