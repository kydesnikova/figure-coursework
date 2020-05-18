using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Курсовой_Проект
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Figura();
            textBox1.Text = Convert.ToString(pictureBox1.Width);
            textBox2.Text = Convert.ToString(pictureBox1.Height);
            Width = pictureBox1.Width;
            Height = pictureBox1.Height;
        }
        int Width;
        int Height;
        double dt = 0;  //у границ рабочей области  
        private float diametr = 40;
        private void Figura()
        {//центр
            float x0 = 0 + dx;
            float y0 = pictureBox1.Height - diametr - dy;
          
            {
               Bitmap Ramka = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics graph = Graphics.FromImage(Ramka);
                byte blz = (byte)(255 - 195 / (pictureBox1.Width - diametr) * dx);//изменение цвета относительно x
                byte rdz = (byte)(60 + 195 / (pictureBox1.Width - diametr) * dx);
                Color col =  Color.FromArgb(255, blz, 255, rdz);
                Color col1 =  Color.FromArgb(255, rdz, 0, blz);
                Pen pen;
                if (diametr > 60) { pen = new Pen(col, 2); } else { pen = new Pen(col, 1); }
                SolidBrush brSolid = new SolidBrush(col);
                SolidBrush brSoft = new SolidBrush(col1);
                graph.DrawRectangle(pen, 1, 1, pictureBox1.Width - 2, pictureBox1.Height - 2);//ramka
                graph.FillEllipse(brSolid, x0, y0, diametr, diametr);//large circle
                                                                     //построение звезды
                float xz0 = x0 + diametr / 2;//центр фигруы
                float yz0 = y0 + diametr / 2;
                float opisR = (float)((diametr / 2) / (Math.Sqrt(5) - 1));//вписанный радиус (для нижних точек)
                double starSide = (diametr / 2 * Math.Sqrt((5 - Math.Sqrt(5)) / 2));//длина стороны , для нижних точек
                PointF p1 = new PointF(xz0, yz0 - diametr / 2);//первая часть звезды
                PointF p2 = new PointF((float)(xz0 + starSide / 4), yz0 - opisR / 2);
                PointF p3 = new PointF((float)(xz0 - starSide / 4), yz0 - opisR / 2);
                p1 = RotatePoint(p1, 90 * yg, xz0, yz0);//смещение треугольника 
                p2 = RotatePoint(p2, 90 * yg, xz0, yz0);
                p3 = RotatePoint(p3, 90 * yg, xz0, yz0);
                for (int n = 0; n < 11; n++)
                {//остальные части относительно первой
                    PointF pNew1 = RotatePoint(p1, 60 * n, xz0, yz0);
                    PointF pNew2 = RotatePoint(p2, 60 * n, xz0, yz0);
                    PointF pNew3 = RotatePoint(p3, 60 * n, xz0, yz0);
                    PointF[] treeDots = { pNew1, pNew2, pNew3 };
                    graph.FillPolygon(brSoft, treeDots);
                }
             
                pictureBox1.Image = Ramka;
                
            }

        }
        private PointF RotatePoint(PointF point, double n, float x0, float y0)
        {//функция для смещения относительно точки на угол
            float ddX = point.X - x0;
            float ddY = point.Y - y0;
            point.X = (float)(ddX * Math.Cos(Math.PI * n / 180) - ddY * Math.Sin(Math.PI * n / 180)) + x0;
            point.Y = (float)(ddX * Math.Sin(Math.PI * n / 180) + ddY * Math.Cos(Math.PI * n / 180)) + y0;
            return point;
        }

        float dx = 1;//смещение по координатам
        float dy = 1;
        float smesh = 3.1f;//скорость смещение по координтам
        float proverka = 1;//чтобы проверка на выход за предел работала
        double yg = 0;//угловое смещение
        double ygSpeed = 0.01f;
        
    
        //для движения по параболе
        private double FomA;
        private double FomB;
        private double FomC;
        private void CalcFom()
        {
            double x1 = 0; double x2 = (pictureBox1.Width) / 2 - diametr / 2;
            double y1 = (pictureBox1.Height - diametr); double y2 = 2;
            FomA = Math.Sqrt((y1 - y2) / Math.Pow(x1 / x2 - 1, 2));
            FomB = pictureBox1.Height - diametr - y2;
            FomC = FomA / x2;
        }
        private float move(float x)
        {
            CalcFom();
            return (float)(-Math.Pow(FomC * x - FomA, 2) + FomB);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                button1.Text = "Старт";
            }
            else
            {
                button1.Text = "Стоп";
                timer1.Start();
            }
            if (Convert.ToInt32(textBox1.Text) < Width && Convert.ToInt32(textBox1.Text) > 1)
            {
                pictureBox1.Width = Convert.ToInt32(textBox1.Text);
            }
            else { pictureBox1.Width = Width; }
            if (Convert.ToInt32(textBox2.Text) < Height && Convert.ToInt32(textBox1.Text) > 1)
            {
                pictureBox1.Height = Convert.ToInt32(textBox2.Text);
            }
            else { pictureBox1.Height = Height; }
            textBox1.Visible = false;
            textBox2.Visible = false;
        }
        

        private void label3_Click(object sender, EventArgs e)
        {
            PicDef();
            Figura();

        }
        private void PicDef()
        {
            dx = dy = 0;
            yg = 0;
            timer1.Stop();
            button1.Text = "Start";
            label2.Text = "x";
            label3.Text = "y";
            label3.Text = "move";
            textBox1.Visible = true;
            textBox2.Visible = true;
        }

        

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            Figura();
            
           if (dx+trackBar2.Value >= pictureBox1.Width)
            {
                Thread.Sleep(1000*Convert.ToInt32(textBox3.Text));
            }
            else
            {
                if (dx < 0)
                    Thread.Sleep(1000*Convert.ToInt32(textBox3.Text));
            }
            if (proverka < 0)
                {
               
                    dx -= smesh;
                    dy = move(dx);
                    proverka = -dy;

                    yg -= ygSpeed;
                }

                else
                {
                    dx += smesh;
                    dy = move(dx);
                    proverka = dy;

                    yg += ygSpeed;
                }
            label2.Text = "x: " + Convert.ToString(dx);
            label3.Text = "y: " + Convert.ToString(dy);
        }


        int t = 1;
        private int prist() { t = Convert.ToInt32(textBox1.Text) - 2; return t; }

       private void timer2_Tick(object sender, EventArgs e)
        {
            if (t >= 0) { timer1.Start(); timer2.Stop(); }
            t -= 1;
        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            smesh = (trackBar1.Value) / 10;
        }

        private void trackBar2_Scroll_1(object sender, EventArgs e)
        {
            diametr = trackBar2.Value;
            if (!timer1.Enabled)
            {
                Figura();//рисует фигуру
            }
        }

        private void trackBar3_Scroll_1(object sender, EventArgs e)
        {
            ygSpeed = (double)(trackBar3.Value) / 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dx = 0;
            dy = 1;
            timer1.Enabled = false;
            timer2.Enabled = false;
            Figura();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
