using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gkproj4
{
    public class Triangle
    {
        //Point coordinates
        public Vector4 x1;
        //Normal vector
        public Vector4 x1N;
        //Tangential vector
        public Vector4 x1T;
        //Binormal vector
        public Vector4 x1B;
        //Textrue coordinates
        public Vector4 x1TC;
        //Normal map vector
        public Vector4 x1NMV;
        //Color
        public Color x1Color;

        //Point coordinates
        public Vector4 x2;
        //Normal vector
        public Vector4 x2N;
        //Tangential vector
        public Vector4 x2T;
        //Binormal vector
        public Vector4 x2B;
        //Textrue coordinates
        public Vector4 x2TC;
        //Normal map vector
        public Vector4 x2NMV;
        public Color x2Color;

        //Point coordinates
        public Vector4 x3;
        //Normal vector
        public Vector4 x3N;
        //Tangential vector
        public Vector4 x3T;
        //Binormal vector
        public Vector4 x3B;
        //Textrue coordinates
        public Vector4 x3TC;
        //Normal map vector
        public Vector4 x3NMV;
        public Color x3Color;

        public Triangle(Vector4 x1, Vector4 x1N, Vector4 x1TC, Vector4 x1NMV, 
                        Vector4 x2, Vector4 x2N, Vector4 x2TC, Vector4 x2NMV, 
                        Vector4 x3, Vector4 x3N, Vector4 x3TC, Vector4 x3NMV)
        {
            this.x1 = x1;
            this.x1N = x1N;
            this.x1T = Vector4.Cross(x1N, new Vector4(0, 1, 0, 0));
            this.x1T.Normalize();
            this.x1B = Vector4.Cross(x1N, x1T);
            this.x1B.Normalize();
            this.x1TC = x1TC;
            this.x1NMV = x1NMV;

            this.x2 = x2;
            this.x2N = x2N;
            this.x2T = Vector4.Cross(x2N, new Vector4(0, 1, 0, 0));
            this.x2T.Normalize();
            this.x2B = Vector4.Cross(x2N, x2T);
            this.x2B.Normalize();
            this.x2TC = x2TC;
            this.x2NMV = x2NMV;

            this.x3 = x3;
            this.x3N = x3N;
            this.x3T = Vector4.Cross(x3N, new Vector4(0, 1, 0, 0));
            this.x3T.Normalize();
            this.x3B = Vector4.Cross(x3N, x3T);
            this.x3B.Normalize();
            this.x3TC = x3TC;
            this.x3NMV = x3NMV;
        }

        public void Multiply(Matrix4x4 m)
        {
            x1 = Matrix4x4.MultiplyV(m, x1);
            x1N = Matrix4x4.MultiplyV(m, x1N);
            x1T = Matrix4x4.MultiplyV(m, x1T);
            x1B = Matrix4x4.MultiplyV(m, x1B);
            x2 = Matrix4x4.MultiplyV(m, x2);
            x2N = Matrix4x4.MultiplyV(m, x2N);
            x2T = Matrix4x4.MultiplyV(m, x2T);
            x2B = Matrix4x4.MultiplyV(m, x2B);
            x3 = Matrix4x4.MultiplyV(m, x3);
            x3N = Matrix4x4.MultiplyV(m, x3N);
            x3T = Matrix4x4.MultiplyV(m, x3T);
            x3B = Matrix4x4.MultiplyV(m, x3B);
            x1N.Normalize();
            x1T.Normalize();
            x1B.Normalize();
            x2N.Normalize();
            x2T.Normalize();
            x2B.Normalize();
            x3N.Normalize();
            x3T.Normalize();
            x3B.Normalize();
        }
    }
}
