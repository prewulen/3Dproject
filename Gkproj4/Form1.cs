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

namespace Gkproj4
{
    public partial class Form1 : Form
    {
        public enum Mode { AddPolygon, Move, Delete, Light }
        List<Polygon> polygons = new List<Polygon>();
        Mode CurrentMode;
        Color LightColor = Color.White;
        Point LightP = new Point(10, 10);
        Bitmap map;
        int time = 0;
        double LightZ = 100D;
        Color DrawColor = Color.BlueViolet;

        DateTime _lastCheckTime = DateTime.Now;
        long _frameCount = 0;
        public Form1()
        {
            InitializeComponent();

            CurrentMode = Mode.Light;
            ColorPicker.BackColor = Color.White;
            this.DoubleBuffered = true;
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, DrawArea, new object[] { true });
            map = new Bitmap(DrawArea.Width, DrawArea.Height);
            Polygon p = new Polygon();
            p.Completed = true;
            p.points.Add(new Point(150, 150));
            p.points.Add(new Point(200, 150));
            p.points.Add(new Point(250, 200));
            p.points.Add(new Point(250, 250));
            p.points.Add(new Point(200, 300));
            p.points.Add(new Point(150, 300));
            p.points.Add(new Point(100, 250));
            p.points.Add(new Point(100, 200));
            polygons.Add(p);
        }



        private void ButtonLightPos_Click(object sender, EventArgs e)
        {
            CurrentMode = Mode.Light;
        }

