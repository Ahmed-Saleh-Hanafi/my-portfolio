using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Project_Game_CG
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public void TranslatePoint(double TX, double TY)
        {
            this.X = (int)(this.X + TX);
            this.Y = (int)(this.Y + TY);
        }
        public void ScalingPoint(double SX, double SY, Point Center)
        {
            int TranslatedX = this.X - Center.X;
            int TranslatedY = this.Y - Center.Y;

            int ScaledX = (int)(TranslatedX * SX);
            int ScaledY = (int)(TranslatedY * SY);

            this.X = ScaledX + Center.X;
            this.Y = ScaledY + Center.Y;

        }
        public void RotateClockwise(double angle,Point center)
        {
            double radian = angle * (double)Math.PI/180;

            double TranslatedX = this.X - center.X;
            double TranslatedY = this.Y - center.Y;
            
            double RotatedX = TranslatedX * Math.Cos(radian)- (TranslatedY * Math.Sin(radian));
            double RotatedY = TranslatedX * Math.Sin(radian) + (TranslatedY * Math.Cos(radian));

            this.X = (int)(RotatedX + center.X) ;
            this.Y = (int)(RotatedY + center.Y) ;
        }
    }
    class Utilities
    {
        
        static public void DrawLine(Point p1, Point p2, Bitmap bitmap, Color color)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            int steps = (int)Math.Max(Math.Abs(dx), Math.Abs(dy));
            double Xinc = dx / steps;
            double Yinc = dy / steps;
            double Xpoint = p1.X;
            double Ypoint = p1.Y;
            for (int i = 0; i < steps; i++)
            {
                int X = (int)Math.Round(Xpoint);
                int Y = (int)Math.Round(Ypoint);
                if (X < 1000 && X > -1 && Y < 1000 && Y > -1) bitmap.SetPixel(X, Y, color);
                Xpoint = (Xpoint + Xinc);
                Ypoint = (Ypoint + Yinc);
            }
        }
        static public void DrawCircle(int radius, Point p1, Bitmap bitmap, Color color)
        {
            int x_Point = radius, y_Point = 0;
            bitmap.SetPixel(x_Point + p1.X, y_Point + p1.Y, color);
            int p = 1 - radius;
            while (x_Point > y_Point)
            {
                y_Point++;
                if (p <= 0)
                {
                    p = p + (2 * y_Point) + 1;
                }
                else
                {
                    x_Point--;
                    p = p + 2 * (y_Point - x_Point) + 1;

                }
                bitmap.SetPixel(x_Point + p1.X, y_Point + p1.Y, color);
                bitmap.SetPixel(y_Point + p1.X, x_Point + p1.Y, color);
                bitmap.SetPixel(x_Point + p1.X, -y_Point + p1.Y, color);
                bitmap.SetPixel(-x_Point + p1.X, -y_Point + p1.Y, color);
                bitmap.SetPixel(-x_Point + p1.X, y_Point + p1.Y, color);
                bitmap.SetPixel(y_Point + p1.X, -x_Point + p1.Y, color);
                bitmap.SetPixel(-y_Point + p1.X, -x_Point + p1.Y, color);
                bitmap.SetPixel(-y_Point + p1.X, x_Point + p1.Y, color);

            }
        }
        static public void Coloring(Face face,  Bitmap bitmap)
        {
            Graphics gr = Graphics.FromImage(bitmap);
            SolidBrush brush = new SolidBrush(face.color);

            PointF[] points = new PointF[face.Points.Length];
            int i = 0;
            foreach (Point point in face.Points)
            {
                points[i] =new PointF (point.X, point.Y);
                i++;
            }
            gr.FillPolygon(brush, points);

        }
        static public void ColoringBorder(Face face, Bitmap bitmap)
        {
            Graphics gr = Graphics.FromImage(bitmap);
            
            Pen pen = new Pen(face.BorderColor, face.size_Border);
            PointF[] points = new PointF[face.Points.Length];
            int i = 0;
            foreach (Point point in face.Points)
            {
                points[i] = new PointF(point.X, point.Y);
                i++;
            }
            gr.DrawPolygon(pen, points);
        }
        static public bool IsInsideFace(Point point,Face face)
        {
            Point topl = face.Points[0];
            Point bottomr = face.Points[2];
            return (point.X >= topl.X && point.X <= bottomr.X && point.Y >= topl.Y && point.Y <= bottomr.Y);
        }
    }
    class Edge
    {
        public Point Strart { get; set; }
        public Point End { get; set; }
        public Edge(Point start, Point end)
        {
            this.Strart = start;
            this.End = end;
        }
    }
    class Face
    {
        public Point[] Points { get; set; }
        public Edge[] Edges { get; set; }
        public Color color { get; set; }
        public Color BorderColor { get; set; }
        public int size_Border;
        public Face(Point[] points, Edge[] edges, Color color, Color BorderColor, int size_Border)
        {
            this.Points = points;
            this.Edges = edges;
            this.color = color;
            this.BorderColor = BorderColor;
            this.size_Border = size_Border;
        }
        public void DrawFace (Bitmap bitmap)
        {
            foreach (Edge edge in Edges)
            {
              Utilities.DrawLine(edge.Strart,edge.End,bitmap,BorderColor);
            }
            
        }

       
    }
    class Shape
    {
        public Face[] Faces { get; set; }
        public int[] Priority { get; set; }
        public Shape(Face[] faces, int[] Priority)
        {
            
            this.Faces = faces;
            this.Priority = Priority;
                
        }
        public void DrawShape(Bitmap bitmap)
        {
            foreach (int prio in Priority)
            {
                Faces[prio].DrawFace(bitmap);
                Utilities.Coloring(Faces[prio], bitmap);
                Utilities.ColoringBorder(Faces[prio], bitmap);
            }
        }
        public Point GetCentetr()
        {
            double cx = 0, cy = 0, npoint = 0;
            foreach (Face face in Faces)
            {
                foreach (Point point in face.Points)
                {
                    cx += point.X;
                    cy += point.Y;
                    npoint++;
                }
            }
            return new Point((int)(cx / npoint), (int)(cy / npoint));

        }

    }
    class Player
    {
       public Shape player;
       public Player(Shape shape)
        {
            this.player = shape;
        }
        public void DrawPlayer (Bitmap bitmap)
        {
            player.DrawShape(bitmap);
        }

        public void TranslatePlayer (double Tx , double Ty)
        {
           
            if (player.Faces[0].Points[0].Y <= 22  && Ty < 0)
                return;
            if (player.Faces[1].Points[3].Y >= 518 && Ty > 0)
                return;
            if (player.Faces[1].Points[0]. X<= 57 && Tx < 0)
                return;
            if (player.Faces[1].Points[1].X >= 945 && Tx > 0)
                return;
            if (player.Faces[0].Points[0].X <= 600 && player.Faces[0].Points[0].X > 400 && Tx < 0)
                return;
            if (player.Faces[0].Points[1].X >= 400 && player.Faces[0].Points[1].X < 600 && Tx > 0)
                return;
            foreach (Face face in player.Faces)
            {
                foreach (Point point in face.Points)
                {
                    point.TranslatePoint(Tx, Ty);
                }
            }
        }
        public void ScalePlayer(double Sx, double Sy)
        {
            foreach (Face face in player.Faces)
            {
                foreach (Point point in face.Points)
                {
                    point.ScalingPoint(Sx, Sy,player.GetCentetr());
                }
            }
        }

    }
    class Game
    {
        public Player player1;
        public Player player2;
        public Ball ball;
        public int ngoalp1, ngoalp2;
        public void StartGame (Bitmap bitmap)
        {
            DrawPlayGround(bitmap);
            DrawOutLines(bitmap);
            DrawOriginCircle(bitmap);
            initalPlayers();
            DisplayPlayers(bitmap);
            InitialBall();
            DisplayBall(bitmap);
            ngoalp1 =0;
            ngoalp2 =0;


        }
        public void DisplayGame(Bitmap bitmap)
        {
            DrawPlayGround(bitmap);
            DrawOutLines(bitmap);
            DrawOriginCircle(bitmap);
            DisplayPlayers(bitmap);
            DisplayBall(bitmap);
        }
        public void DrawPlayGround (Bitmap bitmap)
        {
            Point[] points = new Point[4]{
                new Point (0,0),
                new Point (1000,0),
                new Point (1000,540),
                new Point (0,540)
            };
            Edge[] edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            Face []face = new Face[1] {new Face (points, edges, Color.Green, Color.Green, 1) };
            Shape sh = new Shape(face, new int [] { 0 });
            sh.DrawShape(bitmap);

            points = new Point[]{
                new Point (200,0),
                new Point (400,0),
                new Point (400,540),
                new Point (200,540)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            face = new Face[1] { new Face(points, edges, Color.DarkGreen, Color.DarkGreen, 1) };
            sh = new Shape(face, new int[] { 0 });
            sh.DrawShape(bitmap);

            points = new Point[]{
                new Point (600,0),
                new Point (800,0),
                new Point (800,540),
                new Point (600,540)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            face = new Face[1] { new Face(points, edges, Color.DarkGreen, Color.DarkGreen, 1) };
            sh = new Shape(face, new int[] { 0 });
            sh.DrawShape(bitmap);
        }
        public void DrawOutLine(Point p1,Point p2,Bitmap bitmap)
        {
            Edge[] edges = new Edge[]{
                new Edge (p1,p2)
            };
            Face[] face = new Face[1] { new Face(new Point[] {p1,p2}, edges, Color.White, Color.White, 2) };
            Shape sh = new Shape(face, new int[] { 0 });
            sh.DrawShape(bitmap);
        }
        public void DrawOutLines (Bitmap bitmap)
        {
            DrawOutLine(new Point(50, 15), new Point(950, 15), bitmap);
            DrawOutLine(new Point(50, 525),new Point(950, 525), bitmap);
            DrawOutLine(new Point(950, 15),new Point(950, 170), bitmap);
            DrawOutLine(new Point(950, 370),new Point(950, 525), bitmap);
            DrawOutLine(new Point(50, 15), new Point(50, 170), bitmap);
            DrawOutLine(new Point(50, 370), new Point(50, 525), bitmap);
            DrawOutLine(new Point(500, 15), new Point(500, 525), bitmap);
        }
        public void DrawOriginCircle(Bitmap bitmap)
        {
            Utilities.DrawCircle(80, new Point(500, 270), bitmap, Color.White);
            Utilities.DrawCircle(2, new Point(500, 270), bitmap, Color.White);
        }
        public void initalPlayers()
        {
            Point[] points = new Point[4]{
                new Point (910,220),
                new Point (930,220),
                new Point (930,320),
                new Point (910,320)
            };
            Edge[] edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            Face face1 = new Face (points, edges, Color.Gray, Color.Black, 0);
            points = new Point[4]{
                new Point (920,230),
                new Point (940,230),
                new Point (940,330),
                new Point (920,330)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            Face face2 = new Face(points, edges, Color.Gray, Color.Black, 0);
            points = new Point[4]{
                new Point (910,220),
                new Point (920,230),
                new Point (920,330),
                new Point (910,320)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            Face face3 = new Face(points, edges, Color.DimGray, Color.Black, 0);
            points = new Point[4]{
                new Point (910,220),
                new Point (930,220),
                new Point (940,230),
                new Point (920,230)
                
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            Face face4 = new Face(points, edges, Color.Gray, Color.Black, 0);

            Face []faces = new Face[] { face1, face2,face3 ,face4};
            Shape sh = new Shape(faces, new int[] { 0 ,1,2,3});
            player2 = new Player(sh);

            ////////////////////////////////////////////////////////
            points = new Point[4]{ // p1
                new Point (70,220),
                new Point (90,220),
                new Point (90,320),
                new Point (70,320)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            face1 = new Face(points, edges, Color.Gray, Color.Black, 0);
            points = new Point[4]{ //p2
                new Point (60,230),
                new Point (80,230),
                new Point (80,330),
                new Point (60,330)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            face2 = new Face(points, edges, Color.Gray, Color.Black, 0);
            points = new Point[4]{//f3
                new Point (80,230),
                new Point (90,220),
                new Point (90,320),
                new Point (80,330)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            face3 = new Face(points, edges, Color.DimGray, Color.Black, 0);
            points = new Point[4]{//f4
                new Point (70,220),
                new Point (60,230),
                new Point (80,230),
                new Point (90,220)
            };
            edges = new Edge[4]{
                new Edge (points[0],points[1]),
                new Edge (points[1],points[2]),
                new Edge (points[2],points[3]),
                new Edge (points[3],points[0])
            };
            face4 = new Face(points, edges, Color.Gray, Color.Black, 0);

            faces = new Face[] { face1, face2, face3, face4 };
            sh = new Shape(faces, new int[] { 0, 1, 2, 3 });
            player1 = new Player(sh);
        }
        public void DisplayPlayers (Bitmap bitmap)
        {
           
            player2.DrawPlayer(bitmap);
            player1.DrawPlayer(bitmap);
        }
        public void InitialBall()
        {
            ball = new Ball (new Point(500,270),13,Color.Black , Color.Black);
        }
        public void DisplayBall(Bitmap bitmap)
        {
            ball.DrawBall(bitmap);
        }
    }
 
    class Ball
    {
       public Point orgin { get; set; }
       public int radius { get; set; }
       public Color color1 { get; set; }
       public Color outline_color { get; set; }
       public Point pt, pd,pl,pr;

        public Ball(Point orgin, int radius, Color color1, Color outline_color)
        {
            this.orgin = orgin;
            this.radius = radius;
            this.color1 = color1;
            pt= new Point (orgin.X, orgin.Y- radius);
            pd = new Point(orgin.X, orgin.Y + radius);
            pl = new Point(orgin.X- radius, orgin.Y);
            pr = new Point(orgin.X+ radius, orgin.Y);
            this.outline_color = outline_color;
        }

       public void DrawBall (Bitmap bitmap)
        {
            Utilities.DrawCircle(radius, orgin, bitmap, outline_color);
            for (int y = -radius ; y <= radius ; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {

                    if((x * x) + (y*y) <= (radius*radius))
                    {
                        int pixelx = orgin.X + x;
                        int pixely = orgin.Y + y;
                        bitmap.SetPixel(pixelx, pixely,color1);
                    }

                }
                //Utilities.DrawLine(p1, p2, bitmap, Color.Red);
               
            }
            

        }
        public void TranslateBall (double Tx, double Ty)
        {
            
            orgin.TranslatePoint(Tx, Ty);
            pt.TranslatePoint(Tx, Ty);
            pd .TranslatePoint(Tx, Ty);
            pl .TranslatePoint(Tx, Ty);
            pr .TranslatePoint(Tx, Ty);
            
        }
        public void ResetBall (Point orgin)
        {
            this.orgin = orgin;
            
            pt = new Point(orgin.X, orgin.Y - radius);
            pd = new Point(orgin.X, orgin.Y + radius);
            pl = new Point(orgin.X - radius, orgin.Y);
            pr = new Point(orgin.X + radius, orgin.Y);
            
        }






    }

    
        internal static class Program
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    
}
