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
    }
}
