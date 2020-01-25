using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Gkproj4
{
    public partial class Form1 : Form
    {
        Bitmap map;
        int time = 0;
        DateTime _lastCheckTime = DateTime.Now;
        long _frameCount = 0;
        List<SceneObject> SceneObjectList = new List<SceneObject>();
        Camera SelectedCamera;
        double color;
        bool IsMouseDown = false;
        double LastMouseX;
        double LastMouseY;
        Vector4 MoveVectorX;
        Vector4 MoveVectorY;
        GroupBox GB;
        double[,] ZBuffer;
        int asdafa = 0;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, DrawArea, new object[] { true });
            map = new Bitmap(DrawArea.Width, DrawArea.Height);
            Camera C = new Camera(new Vector4(0, 5, 2, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
            Camera C1 = new Camera(new Vector4(0, 8, 1, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C1);
            listBox1.Items.Add(C1);
            Camera C2 = new Camera(new Vector4(8, 0, 0, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C2);
            listBox1.Items.Add(C2);
            Camera C3 = new Camera(new Vector4(3, 0, 0, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C3);
            listBox1.Items.Add(C3);
            Light L1 = new Light(new Vector4(1,1,1, 1), Color.White, 1, 1);
            SceneObjectList.Add(L1);
            listBox1.Items.Add(L1);
            SelectedCamera = C;
            //Cuboid Cb = new Cuboid(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 0), 1, 1, 1.5, 1);
            //SceneObjectList.Add(Cb);
            //Cuboid Cb1 = new Cuboid(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 0), 1, 1, 1, 1);
            //SceneObjectList.Add(Cb1);
            //listBox1.Items.Add(Cb1);
            Sphere s = new Sphere(new Vector4(0, 0, 1, 1), 1, 15, 15);
            SceneObjectList.Add(s);
            listBox1.Items.Add(s);
            Sphere s1 = new Sphere(new Vector4(0, 0, 0, 1), 1, 15, 15);
            SceneObjectList.Add(s1);
            listBox1.Items.Add(s1);
            //Cylinder Cy = new Cylinder(new Vector4(0, 0, 0, 1), 2, 1, 15);
            //SceneObjectList.Add(Cy);
            //listBox1.Items.Add(Cy); 
            //Cone Co = new Cone(new Vector4(0, 0, 0, 1), 2, 1, 15);
            //SceneObjectList.Add(Co);
            //listBox1.Items.Add(Co);

            comboBox1.Items.Add("Camera");
            comboBox1.Items.Add("Light");
            comboBox1.Items.Add("Cuboid");
            comboBox1.Items.Add("Sphere");
            comboBox1.Items.Add("Cylinder");
            comboBox1.Items.Add("Cone");
            comboBox1.SelectedIndex = 0;


            listBox1.SelectedIndex = 0;


        }

        void DrawPixel(int x, int y, Color c)
        {
            if (x >= map.Width || y >= map.Height || x < 0 || y < 0) return;
            map.SetPixel(x, y, c);
        }

        void drawVertice(Point p, Color c)
        {
            for (int i = -3; i <= 3; i++)
                DrawPixel(p.X - i, p.Y - 3, c);
            for (int i = -3; i <= 3; i++)
                DrawPixel(p.X - i, p.Y + 3, c);
            for (int i = -2; i <= 2; i++)
                DrawPixel(p.X - 3, p.Y - i, c);
            for (int i = -3; i <= 3; i++)
                DrawPixel(p.X + 3, p.Y - i, c);
        }

        class EdgePointer
        {
            public int ymax;
            public double x;
            public double m;
            public EdgePointer next;
            public int p1, p2;

            public EdgePointer(int ymax, double x, double m, EdgePointer next, int p1, int p2)
            {
                this.ymax = ymax;
                this.x = x;
                this.m = m;
                this.next = next;
                this.p1 = p1;
                this.p2 = p2;
            }
        }

        void FillPoly(Polygon p)
        {


            List<Color> VerticeColor = new List<Color>();
            for (int i = 0; i < p.points.Count; i++)
            {
                VerticeColor.Add(GetColor(p.points[i].X, p.points[i].Y));
            }

            EdgePointer[] ET = new EdgePointer[2000];
            int ymin, ymax, x, ystart = p.points[0].Y;
            double m;
            int count = p.points.Count;
            int p1, p2;
            for (int i = 0; i < p.points.Count; i++)
            {
                p1 = i;
                p2 = (i + 1) % p.points.Count;
                ymin = p.points[i].Y;
                ymax = p.points[(i + 1) % p.points.Count].Y;
                x = p.points[i].X;
                m = (double)(ymin - ymax) / (double)(x - p.points[(i + 1) % p.points.Count].X);
                m = 1 / m;
                if (ymax < ymin)
                {
                    int t = ymin;
                    ymin = ymax;
                    ymax = t;
                    x = p.points[(i + 1) % p.points.Count].X;
                    p1 = p2;
                    p2 = i;
                }
                else if (ymax == ymin)
                {
                    count--;
                    continue;
                }
                if (ET[ymin] == null) ET[ymin] = new EdgePointer(ymax, x, m, null, p1, p2);
                else
                {
                    EdgePointer pt = ET[ymin];
                    while (pt.next != null)
                        pt = pt.next;
                    pt.next = new EdgePointer(ymax, x, m, null, p1, p2);
                }
                if (ystart > ymin) ystart = ymin;
            }

            List<EdgePointer> AET = new List<EdgePointer>();
            while ((count != 0 || AET.Count != 0) && ystart < DrawArea.Height)
            {
                EdgePointer pty = ET[ystart];
                while (pty != null)
                {
                    AET.Add(pty);
                    pty = pty.next;
                    count--;
                }
                AET.Sort((x1, x2) => (int)(x1.x - x2.x));
                for (int i = 0; i < AET.Count; i += 2)
                {
                    int xa1 = (int)AET[i].x + 1;
                    int xa2 = (int)AET[i + 1].x;

                    Color Left = InterpolateColor(p.points[AET[i].p1], p.points[AET[i].p2], xa1, ystart + 1, VerticeColor[AET[i].p1], VerticeColor[AET[i].p2]);
                    Color Right = InterpolateColor(p.points[AET[i + 1].p1], p.points[AET[i + 1].p2], xa2, ystart + 1, VerticeColor[AET[i + 1].p1], VerticeColor[AET[i + 1].p2]);
                    DrawPixel(xa1, ystart, Left);
                    DrawPixel(xa2, ystart, Right);
                    for (int j = xa1 + 1; j < xa2; j++)
                    {
                        DrawPixel(j, ystart, InterpolateColorL(xa1, xa2, j, Left, Right));
                    }

                }
                ystart++;
                AET.RemoveAll(x1 => x1.ymax == ystart);
                foreach (var e in AET)
                {
                    e.x += e.m;
                }
            }
        }
        
        void FillPoly(Polygon p, Triangle T, Bitmap TextureMap, Bitmap BumpMap)
        {
            List<Vector4> VerticeVector = new List<Vector4>();
            VerticeVector.Add(T.x1);
            VerticeVector.Add(T.x2);
            VerticeVector.Add(T.x3);

            List<Color> VerticeColor = new List<Color>();
            VerticeColor.Add(T.x1Color);
            VerticeColor.Add(T.x2Color);
            VerticeColor.Add(T.x3Color);

            EdgePointer[] ET = new EdgePointer[2000];
            int ymin, ymax, x, ystart = p.points[0].Y;
            double m;
            int count = p.points.Count;
            int p1, p2;
            for (int i = 0; i < p.points.Count; i++)
            {
                p1 = i;
                p2 = (i + 1) % p.points.Count;
                ymin = p.points[i].Y;
                ymax = p.points[(i + 1) % p.points.Count].Y;
                x = p.points[i].X;
                m = (double)(ymin - ymax) / (double)(x - p.points[(i + 1) % p.points.Count].X);
                m = 1 / m;
                if (ymax < ymin)
                {
                    int t = ymin;
                    ymin = ymax;
                    ymax = t;
                    x = p.points[(i + 1) % p.points.Count].X;
                    p1 = p2;
                    p2 = i;
                }
                else if (ymax == ymin)
                {
                    count--;
                    continue;
                }
                if (ET[ymin] == null) ET[ymin] = new EdgePointer(ymax, x, m, null, p1, p2);
                else
                {
                    EdgePointer pt = ET[ymin];
                    while (pt.next != null)
                        pt = pt.next;
                    pt.next = new EdgePointer(ymax, x, m, null, p1, p2);
                }
                if (ystart > ymin) ystart = ymin;
            }

            List<EdgePointer> AET = new List<EdgePointer>();
            while ((count != 0 || AET.Count != 0) && ystart < DrawArea.Height)
            {
                EdgePointer pty = ET[ystart];
                while (pty != null)
                {
                    AET.Add(pty);
                    pty = pty.next;
                    count--;
                }
                AET.Sort((x1, x2) => (int)(x1.x - x2.x));
                for (int i = 0; i < AET.Count; i += 2)
                {
                    int xa1 = (int)AET[i].x + 1;
                    int xa2 = (int)AET[i + 1].x;
                    if (xa1 < 0) xa1 = 0;
                    if (xa2 < 0) continue;
                    if (xa1 >= DrawArea.Width) continue;
                    if (xa2 >= DrawArea.Width) xa2 = DrawArea.Width - 1; 
                    double z1 = -InterpolateZBuf(p.points[AET[i].p1], p.points[AET[i].p2], xa1, ystart, VerticeVector[AET[i].p1].vector[2], VerticeVector[AET[i].p2].vector[2]);
                    double z2 = -InterpolateZBuf(p.points[AET[i + 1].p1], p.points[AET[i + 1].p2], xa2, ystart, VerticeVector[AET[i + 1].p1].vector[2], VerticeVector[AET[i + 1].p2].vector[2]);

                    Color Left = InterpolateColor(p.points[AET[i].p1], p.points[AET[i].p2], xa1, ystart + 1, VerticeColor[AET[i].p1], VerticeColor[AET[i].p2]);
                    Color Right = InterpolateColor(p.points[AET[i + 1].p1], p.points[AET[i + 1].p2], xa2, ystart + 1, VerticeColor[AET[i + 1].p1], VerticeColor[AET[i + 1].p2]);
                    if (ZBuffer[xa1, ystart] <= z1)
                    {
                        DrawPixel(xa1, ystart, Left);
                        ZBuffer[xa1, ystart] = z1;
                    }
                    if (ZBuffer[xa2, ystart] <= z2)
                    {
                        DrawPixel(xa2, ystart, Right);
                        ZBuffer[xa2, ystart] = z2;
                    }
                    for (int j = xa1 + 1; j < xa2; j++)
                    {
                        if (ZBuffer[j, ystart] <= InterpolateZBufL(xa1, xa2, j, z1, z2))
                        {
                            DrawPixel(j, ystart, InterpolateColorL(xa1, xa2, j, Left, Right));
                            ZBuffer[j, ystart] = InterpolateZBufL(xa1, xa2, j, z1, z2);
                        }
                    }
                }
                ystart++;
                AET.RemoveAll(x1 => x1.ymax == ystart);
                foreach (var e in AET)
                {
                    e.x += e.m;
                }
            }
        }

        double InterpolateZBuf(Point p1, Point p2, int x, int y, double Left, double Right)
        {
            double d1 = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            double d2 = (x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y);
            double ratio;
            if (d2 < 1e-5) ratio = 1;
            else ratio = d2 / d1;
            double ratio2 = 1D - ratio;
            //return Color.FromArgb(255, (int)(Left.R * ratio2 + Right.R * ratio) % 256, (int)(Left.G * ratio2 + Right.G * ratio) % 256, (int)(Left.B * ratio2 + Right.B * ratio) % 256);
            return Left * ratio2 + Right * ratio;
        }

        double InterpolateZBufL(int x1, int x2, int x, double Left, double Right)
        {
            double ratio = (double)Math.Abs(x - x1) / Math.Abs(x1 - x2);
            double ratio2 = 1D - ratio;
            //return Color.FromArgb(255, (int)(Left.R * ratio2 + Right.R * ratio) % 256, (int)(Left.G * ratio2 + Right.G * ratio) % 256, (int)(Left.B * ratio2 + Right.B * ratio) % 256);
            return Left * ratio2 + Right * ratio;
        }

        void FillPolyWithoutZBuf(Polygon p, Triangle T, Bitmap TextureMap, Bitmap BumpMap)
        {


            List<Color> VerticeColor = new List<Color>();
            VerticeColor.Add(T.x1Color);
            VerticeColor.Add(T.x2Color);
            VerticeColor.Add(T.x3Color);

            EdgePointer[] ET = new EdgePointer[2000];
            int ymin, ymax, x, ystart = p.points[0].Y;
            double m;
            int count = p.points.Count;
            int p1, p2;
            for (int i = 0; i < p.points.Count; i++)
            {
                p1 = i;
                p2 = (i + 1) % p.points.Count;
                ymin = p.points[i].Y;
                ymax = p.points[(i + 1) % p.points.Count].Y;
                x = p.points[i].X;
                m = (double)(ymin - ymax) / (double)(x - p.points[(i + 1) % p.points.Count].X);
                m = 1 / m;
                if (ymax < ymin)
                {
                    int t = ymin;
                    ymin = ymax;
                    ymax = t;
                    x = p.points[(i + 1) % p.points.Count].X;
                    p1 = p2;
                    p2 = i;
                }
                else if (ymax == ymin)
                {
                    count--;
                    continue;
                }
                if (ET[ymin] == null) ET[ymin] = new EdgePointer(ymax, x, m, null, p1, p2);
                else
                {
                    EdgePointer pt = ET[ymin];
                    while (pt.next != null)
                        pt = pt.next;
                    pt.next = new EdgePointer(ymax, x, m, null, p1, p2);
                }
                if (ystart > ymin) ystart = ymin;
            }

            List<EdgePointer> AET = new List<EdgePointer>();
            while ((count != 0 || AET.Count != 0) && ystart < DrawArea.Height)
            {
                EdgePointer pty = ET[ystart];
                while (pty != null)
                {
                    AET.Add(pty);
                    pty = pty.next;
                    count--;
                }
                AET.Sort((x1, x2) => (int)(x1.x - x2.x));
                for (int i = 0; i < AET.Count; i += 2)
                {
                    int xa1 = (int)AET[i].x + 1;
                    int xa2 = (int)AET[i + 1].x;

                    Color Left = InterpolateColor(p.points[AET[i].p1], p.points[AET[i].p2], xa1, ystart + 1, VerticeColor[AET[i].p1], VerticeColor[AET[i].p2]);
                    Color Right = InterpolateColor(p.points[AET[i + 1].p1], p.points[AET[i + 1].p2], xa2, ystart + 1, VerticeColor[AET[i + 1].p1], VerticeColor[AET[i + 1].p2]);
                  
                    DrawPixel(xa1, ystart, Left);
             
                    DrawPixel(xa2, ystart, Right);

                    for (int j = xa1 + 1; j < xa2; j++)
                    {
                        DrawPixel(j, ystart, InterpolateColorL(xa1, xa2, j, Left, Right));
                    }

                }
                ystart++;
                AET.RemoveAll(x1 => x1.ymax == ystart);
                foreach (var e in AET)
                {
                    e.x += e.m;
                }
            }
        }

        Color InterpolateColor(Point p1, Point p2, int x, int y, Color Left, Color Right)
        {
            double d1 = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            double d2 = (x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y);
            if(d1<d2)
            {
                double d3 = d1;
                d1 = d2;
                d2 = d3;
            }
            double ratio;
            if (d2 < 1e-5) ratio = 1;
            else ratio = d2 / d1;
            double ratio2 = 1D - ratio;
            return Color.FromArgb(255, (int)(Left.R * ratio2 + Right.R * ratio) % 256, (int)(Left.G * ratio2 + Right.G * ratio) % 256, (int)(Left.B * ratio2 + Right.B * ratio) % 256);
        }

        Color InterpolateColorL(int x1, int x2, int x, Color Left, Color Right)
        {
            double ratio = (double)Math.Abs(x - x1) / Math.Abs(x1 - x2);
            double ratio2 = 1D - ratio;
            return Color.FromArgb(255, (int)(Left.R * ratio2 + Right.R * ratio) % 256, (int)(Left.G * ratio2 + Right.G * ratio) % 256, (int)(Left.B * ratio2 + Right.B * ratio) % 256);
        }

        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(map, 0, 0);
            _frameCount++;
        }

        Color GetColor(int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            //Color TC = DrawColor;
            Color TC = Color.Red;
            double r = TC.R / 255D;
            double g = TC.G / 255D;
            double b = TC.B / 255D;
            Color N = Color.FromArgb(255);
            Color NX = Color.FromArgb(255);
            Color NY = Color.FromArgb(255);
            double DX = (NX.R - N.R) / 255D;
            double DY = (NY.R - N.R) / 255D;
            double DZ = 1D;
            double DLength = Math.Sqrt(DX * DX + DY * DY + 1);
            DX /= DLength;
            DY /= DLength;
            DZ /= DLength;
            //double ToLightX = LightP.X - x;
            //double ToLightY = LightP.Y - y;
            //double ToLightZ = LightZ;
            double ToLightX = 0 - x;
            double ToLightY = 3 - y;
            double ToLightZ = 0;
            double ToLightLength = Math.Sqrt(ToLightX * ToLightX + ToLightY * ToLightY + ToLightZ * ToLightZ);
            ToLightX /= ToLightLength;
            ToLightY /= ToLightLength;
            ToLightZ /= ToLightLength;
            double cos = DX * ToLightX + DY * ToLightY + DZ * ToLightZ;


            //r = r * (LightColor.R / 255D) * cos;
            //g = g * (LightColor.G / 255D) * cos;
            //b = b * (LightColor.B / 255D) * cos;
            r = r * (TC.R / 255D) * cos;
            g = g * (TC.G / 255D) * cos;
            b = b * (TC.B / 255D) * cos;
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            //return Color.FromArgb((int)(r * 255D) % 255, (int)(g * 255D) % 255, (int)(b * 255D) % 255);
            return Color.FromArgb(255,255,0,0);
        }

        public void line(int x, int y, int x2, int y2, Color c) //https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                DrawPixel(x, y, c);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        private void ButtonDraw3D_Click(object sender, EventArgs e)
        {
            map = new Bitmap(DrawArea.Width, DrawArea.Height);
            if (ZBufferCheckBox.Checked)
            {
                ZBuffer = new double[DrawArea.Width, DrawArea.Height];
                for (int i = 0; i < DrawArea.Width; i++)
                    for (int j = 0; j < DrawArea.Height; j++)
                        ZBuffer[i, j] = -1D;
            }

            SelectedCamera.AspectRatio = (double)DrawArea.Width / (double)DrawArea.Height;
            Vector4 D = new Vector4(
                SelectedCamera.Position.vector[0] - SelectedCamera.Target.vector[0],
                SelectedCamera.Position.vector[1] - SelectedCamera.Target.vector[1],
                SelectedCamera.Position.vector[2] - SelectedCamera.Target.vector[2],
                0);
            D.Normalize();
            Vector4 R = Vector4.Cross(SelectedCamera.UpWorld, D);
            R.Normalize();
            Vector4 U = Vector4.Cross(D, R);
            U.Normalize();
            Matrix4x4 PM = new Matrix4x4(new double[,] { 
                { 1, 0, 0, -(SelectedCamera.Position.vector[0]) }, 
                { 0, 1, 0, -(SelectedCamera.Position.vector[1]) }, 
                { 0, 0, 1, -(SelectedCamera.Position.vector[2]) }, 
                { 0, 0, 0, 1 } });
            Matrix4x4 V = new Matrix4x4(new double[,] { 
                { R.vector[0], R.vector[1], R.vector[2], 0 }, 
                { U.vector[0], U.vector[1], U.vector[2], 0 }, 
                { D.vector[0], D.vector[1], D.vector[2], 0 }, 
                { 0, 0, 0, 1 } });
            V = Matrix4x4.MultiplyM(V, PM);
            Matrix4x4 P = Matrix4x4.GetProjectionMatrix(SelectedCamera.fov, SelectedCamera.AspectRatio, SelectedCamera.NearPlaneDistance, SelectedCamera.FarPlaneDistance);
            Matrix4x4 M1= Matrix4x4.MultiplyM(P, V);

            foreach (SceneObject so in SceneObjectList)
            {
                if (so.ObjectType == SceneObject.ObjectTypeEnum.Camera) continue;
                //Matrix4x4 M = Matrix4x4.MultiplyM(M1, Matrix4x4.GetTranslationMatrix(so.Position.vector[0], so.Position.vector[1], so.Position.vector[2]));
                Matrix4x4 M = Matrix4x4.GetTranslationMatrix(so.Position.vector[0], so.Position.vector[1], so.Position.vector[2]);
                if (so.ObjectType == SceneObject.ObjectTypeEnum.Light)
                {
                    M = Matrix4x4.MultiplyM(M1, M);
                    DrawLightVertice(so, M);
                    continue;
                }
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationXMatrix(so.Rotation.vector[0]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationYMatrix(so.Rotation.vector[1]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationZMatrix(so.Rotation.vector[2]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetScaleMatrix(so.Scale, so.Scale, so.Scale));
                Matrix4x4 PVM = Matrix4x4.MultiplyM(M1, M);

                //List<Triangle> triangles = so.Triangles();
                //triangles = Clip(triangles);

                asdafa++;
                foreach (Triangle t in so.Triangles())
                {

                    if (LineFillCheckbox.Checked)
                    {
                        t.Multiply(PVM, M);
                        //todo: Obcinanie
                        if (Clip(t))
                            DrawTriangleLines(t.x1, t.x2, t.x3, Color.Red);
                    }
                    else
                    {
                            DrawTriangle(t, PVM, M);
                    }
                }
            }

            //P x V x M x p
            DrawArea.Refresh();
        }

        private bool Clip(Triangle triangle)
        {
            if(!(
                triangle.x1.vector[0] >= -triangle.x1.vector[3] &&
                triangle.x1.vector[1] >= -triangle.x1.vector[3] &&
                triangle.x1.vector[2] >= -triangle.x1.vector[3] &&
                triangle.x1.vector[0] <= triangle.x1.vector[3] &&
                triangle.x1.vector[1] <= triangle.x1.vector[3] &&
                triangle.x1.vector[2] <= triangle.x1.vector[3]
                ))
            {
                return false;
            }
            return true;
        }

        //private List<Triangle> Clip(List<Triangle> triangles)
        //{
        //    for (int i = 0; i < triangles.Count; i++)
        //    {
        //        if(!(
        //            triangles[i].x1.vector[0] >= -triangles[i].x1.vector[3] &&
        //            triangles[i].x1.vector[1] >= -triangles[i].x1.vector[3] &&
        //            triangles[i].x1.vector[2] >= -triangles[i].x1.vector[3] &&
        //            triangles[i].x1.vector[0] <= triangles[i].x1.vector[3] &&
        //            triangles[i].x1.vector[1] <= triangles[i].x1.vector[3] &&
        //            triangles[i].x1.vector[2] <= triangles[i].x1.vector[3]
        //            ))
        //        {
        //            triangles.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //    return triangles;
        //}

        private bool BackFaceCull(Vector4 v1, Vector4 v2, Vector4 v3)
        {
            if (!BackfaceCullCheckBox.Checked) return false;
            Vector4 x1 = v2 - v1;
            Vector4 x2 = v3 - v1;
            double d = x1.vector[0] * x2.vector[1] - x2.vector[0] * x1.vector[1];
            if (d < 0)
                return true;
            return false;
        }

        private void DrawRectangle(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 v4, Color c, Matrix4x4 m)
        {
            Vector4 c1 = Matrix4x4.MultiplyV(m, v1);
            Vector4 c2 = Matrix4x4.MultiplyV(m, v2);
            Vector4 c3 = Matrix4x4.MultiplyV(m, v3);
            Vector4 c4 = Matrix4x4.MultiplyV(m, v4);

            Vector4 n1 = new Vector4(c1.vector[0] / c1.vector[3], c1.vector[1] / c1.vector[3], c1.vector[2] / c1.vector[3], 1);
            Vector4 n2 = new Vector4(c2.vector[0] / c2.vector[3], c2.vector[1] / c2.vector[3], c2.vector[2] / c2.vector[3], 1);
            Vector4 n3 = new Vector4(c3.vector[0] / c3.vector[3], c3.vector[1] / c3.vector[3], c3.vector[2] / c3.vector[3], 1);
            Vector4 n4 = new Vector4(c4.vector[0] / c4.vector[3], c4.vector[1] / c4.vector[3], c4.vector[2] / c4.vector[3], 1);

            Polygon p = new Polygon();
            p.Completed = true;
            p.points.Add(new Point((int)(((n1.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n1.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n2.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n2.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n3.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n3.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n4.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n4.vector[1] / 2) + 0.5) * DrawArea.Height)));

            p.Fix();

            //FillPoly(p, c);
        }
        private void DrawTriangle(Vector4 v1, Vector4 v2, Vector4 v3, Color c)
        {

            Vector4 n1 = new Vector4(v1.vector[0] / v1.vector[3], v1.vector[1] / v1.vector[3], v1.vector[2] / v1.vector[3], 1);
            Vector4 n2 = new Vector4(v2.vector[0] / v2.vector[3], v2.vector[1] / v2.vector[3], v2.vector[2] / v2.vector[3], 1);
            Vector4 n3 = new Vector4(v3.vector[0] / v3.vector[3], v3.vector[1] / v3.vector[3], v3.vector[2] / v3.vector[3], 1);

            if (BackFaceCull(n1, n2, n3))
                return;

            Polygon p = new Polygon();
            p.Completed = true;
            p.points.Add(new Point((int)(((n1.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n1.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n2.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n2.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n3.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n3.vector[1] / 2) + 0.5) * DrawArea.Height)));


            p.Fix();

            //FillPoly(p, c);
        }
        private void DrawTriangle(Triangle t, Matrix4x4 PVM, Matrix4x4 M)
        {
            Triangle tcopy = t.Copy();
            t.Multiply(PVM, M);
            if (!Clip(t))
                return;
            Vector4 v1 =   new Vector4(t.x1.vector[0] / t.x1.vector[3], t.x1.vector[1] / t.x1.vector[3], t.x1.vector[2] / t.x1.vector[3], 1);
            Vector4 v2 =   new Vector4(t.x2.vector[0] / t.x2.vector[3], t.x2.vector[1] / t.x2.vector[3], t.x2.vector[2] / t.x2.vector[3], 1);
            Vector4 v3 =   new Vector4(t.x3.vector[0] / t.x3.vector[3], t.x3.vector[1] / t.x3.vector[3], t.x3.vector[2] / t.x3.vector[3], 1);

            if (BackFaceCull(v1, v2, v3))
                return;

            //todo: Mapowanie normalnych

            tcopy.Multiply(M);
            t.x1Color = GetColor(tcopy.x1, t.x1N);
            t.x2Color = GetColor(tcopy.x2, t.x2N);
            t.x3Color = GetColor(tcopy.x3, t.x3N);

            t.x1 = v1;
            t.x2 = v2;
            t.x3 = v3;

            Polygon p = new Polygon();
            p.Completed = true;
            p.points.Add(new Point((int)(((t.x1.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-t.x1.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((t.x2.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-t.x2.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((t.x3.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-t.x3.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.Fix();

            Console.WriteLine(asdafa + v3.vector[2]);

            if (ZBufferCheckBox.Checked)
                FillPoly(p, t, null, null);
            else
                FillPolyWithoutZBuf(p, t, null, null);

        }
        private void DrawLightVertice(SceneObject S, Matrix4x4 M)
        {
            Vector4 lpos = Matrix4x4.MultiplyV(M, S.Position);
            Vector4 v1 =   new Vector4(lpos.vector[0] / lpos.vector[3], lpos.vector[1] / lpos.vector[3], lpos.vector[2] / lpos.vector[3], 1);
            drawVertice(new Point((int)(((v1.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-v1.vector[1] / 2) + 0.5) * DrawArea.Height)), Color.Red);
        }
        private Color GetColor(Vector4 v, Vector4 vN)
        {
            double Red = 0, Green = 0, Blue = 0;
            double AmbientRatio = 0.33;
            Color DiffuseRatio = Color.Green;
            double SpecularRatio = 0.7;
            Color DiffuseColor;
            Color AmbientLight = Color.FromArgb(15,15,15);
            Color SpecularLight = Color.FromArgb(255, 255, 255);
            Red = AmbientRatio * (AmbientLight.R / 255D);
            Green = AmbientRatio * (AmbientLight.G / 255D);
            Blue = AmbientRatio * (AmbientLight.B / 255D);
            foreach(Light L in SceneObjectList.OfType<Light>())
            {
                DiffuseColor = L.LightColor;
                SpecularLight = L.LightColor;
                Vector4 Li = L.Position - v;
                double Dist = Li.Length();
                double If = (1 + 0.09 * Dist + 0.032 * Dist * Dist);
                If = 1 / If;
                If = If * L.Intensity * L.Attenuation;
                Li.Normalize();
                Vector4 R = vN.Copy();
                R = R * 2;
                R = R * Vector4.Dot(Li, vN);
                R = R - Li;
                R.Normalize();
                Vector4 V = SelectedCamera.Position - v;
                V.Normalize();
                //Vector4 LiN = Vector4.Cross(Li, vN);
                //LiN.Normalize();
                //Vector4 RV = Vector4.Cross(R, V);
                //RV.Normalize();
                //Red = Red + ((DiffuseColor.R / 255D) * DiffuseRatio * LiN.vector[0]) + ((SpecularLight.R / 255D) * SpecularRatio * RV.vector[0]);
                //Green = Green + ((DiffuseColor.G / 255D) * DiffuseRatio * LiN.vector[1]) + ((SpecularLight.G / 255D) * SpecularRatio * RV.vector[1]);
                //Blue = Blue + ((DiffuseColor.B / 255D) * DiffuseRatio * LiN.vector[2]) + ((SpecularLight.B / 255D) * SpecularRatio * RV.vector[2]);
                double LiN = Vector4.Dot(Li, vN);
                double RV = Vector4.Dot(R, V);
                RV = Math.Pow(RV, 150);
                //RV = 0;
                if (RV < 1e-5)
                    RV = 0;
                if (RV > 1)
                    RV = 1;
                if (LiN < 1e-5)
                    LiN = 0;
                if (LiN > 1)
                    LiN = 1;

                Red = Red + (
                    ((DiffuseColor.R / 255D) * (DiffuseRatio.R / 255D) * LiN) + 
                    ((SpecularLight.R / 255D) * SpecularRatio * RV)
                    ) * If;
                Green = Green + (
                    ((DiffuseColor.G / 255D) * (DiffuseRatio.G / 255D) * LiN) +
                    ((SpecularLight.G / 255D) * SpecularRatio * RV)
                    ) * If;
                Blue = Blue + (
                    ((DiffuseColor.B / 255D) * (DiffuseRatio.B / 255D) * LiN) +
                    ((SpecularLight.B / 255D) * SpecularRatio * RV)
                    ) * If;
                
            }
            if (Red < 0)
                    Red = 0;
            if (Green < 0)
                Green = 0;
            if (Blue < 0)
                Blue = 0;
            if (Red > 1)
            {
                Blue /= Red;
                Green /= Red;
                Red = 1;
            }

            if (Green > 1)
            {
                Red /= Green;
                Blue /= Green;
                Green = 1;
            }
            if (Blue > 1)
            {
                Red /= Blue;
                Green /= Blue;
                Blue = 1;
            }
            return Color.FromArgb(
                ((int)(Red * 255) % 256),
                ((int)(Green * 255) % 256),
                ((int)(Blue * 255) % 256));
        }
        private void DrawTriangleLines(Vector4 v1, Vector4 v2, Vector4 v3, Color c)
        {

            Vector4 n1 = new Vector4(v1.vector[0] / v1.vector[3], v1.vector[1] / v1.vector[3], v1.vector[2] / v1.vector[3], 1);
            Vector4 n2 = new Vector4(v2.vector[0] / v2.vector[3], v2.vector[1] / v2.vector[3], v2.vector[2] / v2.vector[3], 1);
            Vector4 n3 = new Vector4(v3.vector[0] / v3.vector[3], v3.vector[1] / v3.vector[3], v3.vector[2] / v3.vector[3], 1);

            if (BackFaceCull(n1, n2, n3))
                return;

            line((int)(((n1.vector[0] / 2) + 0.5) * DrawArea.Width),
                (int)(((-n1.vector[1] / 2) + 0.5) * DrawArea.Height),
                (int)(((n2.vector[0] / 2) + 0.5) * DrawArea.Width),
                (int)(((-n2.vector[1] / 2) + 0.5) * DrawArea.Height),
                c);
            line((int)(((n2.vector[0] / 2) + 0.5) * DrawArea.Width),
                (int)(((-n2.vector[1] / 2) + 0.5) * DrawArea.Height),
                (int)(((n3.vector[0] / 2) + 0.5) * DrawArea.Width),
                (int)(((-n3.vector[1] / 2) + 0.5) * DrawArea.Height),
                c);
            line((int)(((n3.vector[0] / 2) + 0.5) * DrawArea.Width),
                (int)(((-n3.vector[1] / 2) + 0.5) * DrawArea.Height),
                (int)(((n1.vector[0] / 2) + 0.5) * DrawArea.Width),
                (int)(((-n1.vector[1] / 2) + 0.5) * DrawArea.Height),
                c);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time == int.MaxValue)
                time = 0;
            time++;
            ButtonDraw3D_Click(null, null);
        }
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                timer1.Enabled = false;
            else
                timer1.Enabled = true;
        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
            double secondsElapsed = (DateTime.Now - _lastCheckTime).TotalSeconds;
            double fps = _frameCount / secondsElapsed;
            _frameCount = 0;
            labelFps.Text = "fps: " + ((int)fps).ToString();
            _lastCheckTime = DateTime.Now;
        }

        private void DrawArea_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IsMouseDown = true;
            LastMouseX = e.X;
            LastMouseY = e.Y;
            Vector4 D = new Vector4(
                SelectedCamera.Position.vector[0] - SelectedCamera.Target.vector[0],
                SelectedCamera.Position.vector[1] - SelectedCamera.Target.vector[1],
                SelectedCamera.Position.vector[2] - SelectedCamera.Target.vector[2],
                0);
            D.Normalize();
            Vector4 R = Vector4.Cross(SelectedCamera.UpWorld, D);
            R.Normalize();
            Vector4 U = Vector4.Cross(D, R);
            U.Normalize();
            MoveVectorX = R;
            MoveVectorY = U;
        }
        private void DrawArea_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IsMouseDown = false;
        }
        private void DrawArea_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                
                    if (Keyboard.IsKeyDown(Key.X) && 
                    SceneObjectList[listBox1.SelectedIndex].ObjectType != SceneObject.ObjectTypeEnum.Camera)
                    {
                        switch (e.Button)
                        {
                            case (MouseButtons.Left):
                                SceneObjectList[listBox1.SelectedIndex].Position.vector[0] += ((e.X - LastMouseX) / 500);
                                break;
                            case (MouseButtons.Middle):
                                SceneObjectList[listBox1.SelectedIndex].Position.vector[1] += (-(e.Y - LastMouseY) / 500);
                                break;
                            case (MouseButtons.Right):
                                SceneObjectList[listBox1.SelectedIndex].Position.vector[2] += ((e.X - LastMouseX) / 500);
                                break;
                        }
                    }
                    else if (Keyboard.IsKeyDown(Key.C) &&
                    SceneObjectList[listBox1.SelectedIndex].ObjectType != SceneObject.ObjectTypeEnum.Camera)
                    {
                        switch (e.Button)
                        {
                            case (MouseButtons.Left):
                                SceneObjectList[listBox1.SelectedIndex].Rotation.vector[0] += ((e.X - LastMouseX) / 500);
                                break;
                            case (MouseButtons.Middle):
                                SceneObjectList[listBox1.SelectedIndex].Rotation.vector[1] += ((e.X - LastMouseX) / 500);
                                break;
                            case (MouseButtons.Right):
                                SceneObjectList[listBox1.SelectedIndex].Rotation.vector[2] += ((e.X - LastMouseX) / 500);
                                break;
                        }
                    }
                    else if (Keyboard.IsKeyDown(Key.V) && e.Button == MouseButtons.Left &&
                    SceneObjectList[listBox1.SelectedIndex].ObjectType != SceneObject.ObjectTypeEnum.Camera &&
                    SceneObjectList[listBox1.SelectedIndex].ObjectType != SceneObject.ObjectTypeEnum.Light)
                    {
                        SceneObjectList[listBox1.SelectedIndex].Scale += ((e.X - LastMouseX) / 500);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Middle)
                        {
                            Matrix4x4 m = Matrix4x4.GetTranslationMatrix(
                                (((e.X - LastMouseX) * MoveVectorX.vector[0]) / 100) + ((-(e.Y - LastMouseY) * MoveVectorY.vector[0]) / 100),
                                (((e.X - LastMouseX) * MoveVectorX.vector[1]) / 100) + ((-(e.Y - LastMouseY) * MoveVectorY.vector[1]) / 100),
                                (((e.X - LastMouseX) * MoveVectorX.vector[2]) / 100) + ((-(e.Y - LastMouseY) * MoveVectorY.vector[2]) / 100));
                            SelectedCamera.Position = Matrix4x4.MultiplyV(m, SelectedCamera.Position);
                            SelectedCamera.Target = Matrix4x4.MultiplyV(m, SelectedCamera.Target);
                        }
                        if (e.Button == MouseButtons.Left)
                        {
                            Vector4 P = SelectedCamera.Position;
                            Vector4 T = SelectedCamera.Target - P;
                            Matrix4x4 M = Matrix4x4.GetRotationAxisMatrix((-(e.Y - LastMouseY) / 500), MoveVectorX);
                            M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationAxisMatrix((-(e.X - LastMouseX) / 500), MoveVectorY));
                            T = Matrix4x4.MultiplyV(M, T);
                            T = T + P;
                            T.vector[3] = 1;
                            SelectedCamera.Target = T;
                        }
                    }
                
                LastMouseX = e.X;
                LastMouseY = e.Y;
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SceneObject so = SceneObjectList[listBox1.SelectedIndex];
            flowLayoutPanel1.Controls.Remove(GB);
            switch(so.ObjectType)
            {
                case (SceneObject.ObjectTypeEnum.Camera):
                    SelectedCamera = (Camera)so;
                    GenerateCameraControls();
                    break;
                case (SceneObject.ObjectTypeEnum.Light):
                    GenerateLightControls();
                    break;
                case (SceneObject.ObjectTypeEnum.Cuboid):
                    GenerateCuboidControls();
                    break;
                case (SceneObject.ObjectTypeEnum.Sphere):
                    GenerateSphereControls();
                    break;
                case (SceneObject.ObjectTypeEnum.Cylinder):
                    GenerateCylinderControls();
                    break;
                case (SceneObject.ObjectTypeEnum.Cone):
                    GenerateConeControls();
                    break;
            }
        }

        private void GenerateCameraControls()
        {
            GB = new GroupBox();
            GB.Text = SelectedCamera.Name() + " options";
            GB.Size = new Size(141, 188);

            GB.Controls.Add(new Label() { Text = "fov", Location = new Point(6, 16), Size = new Size(60, 15) });

            TrackBar TB = new TrackBar();
            TB.Maximum = 180;
            TB.Minimum = 10;
            TB.Value = (int)(SelectedCamera.fov * 100);
            TB.Location = new Point(3, 37);
            TB.Size = new Size(135, 45);
            TB.ValueChanged += TrackBarChangeFov;
            GB.Controls.Add(TB);

            GB.Controls.Add(new Label() { Text = "Far Plane", Location = new Point(6, 80), Size = new Size(60, 15) });

            GB.Controls.Add(new Label() { Text = "Near Plane", Location = new Point(76, 80), Size = new Size(60, 15) });

            NumericUpDown N1 = new NumericUpDown();
            N1.Minimum = -100;
            N1.Maximum = 100;
            N1.Value = (decimal)SelectedCamera.FarPlaneDistance;
            N1.ValueChanged += NumericChangeFar;
            N1.Size = new Size(50, 20);
            N1.Location = new Point(9, 95);
            GB.Controls.Add(N1);

            NumericUpDown N2 = new NumericUpDown();
            N2.Minimum = -100;
            N2.Maximum = 100;
            N2.Value = (decimal)SelectedCamera.NearPlaneDistance;
            N2.ValueChanged += NumericChangeNear;
            N2.Size = new Size(50, 20);
            N2.Location = new Point(79, 95);
            GB.Controls.Add(N2);

            flowLayoutPanel1.Controls.Add(GB);
        }
        private void TrackBarChangeFov(object sender, EventArgs e)
        {
            SelectedCamera.fov = (double)((TrackBar)sender).Value / 100;
        }
        private void NumericChangeNear(object sender, EventArgs e)
        {
            SelectedCamera.NearPlaneDistance = (double)((NumericUpDown)sender).Value;
        }
        private void NumericChangeFar(object sender, EventArgs e)
        {
            SelectedCamera.FarPlaneDistance = (double)((NumericUpDown)sender).Value;
        }

        private void GenerateLightControls()
        {
            Light l = (Light)SceneObjectList[listBox1.SelectedIndex];
            GB = new GroupBox();
            GB.Text = l.Name() + " options";
            GB.Size = new Size(141, 220);

            GB.Controls.Add(new Label() { Text = "Intensity", Location = new Point(6, 16), Size = new Size(60, 15) });

            TrackBar TB = new TrackBar();
            TB.Maximum = 100;
            TB.Minimum = 1;
            TB.Value = (int)(l.Intensity * 100);
            TB.Location = new Point(3, 37);
            TB.Size = new Size(135, 45);
            TB.ValueChanged += TrackBarChangeIntensity;
            GB.Controls.Add(TB);

            GB.Controls.Add(new Label() { Text = "Attenuation", Location = new Point(6, 80), Size = new Size(70, 15) });

            TrackBar TB1 = new TrackBar();
            TB1.Maximum = 100;
            TB1.Minimum = 1;
            TB1.Value = (int)(l.Attenuation * 100);
            TB1.Location = new Point(9, 95);
            TB1.Size = new Size(135, 45);
            TB1.ValueChanged += TrackBarChangeAttenuation;
            GB.Controls.Add(TB1);

            GB.Controls.Add(new Label() { Text = "Color", Location = new Point(6, 144), Size = new Size(60, 15) });

            Button B1 = new Button();
            B1.Location = new Point(6, 159);
            B1.Size = new Size(135, 45);
            B1.Text = "Change Color";
            B1.BackColor = l.LightColor;
            B1.Click += ButtonChangeLightColor;
            GB.Controls.Add(B1);

            flowLayoutPanel1.Controls.Add(GB);
        }
        private void TrackBarChangeIntensity(object sender, EventArgs e)
        {
            ((Light)SceneObjectList[listBox1.SelectedIndex]).Intensity = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeAttenuation(object sender, EventArgs e)
        {
            ((Light)SceneObjectList[listBox1.SelectedIndex]).Attenuation = (double)((TrackBar)sender).Value / 100;
        }
        private void ButtonChangeLightColor(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ((Button)sender).BackColor = colorDialog1.Color;
                ((Light)SceneObjectList[listBox1.SelectedIndex]).LightColor = colorDialog1.Color;
            }
        }

        private void GenerateCuboidControls()
        {
            Cuboid c = (Cuboid)SceneObjectList[listBox1.SelectedIndex];
            GB = new GroupBox();
            GB.Text = c.Name() + " options";
            GB.Size = new Size(141, 220);

            GB.Controls.Add(new Label() { Text = "Height", Location = new Point(6, 16), Size = new Size(60, 15) });

            TrackBar TB = new TrackBar();
            TB.Maximum = 1000;
            TB.Minimum = 1;
            TB.Value = (int)(c.Height * 100);
            TB.Location = new Point(3, 37);
            TB.Size = new Size(135, 45);
            TB.ValueChanged += TrackBarChangeCuboidHeight;
            GB.Controls.Add(TB);

            GB.Controls.Add(new Label() { Text = "Width", Location = new Point(6, 80), Size = new Size(70, 15) });

            TrackBar TB1 = new TrackBar();
            TB1.Maximum = 1000;
            TB1.Minimum = 1;
            TB1.Value = (int)(c.Width * 100);
            TB1.Location = new Point(9, 95);
            TB1.Size = new Size(135, 45);
            TB1.ValueChanged += TrackBarChangeCuboidWidth;
            GB.Controls.Add(TB1);

            GB.Controls.Add(new Label() { Text = "Depth", Location = new Point(6, 144), Size = new Size(60, 15) });

            TrackBar TB2 = new TrackBar();
            TB2.Maximum = 1000;
            TB2.Minimum = 1;
            TB2.Value = (int)(c.Depth * 100);
            TB2.Location = new Point(9, 159);
            TB2.Size = new Size(135, 45);
            TB2.ValueChanged += TrackBarChangeCuboidDepth;
            GB.Controls.Add(TB2);

            flowLayoutPanel1.Controls.Add(GB);
        }
        private void TrackBarChangeCuboidHeight(object sender, EventArgs e)
        {
            ((Cuboid)SceneObjectList[listBox1.SelectedIndex]).Height = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeCuboidDepth(object sender, EventArgs e)
        {
            ((Cuboid)SceneObjectList[listBox1.SelectedIndex]).Depth = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeCuboidWidth(object sender, EventArgs e)
        {
            ((Cuboid)SceneObjectList[listBox1.SelectedIndex]).Width = (double)((TrackBar)sender).Value / 100;
        }

        private void GenerateSphereControls()
        {
            Sphere s = (Sphere)SceneObjectList[listBox1.SelectedIndex];
            GB = new GroupBox();
            GB.Text = s.Name() + " options";
            GB.Size = new Size(141, 220);

            GB.Controls.Add(new Label() { Text = "Radius", Location = new Point(6, 16), Size = new Size(60, 15) });

            TrackBar TB = new TrackBar();
            TB.Maximum = 1000;
            TB.Minimum = 1;
            TB.Value = (int)(s.Radius * 100);
            TB.Location = new Point(3, 37);
            TB.Size = new Size(135, 45);
            TB.ValueChanged += TrackBarChangeSphereRadius;
            GB.Controls.Add(TB);

            GB.Controls.Add(new Label() { Text = "Horizontal step", Location = new Point(6, 80), Size = new Size(70, 15) });

            TrackBar TB1 = new TrackBar();
            TB1.Maximum = 60;
            TB1.Minimum = 2;
            TB1.Value = s.TriangleStepHorizontal;
            TB1.Location = new Point(9, 95);
            TB1.Size = new Size(135, 45);
            TB1.ValueChanged += TrackBarChangeSphereHorizontalStep;
            GB.Controls.Add(TB1);

            GB.Controls.Add(new Label() { Text = "Veritical step", Location = new Point(6, 144), Size = new Size(60, 15) });

            TrackBar TB2 = new TrackBar();
            TB2.Maximum = 60;
            TB2.Minimum = 3;
            TB2.Value = s.TriangleStepVertical;
            TB2.Location = new Point(9, 159);
            TB2.Size = new Size(135, 45);
            TB2.ValueChanged += TrackBarChangeSphereVerticalStep;
            GB.Controls.Add(TB2);

            flowLayoutPanel1.Controls.Add(GB);
        }
        private void TrackBarChangeSphereRadius(object sender, EventArgs e)
        {
            ((Sphere)SceneObjectList[listBox1.SelectedIndex]).Radius = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeSphereHorizontalStep(object sender, EventArgs e)
        {
            ((Sphere)SceneObjectList[listBox1.SelectedIndex]).TriangleStepHorizontal = ((TrackBar)sender).Value;
        }
        private void TrackBarChangeSphereVerticalStep(object sender, EventArgs e)
        {
            ((Sphere)SceneObjectList[listBox1.SelectedIndex]).TriangleStepVertical = ((TrackBar)sender).Value;
        }

        private void GenerateCylinderControls()
        {
            Cylinder c = (Cylinder)SceneObjectList[listBox1.SelectedIndex];
            GB = new GroupBox();
            GB.Text = c.Name() + " options";
            GB.Size = new Size(141, 220);

            GB.Controls.Add(new Label() { Text = "Height", Location = new Point(6, 16), Size = new Size(60, 15) });

            TrackBar TB = new TrackBar();
            TB.Maximum = 1000;
            TB.Minimum = 1;
            TB.Value = (int)(c.Height * 100);
            TB.Location = new Point(3, 37);
            TB.Size = new Size(135, 45);
            TB.ValueChanged += TrackBarChangeCylinderHeight;
            GB.Controls.Add(TB);

            GB.Controls.Add(new Label() { Text = "Radius", Location = new Point(6, 80), Size = new Size(70, 15) });

            TrackBar TB1 = new TrackBar();
            TB1.Maximum = 1000;
            TB1.Minimum = 1;
            TB1.Value = (int)(c.Radius * 100);
            TB1.Location = new Point(9, 95);
            TB1.Size = new Size(135, 45);
            TB1.ValueChanged += TrackBarChangeCylinderRadius;
            GB.Controls.Add(TB1);

            GB.Controls.Add(new Label() { Text = "Triangle step", Location = new Point(6, 144), Size = new Size(60, 15) });

            TrackBar TB2 = new TrackBar();
            TB2.Maximum = 30;
            TB2.Minimum = 3;
            TB2.Value = c.TriangleStep;
            TB2.Location = new Point(9, 159);
            TB2.Size = new Size(135, 45);
            TB2.ValueChanged += TrackBarChangeCylinderStep;
            GB.Controls.Add(TB2);

            flowLayoutPanel1.Controls.Add(GB);
        }
        private void TrackBarChangeCylinderHeight(object sender, EventArgs e)
        {
            ((Cylinder)SceneObjectList[listBox1.SelectedIndex]).Height = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeCylinderRadius(object sender, EventArgs e)
        {
            ((Cylinder)SceneObjectList[listBox1.SelectedIndex]).Radius = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeCylinderStep(object sender, EventArgs e)
        {
            ((Cylinder)SceneObjectList[listBox1.SelectedIndex]).TriangleStep = ((TrackBar)sender).Value;
        }

        private void GenerateConeControls()
        {
            Cone c = (Cone)SceneObjectList[listBox1.SelectedIndex];
            GB = new GroupBox();
            GB.Text = c.Name() + " options";
            GB.Size = new Size(141, 220);

            GB.Controls.Add(new Label() { Text = "Height", Location = new Point(6, 16), Size = new Size(60, 15) });

            TrackBar TB = new TrackBar();
            TB.Maximum = 1000;
            TB.Minimum = 1;
            TB.Value = (int)(c.Height * 100);
            TB.Location = new Point(3, 37);
            TB.Size = new Size(135, 45);
            TB.ValueChanged += TrackBarChangeConeHeight;
            GB.Controls.Add(TB);

            GB.Controls.Add(new Label() { Text = "Radius", Location = new Point(6, 80), Size = new Size(70, 15) });

            TrackBar TB1 = new TrackBar();
            TB1.Maximum = 1000;
            TB1.Minimum = 1;
            TB1.Value = (int)(c.Radius * 100);
            TB1.Location = new Point(9, 95);
            TB1.Size = new Size(135, 45);
            TB1.ValueChanged += TrackBarChangeConeRadius;
            GB.Controls.Add(TB1);

            GB.Controls.Add(new Label() { Text = "Triangle step", Location = new Point(6, 144), Size = new Size(60, 15) });

            TrackBar TB2 = new TrackBar();
            TB2.Maximum = 60;
            TB2.Minimum = 1;
            TB2.Value = c.TriangleStep;
            TB2.Location = new Point(9, 159);
            TB2.Size = new Size(135, 45);
            TB2.ValueChanged += TrackBarChangeConeStep;
            GB.Controls.Add(TB2);

            flowLayoutPanel1.Controls.Add(GB);
        }
        private void TrackBarChangeConeHeight(object sender, EventArgs e)
        {
            ((Cone)SceneObjectList[listBox1.SelectedIndex]).Height = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeConeRadius(object sender, EventArgs e)
        {
            ((Cone)SceneObjectList[listBox1.SelectedIndex]).Radius = (double)((TrackBar)sender).Value / 100;
        }
        private void TrackBarChangeConeStep(object sender, EventArgs e)
        {
            ((Cone)SceneObjectList[listBox1.SelectedIndex]).TriangleStep = ((TrackBar)sender).Value;
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i < 1) return;
            listBox1.SelectedIndex = (listBox1.SelectedIndex + 1) % listBox1.Items.Count;
            if (i == listBox1.SelectedIndex) listBox1.SelectedIndex--;
            SceneObjectList.RemoveAt(i);
            listBox1.Items.RemoveAt(i);
        }

        private void ButtonAddNewItem_Click(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    AddNewCamera();
                    break;
                case 1:
                    AddNewLight();
                    break;
                case 2:
                    AddNewCuboid();
                    break;
                case 3:
                    AddNewSphere();
                    break;
                case 4:
                    AddNewCylinder();
                    break;
                case 5:
                    AddNewCone();
                    break;
            }
        }
        private void AddNewCamera()
        {
            Camera C = new Camera(
                new Vector4(0, 0, 3, 1),
                new Vector4(0, 0, 0, 1),
                null);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
        }
        private void AddNewLight()
        {
            Light C = new Light(new Vector4(0, 2, 0, 1), Color.White, 1, 1);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
        }
        private void AddNewCuboid()
        {
            Cuboid C = new Cuboid(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 0), 1, 1, 1.5, 1);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
        }
        private void AddNewSphere()
        {
            Sphere C = new Sphere(new Vector4(0, 0, 0, 1), 1, 15, 15);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
        }
        private void AddNewCylinder()
        {
            Cylinder C = new Cylinder(new Vector4(0, 0, 0, 1), 1, 0.5);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
        }
        private void AddNewCone()
        {
            Cone C = new Cone(new Vector4(0, 0, 0, 1), 1, 0.5);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
        }


        private void SaveSceneButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt file|*.txt";
            saveFileDialog1.Title = "Save scene";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                StreamWriter fw = new StreamWriter(fs);
                for (int i = 0; i < SceneObjectList.Count; i++)
                {
                    switch(SceneObjectList[i].ObjectType)
                    {
                        case SceneObject.ObjectTypeEnum.Camera:
                            SaveCamera((Camera)SceneObjectList[i], fw);
                            break;
                        case SceneObject.ObjectTypeEnum.Light:
                            SaveLight((Light)SceneObjectList[i], fw);
                            break;
                        case SceneObject.ObjectTypeEnum.Cuboid:
                            SaveCuboid((Cuboid)SceneObjectList[i], fw);
                            break;
                        case SceneObject.ObjectTypeEnum.Sphere:
                            SaveSphere((Sphere)SceneObjectList[i], fw);
                            break;
                        case SceneObject.ObjectTypeEnum.Cylinder:
                            SaveCylinder((Cylinder)SceneObjectList[i], fw);
                            break;
                        case SceneObject.ObjectTypeEnum.Cone:
                            SaveCone((Cone)SceneObjectList[i], fw);
                            break;
                    }
                }
                fw.Close();
                fs.Close();
            }
        }

        private void SaveCamera(Camera c, StreamWriter fw)
        {
            fw.WriteLine($"0;{c.Position.vector[0]};{c.Position.vector[1]};{c.Position.vector[2]};" +
                $"{c.Target.vector[0]};{c.Target.vector[1]};{c.Target.vector[2]};" +
                $"{c.UpWorld.vector[0]};{c.UpWorld.vector[1]};{c.UpWorld.vector[2]};" +
                $"{c.fov};{c.AspectRatio};{c.NearPlaneDistance};{c.FarPlaneDistance}");
        }
        private void SaveLight(Light c, StreamWriter fw)
        {
            fw.WriteLine($"1;{c.Position.vector[0]};{c.Position.vector[1]};{c.Position.vector[2]};" +
                $"{c.LightColor.R};{c.LightColor.G};{c.LightColor.B};" +
                $"{c.Intensity};{c.Attenuation}");
        }
        private void SaveCuboid(Cuboid c, StreamWriter fw)
        {
            fw.WriteLine($"2;{c.Position.vector[0]};{c.Position.vector[1]};{c.Position.vector[2]};" +
                $"{c.Rotation.vector[0]};{c.Rotation.vector[1]};{c.Rotation.vector[2]};" +
                $"{c.Scale};{c.Width};{c.Height};{c.Depth}");
        }
        private void SaveSphere(Sphere c, StreamWriter fw)
        {
            fw.WriteLine($"3;{c.Position.vector[0]};{c.Position.vector[1]};{c.Position.vector[2]};" +
                $"{c.Rotation.vector[0]};{c.Rotation.vector[1]};{c.Rotation.vector[2]};" +
                $"{c.Scale};{c.Radius};{c.TriangleStepHorizontal};{c.TriangleStepVertical}");
        }
        private void SaveCylinder(Cylinder c, StreamWriter fw)
        {
            fw.WriteLine($"4;{c.Position.vector[0]};{c.Position.vector[1]};{c.Position.vector[2]};" +
                $"{c.Rotation.vector[0]};{c.Rotation.vector[1]};{c.Rotation.vector[2]};" +
                $"{c.Scale};{c.Height};{c.Radius};{c.TriangleStep}");
        }
        private void SaveCone(Cone c, StreamWriter fw)
        {
            fw.WriteLine($"5;{c.Position.vector[0]};{c.Position.vector[1]};{c.Position.vector[2]};" +
                $"{c.Rotation.vector[0]};{c.Rotation.vector[1]};{c.Rotation.vector[2]};" +
                $"{c.Scale};{c.Height};{c.Radius};{c.TriangleStep}");
        }

        private void LoadSceneButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt file|*.txt";
            openFileDialog1.Title = "Save scene";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                for (int i = SceneObjectList.Count - 1; i > 1; i--)
                {
                    SceneObjectList.RemoveAt(i);
                    listBox1.Items.RemoveAt(i);
                }
                listBox1.SelectedIndex = 0;
                FileStream fs = (FileStream)openFileDialog1.OpenFile();
                StreamReader fw = new StreamReader(fs);
                string line = fw.ReadLine();
                LoadFirstCamera(line);
                line = fw.ReadLine();
                while (true)
                {
                    if (line == null) break;

                    switch (line[0])
                    {
                        case '0':
                            LoadCamera(line);
                            break;
                        case '1':
                            LoadLight(line);
                            break;
                        case '2':
                            LoadCuboid(line);
                            break;
                        case '3':
                            LoadSphere(line);
                            break;
                        case '4':
                            LoadCylinder(line);
                            break;
                        case '5':
                            LoadCone(line);
                            break;
                    }

                    line = fw.ReadLine();
                }

                fw.Close();
                fs.Close();
            }
        }
        private void LoadFirstCamera(string line)
        {
            string[] s = line.Split(';');
            Camera c = (Camera)SceneObjectList[0];
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Target.vector[0] = Convert.ToDouble(s[4]);
            c.Target.vector[1] = Convert.ToDouble(s[5]);
            c.Target.vector[2] = Convert.ToDouble(s[6]);
            c.UpWorld.vector[0] = Convert.ToDouble(s[7]);
            c.UpWorld.vector[1] = Convert.ToDouble(s[8]);
            c.UpWorld.vector[2] = Convert.ToDouble(s[9]);
            c.fov = Convert.ToDouble(s[10]);
            c.AspectRatio = Convert.ToDouble(s[11]);
            c.NearPlaneDistance = Convert.ToDouble(s[12]);
            c.FarPlaneDistance = Convert.ToDouble(s[13]);
        }
        private void LoadCamera(string line)
        {
            string[] s = line.Split(';');
            Camera c = new Camera(new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0), null);
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Target.vector[0] = Convert.ToDouble(s[4]);
            c.Target.vector[1] = Convert.ToDouble(s[5]);
            c.Target.vector[2] = Convert.ToDouble(s[6]);
            c.UpWorld.vector[0] = Convert.ToDouble(s[7]);
            c.UpWorld.vector[1] = Convert.ToDouble(s[8]);
            c.UpWorld.vector[2] = Convert.ToDouble(s[9]);
            c.fov = Convert.ToDouble(s[10]);
            c.AspectRatio = Convert.ToDouble(s[11]);
            c.NearPlaneDistance = Convert.ToDouble(s[12]);
            c.FarPlaneDistance = Convert.ToDouble(s[13]);
        }
        private void LoadLight(string line)
        {
            string[] s = line.Split(';');
            Light c = new Light(new Vector4(0, 0, 0, 0), Color.FromArgb(Convert.ToInt32(s[4]), Convert.ToInt32(s[5]), Convert.ToInt32(s[6])), 0, 0);
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Intensity = Convert.ToDouble(s[7]);
            c.Attenuation = Convert.ToDouble(s[8]);
            SceneObjectList.Add(c);
            listBox1.Items.Add(c);
        }
        private void LoadCuboid(string line)
        {
            string[] s = line.Split(';');
            Cuboid c = new Cuboid(new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0), Convert.ToDouble(s[7]), Convert.ToDouble(s[8]), Convert.ToDouble(s[9]), Convert.ToDouble(s[10]));
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Rotation.vector[0] = Convert.ToDouble(s[4]);
            c.Rotation.vector[1] = Convert.ToDouble(s[5]);
            c.Rotation.vector[2] = Convert.ToDouble(s[6]);
            SceneObjectList.Add(c);
            listBox1.Items.Add(c);
        }
        private void LoadSphere(string line)
        {
            string[] s = line.Split(';');
            Sphere c = new Sphere(new Vector4(0, 0, 0, 0), Convert.ToDouble(s[8]), Convert.ToInt32(s[9]), Convert.ToInt32(s[10]));
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Rotation.vector[0] = Convert.ToDouble(s[4]);
            c.Rotation.vector[1] = Convert.ToDouble(s[5]);
            c.Rotation.vector[2] = Convert.ToDouble(s[6]);
            c.Scale = Convert.ToDouble(s[7]);
            SceneObjectList.Add(c);
            listBox1.Items.Add(c);
        }
        private void LoadCylinder(string line)
        {
            string[] s = line.Split(';');
            Cylinder c = new Cylinder(new Vector4(0, 0, 0, 0), Convert.ToDouble(s[8]), Convert.ToDouble(s[9]), Convert.ToInt32(s[10]));
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Rotation.vector[0] = Convert.ToDouble(s[4]);
            c.Rotation.vector[1] = Convert.ToDouble(s[5]);
            c.Rotation.vector[2] = Convert.ToDouble(s[6]);
            c.Scale = Convert.ToDouble(s[7]);
            SceneObjectList.Add(c);
            listBox1.Items.Add(c);
        }
        private void LoadCone(string line)
        {
            string[] s = line.Split(';');
            Cone c = new Cone(new Vector4(0, 0, 0, 0), Convert.ToDouble(s[8]), Convert.ToDouble(s[9]), Convert.ToInt32(s[10]));
            c.Position.vector[0] = Convert.ToDouble(s[1]);
            c.Position.vector[1] = Convert.ToDouble(s[2]);
            c.Position.vector[2] = Convert.ToDouble(s[3]);
            c.Rotation.vector[0] = Convert.ToDouble(s[4]);
            c.Rotation.vector[1] = Convert.ToDouble(s[5]);
            c.Rotation.vector[2] = Convert.ToDouble(s[6]);
            c.Scale = Convert.ToDouble(s[7]);
            SceneObjectList.Add(c);
            listBox1.Items.Add(c);
        }
    }
}
