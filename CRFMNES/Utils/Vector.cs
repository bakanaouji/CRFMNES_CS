﻿using System;
namespace CRFMNES.Utils
{
    public class Vector
    {
        // コンストラクタ
        public Vector(int dim)
        {
            value = new float[dim];
            for (int i = 0; i < dim; ++i)
            {
                value[i] = 0.0f;
            }
        }

        // 二次元コンストラクタ
        public Vector(float x, float y)
        {
            value = new float[2];
            value[0] = x;
            value[1] = y;
        }

        // コピーコンストラクタ
        public Vector(Vector v)
        {
            value = new float[v.value.Length];
            for (int i = 0; i < v.value.Length; ++i)
            {
                value[i] = v.value[i];
            }
        }

        // インデクサ
        public float this[int index]
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

        // fill
        public static Vector Fill(int dim, float value)
        {
            return new Vector(dim) + value;
        }

        // arange
        public static Vector Arange(int start, int stop)
        {
            int dim = stop - start;
            Vector vec = new Vector(dim);
            for (int i = 0; i < dim; ++i)
            {
                vec[i] = (float)start + i;
            }
            return vec;
        }

        // log
        public static Vector Log(Vector v)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.GetDim(); ++i) {
                vec[i] = (float)Math.Log(vec[i]);
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
        public static Vector operator +(Vector v, float x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] += x;
            }
            return vec;
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
        public static Vector operator -(Vector v, float x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] -= x;
            }
            return vec;
        }

        // *演算子オーバーロード
        public static Vector operator *(Vector v, float x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] *= x;
            }
            return vec;
        }

        // *演算子オーバーロード
        public static float operator *(Vector v1, Vector v2)
        {
            float ip = 0.0f;
            for (int i = 0; i < v1.value.Length; ++i)
            {
                ip += v1.value[i] * v2.value[i];
            }
            return ip;
        }

        // /演算子オーバーロード
        public static Vector operator /(Vector v, float x)
        {
            Vector vec = new Vector(v);
            for (int i = 0; i < vec.value.Length; ++i)
            {
                vec.value[i] /= x;
            }
            return vec;
        }

        // 和を取得
        public float Sum()
        {
            float sum = 0.0f;
            for (int i = 0; i < this.GetDim(); ++i)
            {
                sum += this[i];
            }
            return sum;
        }

        // L2ノルムを取得
        public float Norm()
        {
            return (float)(Math.Sqrt(this * this));
        }

        // 正規化
        public void Normalize()
        {
            float norm = this.Norm();
            for (int i = 0; i < value.Length; ++i)
            {
                value[i] /= norm;
            }
        }

        // 要素
        private float[] value;
    }
}