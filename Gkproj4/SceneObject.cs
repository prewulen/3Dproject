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
    }

    public class Camera : SceneObject
    {
        public Vector4 position;

        public Camera(Vector4 position)
        {
            ObjectType = ObjectTypeEnum.Camera;
            this.position = position;
        }
    }

    public class Light : SceneObject
    {
        public Vector4 position;
        public Color LightColor;
        public double Intensity;
        public double Range;

        public Light(Vector4 position, Color LightColor, double Intensity, double Range)
        {
            ObjectType = ObjectTypeEnum.Light;
            this.position = position;
            this.LightColor = LightColor;
            this.Intensity = Intensity;
            this.Range = Range;
        }
    }

    public class Cuboid : SceneObject
    {
        public Vector4 position;
        public double Width;
        public double Height;
        public double Depth;

        public Cuboid(Vector4 position, double Width, double Height, double Depth)
        {
            ObjectType = ObjectTypeEnum.Cuboid;
            this.position = position;
            this.Width = Width;
            this.Height = Height;
            this.Depth = Depth;
        }
    }

    public class Sphere : SceneObject
    {
        public Vector4 position;
        public double Radius;

        public Sphere(Vector4 position, double Radius)
        {
            ObjectType = ObjectTypeEnum.Sphere;
            this.position = position;
            this.Radius = Radius;
        }
    }

    public class Cylinder : SceneObject
    {
        public Vector4 position;
        public double Height;
        public double Radius;

        public Cylinder(Vector4 position, double Height, double Radius)
        {
            ObjectType = ObjectTypeEnum.Cylinder;
            this.position = position;
            this.Height = Height;
            this.Radius = Radius;
        }
    }

    public class Cone : SceneObject
    {
        public Vector4 position;
        public double Height;
        public double Radius;

        public Cone(Vector4 position, double Height, double Radius)
        {
            ObjectType = ObjectTypeEnum.Cone;
            this.position = position;
            this.Height = Height;
            this.Radius = Radius;
        }
    }
}
