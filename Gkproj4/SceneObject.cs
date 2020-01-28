using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gkproj4
{
    public abstract class SceneObject
    {
        public enum ObjectTypeEnum { Camera, Light, Cuboid, Sphere, Cylinder, Cone};
        public ObjectTypeEnum ObjectType;
        public Vector4 Position;
        public Vector4 Rotation = new Vector4(0,0,0,0);
        public double Scale = 1;


        public virtual List<Triangle> Triangles() { return null; }

        public virtual string Name()
        {
            return "empty";
        }
    }

    public class Camera : SceneObject
    {
        public Vector4 Target;
        public Vector4 UpWorld;
        public double fov;
        public double AspectRatio;
        public double NearPlaneDistance;
        public double FarPlaneDistance;
        private static int i = 0;
        private int il;

        public Camera(Vector4 Position, Vector4 Target, Vector4 UpWorld, double fov = 1, double AspectRatio = 1, double NearPlaneDistance = 1, double FarPlaneDistance = 20)
        {
            il = i;
            i++;
            ObjectType = ObjectTypeEnum.Camera;
            this.Position = Position;
            this.Target = Target;
            this.UpWorld = UpWorld;
            if (this.UpWorld == null) this.UpWorld = new Vector4(0, 1, 0, 0);
            this.fov = fov;
            this.AspectRatio = AspectRatio;
            this.NearPlaneDistance = NearPlaneDistance;
            this.FarPlaneDistance = FarPlaneDistance;
        }

        public override string Name()
        {
            return ">Camera" + il.ToString();
        }
        public override string ToString()
        {
            return Name();
        }
    }

    public class Light : SceneObject
    {
        public Color LightColor;
        public double Intensity;
        public double Attenuation;
        private static int i = 0;
        private int il;

        public Light(Vector4 Position, Color LightColor, double Intensity, double Attenuation)
        {
            il = i;
            i++;
            ObjectType = ObjectTypeEnum.Light;
            this.Position = Position;
            this.LightColor = LightColor;
            this.Intensity = Intensity;
            this.Attenuation = Attenuation;
        }

        public override string Name()
        {
            return ">Light" + il.ToString();
        }
        public override string ToString()
        {
            return Name();
        }
    }

    public class Cuboid : SceneObject
    {
        public double Width;
        public double Height;
        public double Depth;
        private static int i = 0;
        private int il;

        public Cuboid(Vector4 Position, Vector4 Rotation, double Scale, double Width, double Height, double Depth)
        {
            il = i;
            i++;
            ObjectType = ObjectTypeEnum.Cuboid;
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.Width = Width;
            this.Height = Height;
            this.Depth = Depth;
        }
        public override List<Triangle> Triangles()
        {
            List<Triangle> p = new List<Triangle>();

            double w = Width / 2, h = Height / 2, d = Depth / 2;

            p.Add(new Triangle(
                new Vector4(w, -h, d, 1),  //x1
                new Vector4(0, 0, 1, 0),  //x1N
                new Vector4(0, 0, 0, 0),  //x1TC
                new Vector4(w, h, d, 1),   //x2
                new Vector4(0, 0, 1, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(-w, -h, d, 1), //x3
                new Vector4(0, 0, 1, 0), //x3N
                new Vector4(1, 0, 0, 0) //x3TC
                ));
            p.Add(new Triangle(
                new Vector4(-w, -h, d, 1),  //x1
                new Vector4(0, 0, 1, 0),  //x1N
                new Vector4(1, 0, 0, 0),  //x1TC
                new Vector4(w, h, d, 1),   //x2
                new Vector4(0, 0, 1, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(-w, h, d, 1), //x3
                new Vector4(0, 0, 1, 0), //x3N
                new Vector4(1, 1, 0, 0) //x3TC
                ));


            p.Add(new Triangle(
                new Vector4(w, -h, -d, 1),  //x1
                new Vector4(1, 0, 0, 0),  //x1N
                new Vector4(0, 0, 0, 0),  //x1TC
                new Vector4(w, h, -d, 1),   //x2
                new Vector4(1, 0, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(w, -h, d, 1), //x3
                new Vector4(1, 0, 0, 0), //x3N
                new Vector4(1, 0, 0, 0) //x3TC
                ));
            p.Add(new Triangle(
                new Vector4(w, -h, d, 1),  //x1
                new Vector4(1, 0, 0, 0),  //x1N
                new Vector4(1, 0, 0, 0),  //x1TC
                new Vector4(w, h, -d, 1),   //x2
                new Vector4(1, 0, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(w, h, d, 1), //x3
                new Vector4(1, 0, 0, 0), //x3N
                new Vector4(1, 1, 0, 0) //x3TC
                ));

            p.Add(new Triangle(
                new Vector4(-w, -h, -d, 1),  //x1
                new Vector4(0, 0, -1, 0),  //x1N
                new Vector4(0, 0, 0, 0),  //x1TC
                new Vector4(-w, h, -d, 1),   //x2
                new Vector4(0, 0, -1, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(w, -h, -d, 1), //x3
                new Vector4(0, 0, -1, 0), //x3N
                new Vector4(1, 0, 0, 0) //x3TC
                ));
            p.Add(new Triangle(
                new Vector4(w, -h, -d, 1),  //x1
                new Vector4(0, 0, -1, 0),  //x1N
                new Vector4(1, 0, 0, 0),  //x1TC
                new Vector4(-w, h, -d, 1),   //x2
                new Vector4(0, 0, -1, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(w, h, -d, 1), //x3
                new Vector4(0, 0, -1, 0), //x3N
                new Vector4(1, 1, 0, 0) //x3TC
                ));

            p.Add(new Triangle(
                new Vector4(-w, -h, d, 1),  //x1
                new Vector4(-1, 0, 0, 0),  //x1N
                new Vector4(0, 0, 0, 0),  //x1TC
                new Vector4(-w, h, d, 1),   //x2
                new Vector4(-1, 0, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(-w, -h, -d, 1), //x3
                new Vector4(-1, 0, 0, 0), //x3N
                new Vector4(1, 0, 0, 0) //x3TC
                ));
            p.Add(new Triangle(
                new Vector4(-w, -h, -d, 1),  //x1
                new Vector4(-1, 0, 0, 0),  //x1N
                new Vector4(1, 0, 0, 0),  //x1TC
                new Vector4(-w, h, d, 1),   //x2
                new Vector4(-1, 0, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(-w, h, -d, 1), //x3
                new Vector4(-1, 0, 0, 0), //x3N
                new Vector4(1, 1, 0, 0) //x3TC
                ));

            p.Add(new Triangle(
                new Vector4(-w, h, d, 1),  //x1
                new Vector4(0, 1, 0, 0),  //x1N
                new Vector4(0, 0, 0, 0),  //x1TC
                new Vector4(w, h, d, 1),   //x2
                new Vector4(0, 1, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(-w, h, -d, 1), //x3
                new Vector4(0, 1, 0, 0), //x3N
                new Vector4(1, 0, 0, 0) //x3TC
                ));
            p.Add(new Triangle(
                new Vector4(-w, h, -d, 1),  //x1
                new Vector4(0, 1, 0, 0),  //x1N
                new Vector4(1, 0, 0, 0),  //x1TC
                new Vector4(w, h, d, 1),   //x2
                new Vector4(0, 1, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(w, h, -d, 1), //x3
                new Vector4(0, 1, 0, 0), //x3N
                new Vector4(1, 1, 0, 0) //x3TC
                ));

            p.Add(new Triangle(
                new Vector4(w, -h, d, 1),  //x1
                new Vector4(0, -1, 0, 0),  //x1N
                new Vector4(0, 0, 0, 0),  //x1TC
                new Vector4(-w, -h, d, 1),   //x2
                new Vector4(0, -1, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(-w, -h, -d, 1), //x3
                new Vector4(0, -1, 0, 0), //x3N
                new Vector4(1, 0, 0, 0) //x3TC
                ));
            p.Add(new Triangle(
                new Vector4(w, -h, d, 1),  //x1
                new Vector4(0, -1, 0, 0),  //x1N
                new Vector4(1, 0, 0, 0),  //x1TC
                new Vector4(-w, -h, -d, 1),   //x2
                new Vector4(0, -1, 0, 0),   //x2N
                new Vector4(0, 1, 0, 0),   //x2TC
                new Vector4(w, -h, -d, 1), //x3
                new Vector4(0, -1, 0, 0), //x3N
                new Vector4(1, 1, 0, 0) //x3TC
                ));

            return p;
        }

        public override string Name()
        {
            return ">Cuboid" + il.ToString();
        }
        public override string ToString()
        {
            return Name();
        }
    }

    public class Sphere : SceneObject
    {
        public double Radius;
        public int TriangleStepHorizontal;
        public int TriangleStepVertical;
        private static int i = 0;
        private int il;

        public Sphere(Vector4 position, double radius, int triangleStepHorizontal = 8, int triangleStepVertical = 8)
        {
            il = i;
            i++;
            ObjectType = ObjectTypeEnum.Sphere; 
            Position = position;
            Radius = radius;
            TriangleStepHorizontal = triangleStepHorizontal;
            TriangleStepVertical = triangleStepVertical;
        }

        public override List<Triangle> Triangles()
        {
            List<Triangle> p = new List<Triangle>();
            double d1 = Math.PI / (double)TriangleStepHorizontal;
            double d2 = (Math.PI * 2) / (double)TriangleStepVertical;

            for (double i = 0; i != d1; i = i + d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)),  //x1N
                            new Vector4((i) / Math.PI, (j) / (Math.PI * 2), 0, 0),  //x1TC

                            new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),   //x2
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1)),   //x2N
                            new Vector4((i+d1) / Math.PI, (j+d2) / (Math.PI * 2), 0, 0),   //x2TC

                            new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1), //x3
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)), //x3N
                            new Vector4((i + d1) / Math.PI, (j) / (Math.PI * 2), 0, 0) //x3TC
                            ));
                }
            }
            for (double i = d1; i < Math.PI - d1; i = i + d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1)),  //x1N
                            new Vector4((i) / Math.PI, (j + d2) / (Math.PI * 2), 0, 0),  //x1TC

                            new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),   //x2
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1)),   //x2N
                            new Vector4((i + d1) / Math.PI, (j + d2) / (Math.PI * 2), 0, 0),   //x2TC

                            new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)),  //x1N
                            new Vector4((i) / Math.PI, (j) / (Math.PI * 2), 0, 0)  //x1TC
                            ));
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)),  //x1N
                            new Vector4((i) / Math.PI, (j) / (Math.PI * 2), 0, 0),  //x1TC

                            new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),   //x2
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1)),   //x2N
                            new Vector4((i + d1) / Math.PI, (j + d2) / (Math.PI * 2), 0, 0),   //x2TC

                            new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)),  //x1N
                            new Vector4((i + d1) / Math.PI, (j) / (Math.PI * 2), 0, 0)  //x1TC
                            ));
                }
            }
            for (double i = Math.PI - d1; i != Math.PI; i = i + d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1)),  //x1N
                            new Vector4((i) / Math.PI, (j + d2) / (Math.PI * 2), 0, 0),  //x1TC

                            new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),   //x2
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1)),   //x2N
                            new Vector4((i + d1) / Math.PI, (j + d2) / (Math.PI * 2), 0, 0),   //x2TC

                            new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)),  //x1N
                            new Vector4((i) / Math.PI, (j) / (Math.PI * 2), 0, 0)  //x1TC
                            ));
                }
            }

            return p;
        }

        public override string Name()
        {
            return ">Sphere" + il.ToString();
        }
        public override string ToString()
        {
            return Name();
        }
    }

    public class Cylinder : SceneObject
    {
        public double Height;
        public double Radius;
        public int TriangleStep;
        private static int i = 0;
        private int il;

        public Cylinder(Vector4 Position, double Height, double Radius, int TriangleStep = 10)
        {
            il = i;
            i++;
            ObjectType = ObjectTypeEnum.Cylinder;
            this.Position = Position;
            this.Height = Height;
            this.Radius = Radius;
            this.TriangleStep = TriangleStep;
            Rotation = new Vector4(Math.PI / 2, 0, 0, 0);
        }

        public override List<Triangle> Triangles()
        {
            List<Triangle> p = new List<Triangle>();
            double h = Height / 2;
            double d2 = (Math.PI * 2) / (double)TriangleStep;
            double d1 = Height / (double)TriangleStep;
            //p.Add(new Triangle(
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1

            //                new Vector4(1, -1, -1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1

            //                new Vector4(1, -1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1)  //x1
            //                ));
            for (double i = -h; i != -h + d1; i = i + d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(0, 0, i, 1),  //x1s
                            new Vector4(0,0,-1,0),  //x1N
                            new Vector4(0.5, 0.5, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i, 1),  //x1
                            new Vector4(0, 0, -1, 0),  //x1N
                            new Vector4(Radius * Math.Cos(j + d2), 1, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1),  //x1
                            new Vector4(0, 0, -1, 0),  //x1N
                            new Vector4(Radius * Math.Cos(j + d2), 1, 0, 0)  //x1TC
                            ));
                }
            }
            for (double i = -h; i < h; i = i + d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius* Math.Cos(j + d2), Radius*Math.Sin(j + d2),i, 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i, 1)),  //x1N
                            new Vector4((j + d2) / (Math.PI * 2), i, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i + d1, 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i + d1, 1)),  //x1N
                            new Vector4((j + d2) / (Math.PI * 2), i + d1, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1)),  //x1N
                            new Vector4((j) / (Math.PI * 2), i, 0, 0)  //x1TC
                            ));
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1)),  //x1N
                            new Vector4((j) / (Math.PI * 2), i, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i + d1, 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i + d1, 1)),  //x1N
                            new Vector4((j + d2) / (Math.PI * 2), i + d1, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i + d1, 1),  //x1
                            Vector4.Normalize(new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i + d1, 1)),  //x1N
                            new Vector4((j) / (Math.PI * 2), i + d1, 0, 0)  //x1TC
                            ));
                }
            }
            for (double i = h; i != h + d1; i = i + d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i, 1),  //x1
                            new Vector4(0, 0, 1, 0),  //x1N
                            new Vector4(Radius * Math.Cos(j + d2), 1, 0, 0),  //x1TC

                            new Vector4(0, 0, i, 1),  //x1s
                            new Vector4(0, 0, 1, 0),  //x1N
                            new Vector4(0.5, 0.5, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1),  //x1
                            new Vector4(0, 0, 1, 0),  //x1N
                            new Vector4(Radius * Math.Cos(j + d2), 1, 0, 0)  //x1TC
                            ));
                }
            }
            return p;
        }

        public override string Name()
        {
            return ">Cylinder" + il.ToString();
        }
        public override string ToString()
        {
            return Name();
        }
    }

    public class Cone : SceneObject
    {
        public double Height;
        public double Radius;
        public int TriangleStep;
        private static int i = 0;
        private int il;

        public Cone(Vector4 Position, double Height, double Radius, int TriangleStep = 10)
        {
            il = i;
            i++;
            ObjectType = ObjectTypeEnum.Cone;
            this.Position = Position;
            this.Height = Height;
            this.Radius = Radius;
            this.TriangleStep = TriangleStep;
            Rotation = new Vector4(Math.PI / 2, 0, 0, 0);
        }
        public override List<Triangle> Triangles()
        {
            List<Triangle> p = new List<Triangle>();
            double h = Height / 2;
            double d2 = (Math.PI * 2) / (double)TriangleStep;
            double d1 = Height / (double)TriangleStep;
            double d3 = 1 / (double)TriangleStep;
            double d4 = 1;
            //p.Add(new Triangle(
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1

            //                new Vector4(1, -1, -1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1

            //                new Vector4(1, -1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1),  //x1
            //                new Vector4(1, 1, 1, 1)  //x1
            //                ));
            for (double i = h + d1; i != h ; i = i - d1)
            {
                for (double j = 0; j < Math.PI * 2; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //   new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4(Radius * Math.Cos(j + d2), Radius * Math.Sin(j + d2), i, 1),  //x1
                            new Vector4(0, 0, 1, 0),  //x1N
                            new Vector4(Radius * Math.Cos(j + d2), 1, 0, 0),  //x1TC

                            new Vector4(0, 0, i, 1),  //x1s
                            new Vector4(0, 0, 1, 0),  //x1N
                            new Vector4(0.5, 0.5, 0, 0),  //x1TC

                            new Vector4(Radius * Math.Cos(j), Radius * Math.Sin(j), i, 1),  //x1
                            new Vector4(0, 0, 1, 0),  //x1N
                            new Vector4(Radius * Math.Cos(j + d2), 1, 0, 0)  //x1TC
                            ));
                }
            }
            for (double i = h; i > -h + d1; i = i - d1)
            {
                for (double j = 0; j < (Math.PI * 2) ; j = j + d2)
                {
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j + d2), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4((Radius * (d4 - d3)) * Math.Cos(j + d2), (Radius * (d4 - d3)) * Math.Sin(j + d2), i, 1),  //x1
                            Vector4.Normalize(new Vector4((Radius * (d4 - d3)) * Math.Cos(j + d2), (Radius * (d4 - d3)) * Math.Sin(j + d2), i, 1)),  //x1N
                            new Vector4((j + d2) / (Math.PI * 2), i, 0, 0),  //x1TC

                            new Vector4((Radius * (d4)) * Math.Cos(j + d2), (Radius * (d4)) * Math.Sin(j + d2), i + d1, 1),  //x1
                            Vector4.Normalize(new Vector4((Radius * (d4)) * Math.Cos(j + d2), (Radius * (d4)) * Math.Sin(j + d2), i + d1, 1)),  //x1N
                            new Vector4((j + d2) / (Math.PI * 2), i + d1, 0, 0),  //x1TC

                            new Vector4((Radius * (d4 - d3)) * Math.Cos(j), (Radius * (d4 - d3)) * Math.Sin(j), i, 1),  //x1
                            Vector4.Normalize(new Vector4((Radius * (d4 - d3)) * Math.Cos(j), (Radius * (d4 - d3)) * Math.Sin(j), i, 1)),  //x1N
                            new Vector4((j) / (Math.PI * 2), i, 0, 0)  //x1TC
                            ));
                    //p.Add(new Triangle(new Vector4(Radius * Math.Sin(i) * Math.Cos(j), Radius * Math.Cos(i), Radius * Math.Sin(i) * Math.Sin(j), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j + d2), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j + d2), 1),
                    //    new Vector4(Radius * Math.Sin(i + d1) * Math.Cos(j), Radius * Math.Cos(i + d1), Radius * Math.Sin(i + d1) * Math.Sin(j), 1)));
                    p.Add(new Triangle(
                            new Vector4((Radius * (d4 - d3)) * Math.Cos(j), (Radius * (d4 - d3)) * Math.Sin(j), i, 1),  //x1
                            Vector4.Normalize(new Vector4((Radius * (d4 - d3)) * Math.Cos(j), (Radius * (d4 - d3)) * Math.Sin(j), i, 1)),  //x1N
                            new Vector4((j) / (Math.PI * 2), i, 0, 0),  //x1TC

                            new Vector4((Radius * (d4)) * Math.Cos(j + d2), (Radius * (d4)) * Math.Sin(j + d2), i + d1, 1),  //x1
                            Vector4.Normalize(new Vector4((Radius * (d4)) * Math.Cos(j + d2), (Radius * (d4)) * Math.Sin(j + d2), i + d1, 1)),  //x1N
                            new Vector4((j + d2) / (Math.PI * 2), i + d1, 0, 0),  //x1TC

                            new Vector4((Radius * (d4)) * Math.Cos(j), (Radius * (d4)) * Math.Sin(j), i + d1, 1),  //x1
                            Vector4.Normalize(new Vector4((Radius * (d4)) * Math.Cos(j), (Radius * (d4)) * Math.Sin(j), i + d1, 1)),  //x1N
                            new Vector4((j) / (Math.PI * 2), i + d1, 0, 0)  //x1TC
                            ));
                }
                d4 -= d3;
            }
            return p;
        }
        public override string Name()
        {
            return ">Cone" + il.ToString();
        }
        public override string ToString()
        {
            return Name();
        }
    }
}
