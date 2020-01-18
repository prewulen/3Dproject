using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, DrawArea, new object[] { true });
            map = new Bitmap(DrawArea.Width, DrawArea.Height);
            Camera C = new Camera(new Vector4(2, 0.5, 2, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C);
            listBox1.Items.Add(C);
            Camera C1 = new Camera(new Vector4(0, -3, 1, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C1);
            listBox1.Items.Add(C1);
            Camera C2 = new Camera(new Vector4(0, 0, 3, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C2);
            listBox1.Items.Add(C2);
            Camera C3 = new Camera(new Vector4(3, 0, 0, 1), new Vector4(0, 0, 0, 1), null);
            SceneObjectList.Add(C3);
            listBox1.Items.Add(C3);
            Light L1 = new Light(new Vector4(0, 2, 0, 1), Color.White, 1, 1);
            SceneObjectList.Add(L1);
            listBox1.Items.Add(L1);
            SelectedCamera = C;
            //Cuboid Cb = new Cuboid(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 0), 1, 1, 1.5, 1);
            //SceneObjectList.Add(Cb);
            Cuboid Cb1 = new Cuboid(new Vector4(2, 0, 0, 1), new Vector4(0, 0, 0, 0), 1, 1, 1.5, 1);
            SceneObjectList.Add(Cb1);
            listBox1.Items.Add(Cb1);
            Sphere s = new Sphere(new Vector4(0, 0, 0, 1), 1, 15, 15);
            SceneObjectList.Add(s);
            listBox1.Items.Add(s);

            comboBox1.Items.Add("Camera");
            comboBox1.Items.Add("Light");
            comboBox1.Items.Add("Cuboid");
            comboBox1.Items.Add("Sphere");
            comboBox1.Items.Add("Cylinder");
            comboBox1.Items.Add("Cone");
            comboBox1.SelectedIndex = 0;

            //TestDraw();
            //DrawArea.Refresh();

        }


        private void TestDraw()
        {
            Polygon p = new Polygon() { Completed = true };
            p.Add(new Point(50, 50));
            p.Add(new Point(150, 150));
            p.Add(new Point(50, 150));
            FillPoly(p, Color.Red);
        }

        //private void ColorPicker_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = colorDialog1.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        ColorPicker.BackColor = colorDialog1.Color;
        //        LightColor = colorDialog1.Color;
        //    }
        //    DrawArea.Refresh();
        //}

     

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

        void FillPoly(Polygon p, Color color)
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
                    //if (InterpolationCheckBox.Checked)
                    //{
                        Color Left = InterpolateColor(p.points[AET[i].p1], p.points[AET[i].p2], xa1, ystart + 1, VerticeColor[AET[i].p1], VerticeColor[AET[i].p2]);
                        Color Right = InterpolateColor(p.points[AET[i + 1].p1], p.points[AET[i + 1].p2], xa2, ystart + 1, VerticeColor[AET[i + 1].p1], VerticeColor[AET[i + 1].p2]);
                        DrawPixel(xa1, ystart, Left);
                        DrawPixel(xa2, ystart, Right);
                        for (int j = xa1 + 1; j < xa2; j++)
                        {
                            DrawPixel(j, ystart, InterpolateColorL(xa1, xa2, j, Left, Right));
                        }
                    //}
                    //else
                    //{
                    //    for (int j = xa1; j <= xa2; j++)
                    //    {
                    //        DrawPixel(j, ystart, color);
                    //    }
                    //}
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
                if (so.ObjectType == SceneObject.ObjectTypeEnum.Camera || so.ObjectType == SceneObject.ObjectTypeEnum.Light) continue;
                Matrix4x4 M = Matrix4x4.MultiplyM(M1, Matrix4x4.GetTranslationMatrix(so.Position.vector[0], so.Position.vector[1], so.Position.vector[2]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationXMatrix(so.Rotation.vector[0]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationYMatrix(so.Rotation.vector[1]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationZMatrix(so.Rotation.vector[2]));
                M = Matrix4x4.MultiplyM(M, Matrix4x4.GetScaleMatrix(so.Scale, so.Scale, so.Scale));

                foreach(Triangle t in so.Triangles())
                {
                    t.Multiply(M);
                    //todo: Obcinanie

                    if (LineFillCheckbox.Checked)
                    {
                        DrawTriangleLines(t.x1, t.x2, t.x3, Color.Red);
                    }
                    else
                    {
                        DrawTriangle(t.x1, t.x2, t.x3, Color.Red);
                    }
                }
            }

            //P x V x M x p
            DrawArea.Refresh();
        }

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

            FillPoly(p, c);
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

            FillPoly(p, c);
        }
        private void DrawTriangle(Triangle t)
        {
            
            t.x1 = new Vector4(t.x1.vector[0] / t.x1.vector[3], t.x1.vector[1] / t.x1.vector[3], t.x1.vector[2] / t.x1.vector[3], 1);
            t.x2 = new Vector4(t.x2.vector[0] / t.x2.vector[3], t.x2.vector[1] / t.x2.vector[3], t.x2.vector[2] / t.x2.vector[3], 1);
            t.x3 = new Vector4(t.x3.vector[0] / t.x3.vector[3], t.x3.vector[1] / t.x3.vector[3], t.x3.vector[2] / t.x3.vector[3], 1);

            if (BackFaceCull(t.x1, t.x2, t.x3))
                return;

            Polygon p = new Polygon();
            p.Completed = true;
            p.points.Add(new Point((int)(((n1.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n1.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n2.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n2.vector[1] / 2) + 0.5) * DrawArea.Height)));
            p.points.Add(new Point((int)(((n3.vector[0] / 2) + 0.5) * DrawArea.Width), (int)(((-n3.vector[1] / 2) + 0.5) * DrawArea.Height)));


            p.Fix();

            FillPoly(p, c);
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
                                SceneObjectList[listBox1.SelectedIndex].Position.vector[1] += ((e.Y - LastMouseY) / 500);
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
                                ((-(e.X - LastMouseX) * MoveVectorX.vector[0]) / 100) + (((e.Y - LastMouseY) * MoveVectorY.vector[0]) / 100),
                                ((-(e.X - LastMouseX) * MoveVectorX.vector[1]) / 100) + (((e.Y - LastMouseY) * MoveVectorY.vector[1]) / 100),
                                ((-(e.X - LastMouseX) * MoveVectorX.vector[2]) / 100) + (((e.Y - LastMouseY) * MoveVectorY.vector[2]) / 100));
                            SelectedCamera.Position = Matrix4x4.MultiplyV(m, SelectedCamera.Position);
                            SelectedCamera.Target = Matrix4x4.MultiplyV(m, SelectedCamera.Target);
                        }
                        if (e.Button == MouseButtons.Left)
                        {
                            Vector4 P = SelectedCamera.Position;
                            Vector4 T = SelectedCamera.Target - P;
                            Matrix4x4 M = Matrix4x4.GetRotationAxisMatrix(((e.Y - LastMouseY) / 500), MoveVectorX);
                            M = Matrix4x4.MultiplyM(M, Matrix4x4.GetRotationAxisMatrix(((e.X - LastMouseX) / 500), MoveVectorY));
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
            TB.Maximum = 300;
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

            GB.Controls.Add(new Label() { Text = "Range", Location = new Point(6, 80), Size = new Size(70, 15) });

            TrackBar TB1 = new TrackBar();
            TB1.Maximum = 200;
            TB1.Minimum = 1;
            TB1.Value = (int)(l.Range * 100);
            TB1.Location = new Point(9, 95);
            TB1.Size = new Size(135, 45);
            TB1.ValueChanged += TrackBarChangeFov;
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
            ((Light)SceneObjectList[listBox1.SelectedIndex]).Range = (double)((TrackBar)sender).Value / 100;
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
            TB2.Maximum = 60;
            TB2.Minimum = 1;
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
            if (i < 0) return;
            if ((SceneObjectList[i] is Camera) && ((SceneObjectList.OfType<Camera>().Count()) == 1)) return;
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

        
    }
}
