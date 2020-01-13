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

        static public Matrix4x4 GetProjectionMatrix(int H, int W)
        {
            Matrix4x4 output = new Matrix4x4();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    output.matrix[i, j] = 0;

            output.matrix[0, 0] = (double)H / (double)W;
            output.matrix[1, 1] = 1D;
            output.matrix[2, 2] = 2D;
            output.matrix[2, 3] = -3D;
            output.matrix[3, 2] = 1D;

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
    }
}
