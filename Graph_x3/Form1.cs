using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Graph_x3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(EarthParalelsNumericUpDown, "Количество паралелей земного шара");
            tt.SetToolTip(EarthRadiusNumericUpDown, "Радиус земного шара");
            tt.SetToolTip(EarthMeridiansNumericUpDown, "Количество меридианов земного шара");
            tt.SetToolTip(EarthRndColorsRadioButton, "Случайный цвет для каждой линии");
            tt.SetToolTip(EarthFixColorsRadioButton, "Заданный цвет для всех линий");
            tt.SetToolTip(colorComboBox1, "Выбор цвета всех линий");
            tt.SetToolTip(EarthAutoReCheckBox, "Вкл./Выкл. автоматическую перерисовку\nпри изменении параметров");
            tt.SetToolTip(EarthBackGroundCheckBox, "Вкл./Выкл. фоновое изображение");
            tt.SetToolTip(EarthLineStyleComboBox, "Выбор стиля всех линий");
            tt.SetToolTip(EarthDrawButton, "Принудительно перерисовать изображение");

            colorComboBox1.SelectedItem = "DimGray";
            EarthLineStyleComboBox.SelectedIndex = 0;
            bx = 0;
            by = (int)BilliardPointNumericUpDown.Value;
            Earth();
            Flower();
            Billiard();
        }


        double S(double a)
        {
            return a * a;
        }

        bool random = false;
        bool auto = true;

        Random rnd = new Random();
        void CheckPenStyle(out Pen p)
        {
            if (random)
                p = new Pen(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)), 1);
            else
                p = new System.Drawing.Pen(Color.FromName((string)(colorComboBox1.SelectedItem)), 1);
            switch ((string)EarthLineStyleComboBox.SelectedItem)
            {
                case "Сплошная линия": p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case "Пунктир из точек": p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
                case "Штрихпунктир 1": p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; break;
                case "Штрихпунктир 2": p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot; break;
                case "Штриховая линия": p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                default: p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
            }
        }

        void Earth()
        {
           

            double r = (double)EarthRadiusNumericUpDown.Value;
            double k = (double)EarthParalelsNumericUpDown.Value;
            double m = (double)EarthMeridiansNumericUpDown.Value;

            //EarthPictureBox.Invalidate();
            GC.Collect();
            Bitmap bm = new Bitmap(EarthPictureBox.Width, EarthPictureBox.Height);
            Graphics g = Graphics.FromImage(bm);
            if (EarthBackGroundCheckBox.Checked)
            {
                Bitmap img = new Bitmap("C:\\Users\\Андрей\\Desktop\\space.bmp");
                EarthPictureBox.BackgroundImage = img;
                //Brush b = new HatchBrush(HatchStyle.Divot, Color.Gray, Color.White);
                //g.FillRectangle(b, 0, 0, EarthPictureBox.Width, EarthPictureBox.Height);
            }
            else
            {
                EarthPictureBox.BackgroundImage = null;
                //g.Clear(Color.White);
            }
            Pen pp;
            CheckPenStyle(out pp);
            g.DrawEllipse(pp, EarthPictureBox.Width / 2 - (int)r, EarthPictureBox.Height / 2 - (int)r, (int)r * 2, (int)r * 2);
            g.DrawLine(pp, EarthPictureBox.Width / 2 - (int)r, EarthPictureBox.Height / 2, EarthPictureBox.Width / 2 + (int)r, EarthPictureBox.Height / 2);
            g.DrawLine(pp, EarthPictureBox.Width / 2, EarthPictureBox.Height / 2 - (int)r, EarthPictureBox.Width / 2, EarthPictureBox.Height / 2 + (int)r);

            //линии проходят через точку (0,0), то есть b=0
            //значит, уравнение каждой прямой y=mod*x
            //есть уравнение окружности y^2 + x^2 = r^2, r уже есть
            //y тоже есть
            //(mod*x)^2 + x^2 = r^2
            //(x^2)*(mod^2+1) = r^2
            //x = sqrt(r^2/(mod^2+1));
            //y = sqrt(r^2 - x^2)
            //это точка на окружности, через которую проходит прямая
            //нужно вычислить центральную точку этой прямой
            //из этой точки построить прямую под углом 90 градусов учитывая tg(k)
            //точнее вычислить новый k и записать уравнение, при этом надо найти b
            //этот b и будет искомой точкой центра окружности
            //короче можно не заморачиваться и искать один перпендикуляр, а потом находить
            //точку пересечения с oY и из неё строить окружность
            //одно уравнение будет x=0
            //другое будет y - y1 = mod*(x - x1)
            //y = y1 - mod*x1
            //радиусом этой окружности будет расстояние от вычисленного b до точки на радиусе

            //g.DrawArc(p, 100, 100, 70, 70, 0, 0);
            //g.DrawArc(p, 100, 200, 70, 70, 0, 90);
            //g.DrawArc(p, 100, 300, 70, 70, 30, 180-60);

            //====  UPDATE  ====
            //решение через квадратное уравнение работает неправильно
            //нужно считать через углы
            //теперь всё наоборот, мне дан угловой коэффициент прямой, проходящей через (0,0) и точку на окружности
            //и нужно найти угловой коэффициент прямой, проходящей через i * (r / k) и точку на окружности
            //в начале рассуждений формулы изменены

            k++;
            for (int i = 1; i < k; i++)
            {
                double moder = Math.Tan(((Math.PI / 2) / k) * i);
                double x = Math.Sqrt(S(r) / (S(moder) + 1));
                double y = Math.Sqrt(S(r) - S(x));
                //double d = (4 * S(moder) * S(i) * (S(r) / S(k))) - (4 * (S(moder) + 1) * ((S(i) * (S(r) / S(k))) - S(r)));
                //double x1 = ((-1) * 2 * moder * i * (r / k) + Math.Sqrt(d)) / (2 * (S(moder) + 1));
                //double x2 = ((-1) * 2 * moder * i * (r / k) - Math.Sqrt(d)) / (2 * (S(moder) + 1));
                //double y1 = Math.Sqrt(S(r) - S(x1));
                //double y2 = Math.Sqrt(S(r) - S(x2));
                //double x = 0;
                //double y = 0;
                //if (y1 >= i * (r / k))
                //{
                //    y = y1;
                //    x = x1;
                //}
                //else
                //{
                //    y = y2;
                //    x = x2;
                //} //x и y - координаты точки на окружности
                double kk = (y - (i * (r / k))) / x;
                double xm = (0 + x) / 2.0; //середина
                double ym = (i * (r / k) + y) / 2.0; //середина
                double alpha = (Math.Atan(kk) * 180) / Math.PI + 90; //перпендикуляр с учетом угла наклона прямой
                ym = ym - Math.Tan((alpha * Math.PI) / 180) * xm; //точка пересечения перпендикуляра с прямой x=0
                //kk = (ym - y) / x; //угловой коэффициент прямой, проходящей через точку на окружности и найденную точку
                //kk = (Math.Atan(kk) * 180) / Math.PI;
                moder = (Math.Atan((y - ym) / x) * 180) / Math.PI;
                y = ym;
                double radius = (y - i * (r / k));
                //g.DrawEllipse(p, (float)(EarthPictureBox.Width / 2 - radius), (float)(EarthPictureBox.Height / 2 + y - radius), (float)(radius * 2), (float)(radius * 2));
                //float rx = (float)(EarthPictureBox.Width / 2 - radius);
                //float ry = (float)(EarthPictureBox.Height / 2 + y - radius);
                //float rw = (float)(radius * 2);
                //float rh = (float)(radius * 2);
                //g.DrawArc(p, rx, ry, rw, rh, 0, 60);//(-1) * (float)(kk), (-1) * (float)(180 - kk * 2));
                //RectangleF rectf = new RectangleF((float)(EarthPictureBox.Width / 2 - radius), (float)(EarthPictureBox.Height / 2 + y - radius), (float)(radius * 2), (float)(radius * 2));
                //g.DrawArc(p, rectf, 0, 90);
                //g.DrawArc(p, 100, 100, 70, 70, (float)(kk), (float)(180 - kk * 2));

                Pen p;
                CheckPenStyle(out p);
                g.DrawArc(p, (float)(EarthPictureBox.Width / 2 - radius), (float)(EarthPictureBox.Height / 2 + y - radius), (float)(radius * 2), (float)(radius * 2), (float)(moder), (float)(-(180 + moder * 2)));
                radius = (EarthPictureBox.Height / 2 - (i * (r / k))) - (EarthPictureBox.Height / 2 - y);
                CheckPenStyle(out p);
                g.DrawArc(p, (float)(EarthPictureBox.Width / 2 - radius), (float)(EarthPictureBox.Height / 2 - y - radius), (float)(radius * 2), (float)(radius * 2), (float)(-moder), (float)(180 + moder * 2));
            }

            //для построения мередиан нужно использовать тот же принцип
            //но в этот раз не надо вычислять точки на окружности - это полюса
            //то есть в уравнении y=kx+b b=r а k надо посчитать
            //k = y2 / x1;
            //к арктангенсу k прибавляем 90, а затем вычисляем x центра при y=0
            //"Ничего не работает, потому что иди нафиг" (с) Сишарп
            //Не факт, что можно провести окружность через любые три точки

            double mm = m;
            m++;
            for (int i = 1; i <= mm; i++)
            {
                /*
                double tmp = (i * (r / m));
                double moder = r / (i * (r / m));
                double angle = (Math.Atan(moder) * 180) / Math.PI;
                double alpha = 180 - ((180 - angle)+90);
                double xm = (0 + (i * (r / m))) / 2.0;
                double ym = (r + 0) / 2.0;
                double x = xm - (ym / (Math.Tan((alpha * Math.PI) / 180.0)));
                double kk = (-1) * r / x;
                kk = (Math.Atan(kk) * 180) / Math.PI;
                double radius = (i * (r / m)) + x;
                g.DrawArc(p, (float)(EarthPictureBox.Width / 2 - x - radius), (float)(EarthPictureBox.Height / 2 - radius), (float)(radius * 2), (float)(radius * 2), (float)(kk), (float)(100));
                 */
                double x1 = 0;
                double y1 = r;
                double x2 = (i * (r / m));
                double y2 = 0;
                double x3 = 0;
                double y3 = -r;
                double k1 = (y2 - y1) / (x2 - x1);
                double k2 = (y3 - y2) / (x3 - x2);
                double x = (k1 * k2 * (y1 - y3) + k2 * (x1 + x2) - k1 * (x2 + x3)) / (2 * (k2 - k1));
                double kk = (y1 - 0) / (x1 - x);
                kk = 180 - (Math.Atan(kk) * 180) / Math.PI;
                double radius = Math.Sqrt(S(x2 - x) + S(y2 - 0));
                double angle = (Math.Atan((k2 - k1) / (1 + k1 * k2)) * 180) / Math.PI;
                Pen p;
                CheckPenStyle(out p);
                g.DrawArc(p, (float)(EarthPictureBox.Width / 2 - x - radius), (float)(EarthPictureBox.Height / 2 - radius), (float)(radius * 2), (float)(radius * 2), (float)(kk), (float)(2 * (180 - kk)));
                CheckPenStyle(out p);
                g.DrawArc(p, (float)(EarthPictureBox.Width / 2 + x - radius), (float)(EarthPictureBox.Height / 2 - radius), (float)(radius * 2), (float)(radius * 2), (float)(kk - 180), (float)(2 * (180 - kk)));
            }

            //    360);//
            EarthPictureBox.Image = bm;
        }

        void Flower()
        {
            int a = (int)FlowerSideNumericUpDown.Value;
            //FlowerPictureBox.Invalidate();
            GC.Collect();
            Bitmap bm = new Bitmap(FlowerPictureBox.Width, FlowerPictureBox.Height);
            Graphics g = Graphics.FromImage(bm);
            

            if (FlowerBackGroundCheckBox.Checked)
            {
                Bitmap img = new Bitmap("C:\\Users\\Андрей\\Desktop\\grass.bmp");
                FlowerPictureBox.BackgroundImage = img;
                //Brush b = new HatchBrush(HatchStyle.Plaid, Color.LightPink, Color.White);
                //g.FillRectangle(b, 0, 0, EarthPictureBox.Width, EarthPictureBox.Height);
            }
            else
            {
                FlowerPictureBox.BackgroundImage = null;
                //g.Clear(Color.White);
            }
            Pen p = new Pen(Brushes.Black, 2);

            Point[] points = new Point[3];
            points[0] = new Point(FlowerPictureBox.Width / 2, FlowerPictureBox.Height / 2);

            int l = a / 10; //отступ от диагонали
            l = (int)Math.Sqrt(S(l) + S(l)); //отступ по осям
            int moder = a / 35;//калибровочный отступ

            GraphicsPath path = new GraphicsPath();
            Brush b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            points[1] = new Point(FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 - a / 2);
            points[2] = new Point(FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 + a / 2);
            path.AddLines(points);
            g.FillPath(b, path);

            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            points[1] = new Point(FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 + a / 2);
            points[2] = new Point(FlowerPictureBox.Width / 2 + a / 2, FlowerPictureBox.Height / 2 + a / 2);
            path.AddLines(points);
            g.FillPath(b, path);

            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            points[1] = new Point(FlowerPictureBox.Width / 2 + a / 2, FlowerPictureBox.Height / 2 + a / 2);
            points[2] = new Point(FlowerPictureBox.Width / 2 + a / 2, FlowerPictureBox.Height / 2 - a / 2);
            path.AddLines(points);
            g.FillPath(b, path);

            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            points[1] = new Point(FlowerPictureBox.Width / 2 + a / 2, FlowerPictureBox.Height / 2 - a / 2);
            points[2] = new Point(FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 - a / 2);
            path.AddLines(points);
            g.FillPath(b, path);

            points = new Point[4];
            points[0] = new Point(FlowerPictureBox.Width / 2, FlowerPictureBox.Height / 2);
            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            int xm = (FlowerPictureBox.Width / 2) - a / 4;
            int ym = (FlowerPictureBox.Height / 2) - a / 4;
            points[1] = new Point(xm + l + moder, ym + moder);
            points[2] = new Point(xm - moder, ym - l - moder);
            points[3] = new Point(FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 - a / 2);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);
            path.Reset();
            points[1] = new Point(xm + moder, ym + l + moder);
            points[2] = new Point(xm - l - moder, ym - moder);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);

            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            xm = (FlowerPictureBox.Width / 2) - a / 4;
            ym = (FlowerPictureBox.Height / 2) + a / 4;
            points[1] = new Point(xm + moder, ym - l - moder);
            points[2] = new Point(xm - l - moder, ym + moder);
            points[3] = new Point(FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 + a / 2);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);
            path.Reset();
            points[1] = new Point(xm + l + moder, ym - moder);
            points[2] = new Point(xm - moder, ym + l + moder);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);

            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            xm = (FlowerPictureBox.Width / 2) + a / 4;
            ym = (FlowerPictureBox.Height / 2) + a / 4;
            points[1] = new Point(xm - moder, ym - l - moder);
            points[2] = new Point(xm + l + moder, ym + moder);
            points[3] = new Point(FlowerPictureBox.Width / 2 + a / 2, FlowerPictureBox.Height / 2 + a / 2);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);
            path.Reset();
            points[1] = new Point(xm - l - moder, ym - moder);
            points[2] = new Point(xm + moder, ym + l + moder);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);

            b = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            path.Reset();
            xm = (FlowerPictureBox.Width / 2) + a / 4;
            ym = (FlowerPictureBox.Height / 2) - a / 4;
            points[1] = new Point(xm - l - moder, ym + moder);
            points[2] = new Point(xm + moder, ym - l - moder);
            points[3] = new Point(FlowerPictureBox.Width / 2 + a / 2, FlowerPictureBox.Height / 2 - a / 2);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);
            path.Reset();
            points[1] = new Point(xm - moder, ym + l + moder);
            points[2] = new Point(xm + l + moder, ym - moder);
            path.AddCurve(points);
            g.FillPath(b, path);
            g.DrawPath(p, path);

            g.DrawRectangle(p, FlowerPictureBox.Width / 2 - a / 2, FlowerPictureBox.Height / 2 - a / 2, a, a);
            FlowerPictureBox.Image = bm;
        }

        void Billiard()
        {
            int w = (int)BilliardWidthNumericUpDown.Value;
            int h = (int)BilliardHeightNumericUpDown.Value;
            //BilliardPictureBox.Invalidate();
            Pen p = new Pen(Brushes.Black);
            GC.Collect();
            Bitmap bm = new Bitmap(BilliardPictureBox.Width, BilliardPictureBox.Height);
            Graphics g = Graphics.FromImage(bm);
            //g.Clear(Color.White);

            g.DrawRectangle(p, BilliardPictureBox.Width / 2 - w / 2, BilliardPictureBox.Height / 2 - h / 2, w, h);

            g.DrawEllipse(p, BilliardPictureBox.Width / 2 - w / 2 + bx, BilliardPictureBox.Height / 2 + h / 2 - by, 10, 10);

            BilliardPictureBox.Image = bm;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void EarthDrawButton_Click(object sender, EventArgs e)
        {
            Earth();
        }

        private void EarthRadiusNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (auto)
                Earth();
        }

        private void EarthParalelsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (auto)
                Earth();
        }

        private void EarthMeridiansNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (auto)
                Earth();
        }

        private void colorComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EarthFixColorsRadioButton.Checked = true;
            EarthRndColorsRadioButton.Checked = false;
            if (auto)
                Earth();
        }

        private void EarthRndColorsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            random = true;
            if (auto)
            {
                Earth();
            }
        }

        private void EarthFixColorsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            random = false;
            if (auto)
            {
                Earth();
            }
        }

        private void EarthLineStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (auto)
                Earth();
        }

        private void EarthAutoReCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            auto = ((CheckBox)sender).Checked;
            if (auto)
                Earth();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                Earth();
                Flower();
                Billiard();
            }
        }

        private void EarthBackGroundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (auto)
                Earth();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void FlowerSideNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (auto2)
            Flower();
        }

        private void FlowerBackGroundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (auto2)
            Flower();
        }

        bool started = false;
        private void BilliardStartButton_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Пауза")
            {
                timer1.Stop();
                ((Button)sender).Text = "Старт";
            }
            else
            {
                if (!started)
                {
                    direction = 2;
                    BilliardHeightNumericUpDown.Enabled = false;
                    BilliardPointNumericUpDown.Enabled = false;
                    BilliardStopButton.Enabled = true;
                    BilliardWidthNumericUpDown.Enabled = false;
                    bx = 0;
                    by = (int)BilliardPointNumericUpDown.Value;
                    Billiard();
                }
                ((Button)sender).Text = "Пауза";
                started = true;
                timer1.Start();//start
            }
        }

        private void BilliardStopButton_Click(object sender, EventArgs e)
        {
            BilliardHeightNumericUpDown.Enabled = true;
            BilliardPointNumericUpDown.Enabled = true;
            BilliardStartButton.Text = "Старт";
            BilliardStopButton.Enabled = false;
            BilliardWidthNumericUpDown.Enabled = true;
            timer1.Stop();//stop
            bx = 0;
            by = (int)BilliardPointNumericUpDown.Value;
            started = false;
            Billiard();
        }

        private void BilliardHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            BilliardPointNumericUpDown.Maximum = BilliardHeightNumericUpDown.Value;
            //by = BilliardPictureBox.Height / 2 + (int)BilliardHeightNumericUpDown.Value / 2 - (int)BilliardPointNumericUpDown.Value;
            Billiard();
        }

        private void BilliardWidthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //bx = BilliardPictureBox.Width / 2 - (int)BilliardWidthNumericUpDown.Value / 2;
            Billiard();
        }

        private void BilliardPointNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            by = (int)BilliardPointNumericUpDown.Value;
            Billiard();
        }

        int bx = 0;
        int by = 0;
        int direction = 2;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = (6 - BilliardSpeedTrackBar.Value)*10;
            if (bx >= BilliardWidthNumericUpDown.Value - 11)//правая стена
            {
                if (direction == 2)
                {
                    direction = 1;
                }
                else if (direction == 3)
                {
                    direction = 4;
                }
            }
            else
                if (by <= 11)//нижняя стена
                {
                    if (direction == 4)
                    {
                        direction = 1;
                    }
                    else if (direction == 3)
                    {
                        direction = 2;
                    }
                }
                else
                    if (bx <= 1)//левая стена
                    {
                        if (direction == 1)
                        {
                            direction = 2;
                        }
                        else if (direction == 4)
                        {
                            direction = 3;
                        }
                    }
                    else
                        if (by >= BilliardHeightNumericUpDown.Value-1)//верхняя стена
                        {
                            if (direction == 2)
                            {
                                direction = 3;
                            }
                            else if (direction == 1)
                            {
                                direction = 4;
                            }
                        }
            switch (direction)
            {
                case 1: bx--; by++; break;
                case 2: bx++; by++; break;
                case 3: bx++; by--; break;
                case 4: bx--; by--; break;
            }
            Billiard();
        }

        private void EarthPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EarthPictureBox.Image != null)
            {
                sfd.OverwritePrompt = true;
                sfd.CheckPathExists = true;
                sfd.ShowHelp = true;
                sfd.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(sfd.FileName);
                        string text = String.Format("Имя файла: {0}; Радиус: {1}; Количество паралелей: {2};\nКоличество меридианов: {3};", fi.Name, EarthRadiusNumericUpDown.Value, EarthParalelsNumericUpDown.Value, EarthMeridiansNumericUpDown.Value);
                         //   text = String.Format("Имя файла: {0}; Сторона квадрата: {1};", fi.Name, FlowerSideNumericUpDown.Value);
                        Brush b = new SolidBrush(Color.Gray);
                        PictureBox pbtmp = EarthPictureBox;
                        Bitmap bmp = new Bitmap(pbtmp.Width, pbtmp.Height + 30);
                        pbtmp.DrawToBitmap(bmp, new Rectangle(0, 0, pbtmp.Width, pbtmp.Height));
                        Graphics G = Graphics.FromImage(bmp);
                        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                        G.DrawString(text, Font, b, 20, bmp.Height - 30);
                        bmp.Save(sfd.FileName, ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //if (this.WindowState != FormWindowState.Minimized)
            //{
            //    Earth();
            //    Flower();
            //}
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void tabPage1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            Earth();
        }

        private void tabPage2_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            Flower();
        }

        private void tabPage3_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            Billiard();
        }

        bool auto2 = true;
        private void автомПерерисовкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auto = !auto;
            EarthAutoReCheckBox.Checked = !EarthAutoReCheckBox.Checked;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (FlowerPictureBox.Image != null)
            {
                sfd.OverwritePrompt = true;
                sfd.CheckPathExists = true;
                sfd.ShowHelp = true;
                sfd.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(sfd.FileName);
                        string text = String.Format("Имя файла: {0}; Сторона квадрата: {1};", fi.Name, FlowerSideNumericUpDown.Value);
                        Brush b = new SolidBrush(Color.Gray);
                        PictureBox pbtmp = FlowerPictureBox;
                        Bitmap bmp = new Bitmap(pbtmp.Width, pbtmp.Height + 30);
                        pbtmp.DrawToBitmap(bmp, new Rectangle(0, 0, pbtmp.Width, pbtmp.Height));
                        Graphics G = Graphics.FromImage(bmp);
                        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                        G.DrawString(text, Font, b, 20, bmp.Height - 30);
                        bmp.Save(sfd.FileName, ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void автомПерерисовкаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            auto2 = !auto2;
        }

        private void перерисоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                Earth();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void перерисоватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                Flower();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