        private void ColorPicker_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ColorPicker.BackColor = colorDialog1.Color;
                LightColor = colorDialog1.Color;
            }
            DrawArea.Refresh();
        }

        private void DrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            switch (CurrentMode)
            {
                case Mode.Light:
                    LightP = e.Location;
                    break;
            }
            DrawArea.Refresh();
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
                    if (InterpolationCheckBox.Checked)
                    {
                        Color Left = InterpolateColor(p.points[AET[i].p1], p.points[AET[i].p2], xa1, ystart + 1, VerticeColor[AET[i].p1], VerticeColor[AET[i].p2]);
                        Color Right = InterpolateColor(p.points[AET[i + 1].p1], p.points[AET[i + 1].p2], xa2, ystart + 1, VerticeColor[AET[i + 1].p1], VerticeColor[AET[i + 1].p2]);
                        DrawPixel(xa1, ystart, Left);
                        DrawPixel(xa2, ystart, Right);
                        for (int j = xa1 + 1; j < xa2; j++)
                        {
                            DrawPixel(j, ystart, InterpolateColorL(xa1, xa2, j, Left, Right));
                        }
                    }
                    else
                    {
                        for (int j = xa1; j <= xa2; j++)
                        {
                            DrawPixel(j, ystart, color);
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

        Color InterpolateColor(Point p1, Point p2, int x, int y, Color Left, Color Right)
        {
            double d1 = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            double d2 = (x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y);
            double ratio;
            if (d2 < 1e-5) ratio = 1;
            else ratio = d2 / d1;
            double ratio2 = 1D - ratio;
            return Color.FromArgb((int)(Left.R * ratio2 + Right.R * ratio) % 255, (int)(Left.G * ratio2 + Right.G * ratio) % 255, (int)(Left.B * ratio2 + Right.B * ratio) % 255);
        }

        Color InterpolateColorL(int x1, int x2, int x, Color Left, Color Right)
        {
            double ratio = (double)Math.Abs(x - x1) / Math.Abs(x1 - x2);
            double ratio2 = 1D - ratio;
            return Color.FromArgb((int)(Left.R * ratio2 + Right.R * ratio) % 255, (int)(Left.G * ratio2 + Right.G * ratio) % 255, (int)(Left.B * ratio2 + Right.B * ratio) % 255);
        }

        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            //map = new Bitmap(DrawArea.Width, DrawArea.Height);

            //foreach (Polygon p in polygons)
            //{
            //    FillPoly(p);
            //    for (int i = 0; i < p.points.Count; i++)
            //        drawVertice(p.points[i], Color.Black);

            //    if (p.Completed)
            //    {
            //        for (int i = 0; i < p.points.Count; i++)
            //        {
            //            line(p.points[i].X, p.points[i].Y, p.points[(i + 1) % p.points.Count].X, p.points[(i + 1) % p.points.Count].Y, Color.Black);
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < p.points.Count - 1; i++)
            //        {
            //            line(p.points[i].X, p.points[i].Y, p.points[(i + 1) % p.points.Count].X, p.points[(i + 1) % p.points.Count].Y, Color.Black);
            //        }
            //    }
            //}
            drawVertice(LightP, Color.Red);



            e.Graphics.DrawImage(map, 0, 0);
            _frameCount++;
        }

        Color GetColor(int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            Color TC = DrawColor;
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
            double ToLightX = LightP.X - x;
            double ToLightY = LightP.Y - y;
            double ToLightZ = LightZ;
            double ToLightLength = Math.Sqrt(ToLightX * ToLightX + ToLightY * ToLightY + ToLightZ * ToLightZ);
            ToLightX /= ToLightLength;
            ToLightY /= ToLightLength;
            ToLightZ /= ToLightLength;
            double cos = DX * ToLightX + DY * ToLightY + DZ * ToLightZ;


            r = r * (LightColor.R / 255D) * cos;
            g = g * (LightColor.G / 255D) * cos;
            b = b * (LightColor.B / 255D) * cos;
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            return Color.FromArgb((int)(r * 255D) % 255, (int)(g * 255D) % 255, (int)(b * 255D) % 255);
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
            Vector4 v1 = new Vector4(-1,0,-1,1);
            Vector4 v2 = new Vector4(-1,0,1,1);
            Vector4 v3 = new Vector4(1,0,1,1);
            Vector4 v4 = new Vector4(1,0,-1,1);

            Matrix4x4 V = Matrix4x4.GetTranslationMatrix(0,-1,-5);
            V = Matrix4x4.MultiplyM(V, Matrix4x4.GetRotationYMatrix(((double)time / 10)));
            Matrix4x4 P = Matrix4x4.GetProjectionMatrix(DrawArea.Height, DrawArea.Width);
            Matrix4x4 m1 = Matrix4x4.MultiplyM(P, V);

            DrawRectangle(v1, v2, v3, v4, Color.FromArgb(0, 255, 0), m1);


            Matrix4x4 m2 = Matrix4x4.MultiplyM(m1, Matrix4x4.GetTranslationMatrix(0, 2, 0));

            DrawRectangle(v1, v2, v3, v4, Color.FromArgb(0, 255, 0), m2);


            Matrix4x4 m3 = Matrix4x4.MultiplyM(m1, Matrix4x4.GetTranslationMatrix(1,1,0));
            m3 = Matrix4x4.MultiplyM(m3, Matrix4x4.GetRotationZMatrix(Math.PI / 2));

            DrawRectangle(v1, v2, v3, v4, Color.FromArgb(255,0,0), m3);


            Matrix4x4 m4 = Matrix4x4.MultiplyM(m1, Matrix4x4.GetTranslationMatrix(-1, 1, 0));
            m4 = Matrix4x4.MultiplyM(m4, Matrix4x4.GetRotationZMatrix(Math.PI / 2));

            DrawRectangle(v1, v2, v3, v4, Color.FromArgb(255, 0, 0), m4);


            Matrix4x4 m5 = Matrix4x4.MultiplyM(m1, Matrix4x4.GetTranslationMatrix(0,1,-1));
            m5 = Matrix4x4.MultiplyM(m5, Matrix4x4.GetRotationXMatrix(Math.PI / 2));

            DrawRectangle(v1, v2, v3, v4, Color.FromArgb(0,0,255), m5);


            Matrix4x4 m6 = Matrix4x4.MultiplyM(m1, Matrix4x4.GetTranslationMatrix(0,1,0.5));
            m6 = Matrix4x4.MultiplyM(m6, Matrix4x4.GetRotationYMatrix(0.4));
            m6 = Matrix4x4.MultiplyM(m6, Matrix4x4.GetRotationXMatrix(Math.PI/2));
            m6 = Matrix4x4.MultiplyM(m6, Matrix4x4.GetScaleMatrix(0.5,0.5,0.25));
            m6 = Matrix4x4.MultiplyM(m6, Matrix4x4.GetRotationYMatrix(Math.PI / 4));
            
            DrawRectangle(v1, v2, v3, v4, Color.FromArgb(255,255,0), m6);

            DrawArea.Refresh();
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
    }
}
