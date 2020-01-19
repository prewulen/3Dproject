using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gkproj4
{
   public class Matrix4x4
    {
        public double[,] matrix = new double[4, 4];

        public Matrix4x4() {}

        public Matrix4x4(double[,] tab)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    matrix[i, j] = tab[i, j];
        }

        static public Matrix4x4 MultiplyM(Matrix4x4 Left, Matrix4x4 Right)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    output.matrix[i, j] = 0;
                    for (int k = 0; k < 4; k++)
                        output.matrix[i, j] = output.matrix[i, j] + Left.matrix[i, k] * Right.matrix[k, j];
                }

            return output;
        }

        static public Vector4 MultiplyV(Matrix4x4 Left, Vector4 Right)
        {
            Vector4 output = new Vector4();

            for (int i = 0; i < 4; i++)
            {
                output.vector[i] = 0;
                for (int j = 0; j < 4; j++)
                    output.vector[i] = output.vector[i] + Left.matrix[i, j] * Right.vector[j];
            }

            return output;
        }

        static public Matrix4x4 GetProjectionMatrix(double fov, double aspect, double near, double far)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            double ctg = 1 / Math.Tan(fov / 2);
            output.matrix[0, 0] = ctg / aspect;
            output.matrix[1, 1] = ctg;
            output.matrix[2, 2] = -(far + near) / (far - near);
            output.matrix[2, 3] = (2D * far * near) / (far - near);
            output.matrix[3, 2] = -1D;

            return output;
        }

        static public Matrix4x4 GetTranslationMatrix(double x, double y, double z)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            output.matrix[0, 0] = 1D;
            output.matrix[1, 1] = 1D;
            output.matrix[2, 2] = 1D;
            output.matrix[3, 3] = 1D;

            output.matrix[0, 3] = x;
            output.matrix[1, 3] = y;
            output.matrix[2, 3] = z;

            return output;
        }

        static public Matrix4x4 GetScaleMatrix(double x, double y, double z)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            output.matrix[0, 0] = x;
            output.matrix[1, 1] = y;
            output.matrix[2, 2] = z;
            output.matrix[3, 3] = 1D;

            return output;
        }

        static public Matrix4x4 GetRotationXMatrix(double alfa)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            double sinA = Math.Sin(alfa);
            double cosA = Math.Cos(alfa);

            output.matrix[0, 0] = 1D;
            output.matrix[1, 1] = cosA;
            output.matrix[2, 2] = cosA;
            output.matrix[3, 3] = 1D;

            output.matrix[1, 2] = -sinA;
            output.matrix[2, 1] = sinA;

            return output;
        }

        static public Matrix4x4 GetRotationYMatrix(double alfa)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            double sinA = Math.Sin(alfa);
            double cosA = Math.Cos(alfa);

            output.matrix[0, 0] = cosA;
            output.matrix[1, 1] = 1D;
            output.matrix[2, 2] = cosA;
            output.matrix[3, 3] = 1D;

            output.matrix[2, 0] = sinA;
            output.matrix[0, 2] = -sinA;

            return output;
        }

        static public Matrix4x4 GetRotationZMatrix(double alfa)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            double sinA = Math.Sin(alfa);
            double cosA = Math.Cos(alfa);

            output.matrix[0, 0] = cosA;
            output.matrix[1, 1] = cosA;
            output.matrix[2, 2] = 1D;
            output.matrix[3, 3] = 1D;

            output.matrix[1, 0] = sinA;
            output.matrix[0, 1] = -sinA;

            return output;
        }

        static public Matrix4x4 GetRotationAxisMatrix(double alfa, Vector4 axis)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            double sinA = Math.Sin(alfa);
            double cosA = Math.Cos(alfa);
            double a = axis.vector[0];
            double b = axis.vector[1];
            double c = axis.vector[2];

            output.matrix[0, 0] = a * a * (1 - cosA) + cosA;
            output.matrix[0, 1] = a * b * (1 - cosA) - c * sinA;
            output.matrix[0, 2] = a * c * (1 - cosA) + b * sinA;
            output.matrix[0, 3] = 0D;

            output.matrix[1, 0] = a * b * (1 - cosA) + c * sinA;
            output.matrix[1, 1] = b * b * (1 - cosA) + cosA;
            output.matrix[1, 2] = b * c * (1 - cosA) - a * sinA;
            output.matrix[1, 3] = 0D;

            output.matrix[2, 0] = a * c * (1 - cosA) - b * sinA;
            output.matrix[2, 1] = b * c * (1 - cosA) + a * sinA;
            output.matrix[2, 2] = c * c * (1 - cosA) + cosA;
            output.matrix[2, 3] = 0D;

            output.matrix[3, 0] = 0D;
            output.matrix[3, 1] = 0D;
            output.matrix[3, 2] = 0D;
            output.matrix[3, 3] = 1D;

            return output;
        }
    }
}
