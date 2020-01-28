using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gkproj4
{
    public class Vector4
    {
        public double[] vector = new double[4];
        public Vector4() { }
        public Vector4(double v1, double v2, double v3, double v4)
        {
            vector[0] = v1;
            vector[1] = v2;
            vector[2] = v3;
            vector[3] = v4;
        }
        public void Normalize()
        {
            double p = vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2];
            if (p <= 0) return;
            double d = Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2]);
            vector[0] /= d;
            vector[1] /= d;
            vector[2] /= d;
        }

        public static Vector4 Normalize(Vector4 v)
        {
            double p = v.vector[0] * v.vector[0] + v.vector[1] * v.vector[1] + v.vector[2] * v.vector[2];
            if (p <= 0) return new Vector4(0,0,0,0);
            double d = Math.Sqrt(v.vector[0] * v.vector[0] + v.vector[1] * v.vector[1] + v.vector[2] * v.vector[2]);
            return new Vector4(v.vector[0] / d, v.vector[1] / d, v.vector[2] / d, 0);
        }

        public double Length()
        {
            return Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2]);
        }

        public static Vector4 Cross(Vector4 v1, Vector4 v2)
        {
            Vector4 output = new Vector4();

            output.vector[0] = v1.vector[1] * v2.vector[2] - v1.vector[2] * v2.vector[1];
            output.vector[1] = v1.vector[2] * v2.vector[0] - v1.vector[0] * v2.vector[2];
            output.vector[2] = v1.vector[0] * v2.vector[1] - v1.vector[1] * v2.vector[0];
            output.vector[3] = 0;

            return output;
        }

        public static double Dot(Vector4 v1, Vector4 v2)
        {
            return (v1.vector[0] * v2.vector[0] + v1.vector[1] * v2.vector[1] + v1.vector[2] * v2.vector[2]);
        }

        public static Vector4 operator- (Vector4 a, Vector4 b)
        {
            return new Vector4(a.vector[0] - b.vector[0], a.vector[1] - b.vector[1], a.vector[2] - b.vector[2], 0);
        }
        public static Vector4 operator- (Vector4 a)
        {
            return new Vector4(-a.vector[0],-a.vector[1],-a.vector[2],-a.vector[3]);
        }

        public static Vector4 operator+ (Vector4 a, Vector4 b)
        {
            return new Vector4(a.vector[0] + b.vector[0], a.vector[1] + b.vector[1], a.vector[2] + b.vector[2], 0);
        }
        public static Vector4 operator* (Vector4 a, double b)
        {
            return new Vector4(a.vector[0] * b, a.vector[1] * b, a.vector[2] * b, a.vector[3] * b);
        }
        public static Vector4 operator/ (Vector4 a, double b)
        {
            return new Vector4(a.vector[0] / b, a.vector[1] / b, a.vector[2] / b, a.vector[3] / b);
        }
        public Vector4 Copy()
        {
            return new Vector4(vector[0], vector[1], vector[2], vector[3]);
        }
    }
}
