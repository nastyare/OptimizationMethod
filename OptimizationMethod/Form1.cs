using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Symbolics;
using MathNet.Numerics;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Generic;

namespace OptimizationMethod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menuStrip_ItemClicked(object sender, EventArgs e)
        {

        }

        double F(double x)
        {
            string formula = equationBox.Text;
            org.matheval.Expression expression = new org.matheval.Expression(formula.ToLower());
            expression.Bind("x", x);
            double value = expression.Eval<double>();
            return value;
        }

        double antiF(double x)
        {
            return -F(x);
        }

        void DescentMethodRoot(double a, double b, double epsilon)
        {

            double x = (a + b) / 2; // Начальное приближение

            while (Math.Abs(F(x)) > epsilon)
            {
                // Обновление координаты x по очереди
                for (int i = 0; i < 100; i++) // Максимальное количество итераций
                {
                    double prevX = x;
                    x = a + (F(b) * (b - a)) / (F(b) - F(a));
                    if (Math.Abs(F(x)) <= epsilon || Math.Abs(x - prevX) <= epsilon)
                        break;
                    if (F(x) * F(a) < 0)
                        b = x;
                    else
                        a = x;
                }
                if (F(x) * F(a) < 0)
                    b = x;
                else
                    a = x;
            }


            funBox.Text = x.ToString();

        }
        public double CoordinateDescentMin(double interval1, double interval2, int accuracy)
        {
            double a = interval1, b = interval2;
            double x = (a + b) / 2; // Инициализация начального значения x
            double delta = 1 / Math.Pow(10, accuracy); // Вычисление дельты по точности n

            while (b - a > delta) // Условие остановки
            {
                if (F(a) > F(b))
                    a = x; // Если значение функции в точке a больше, обновляем начальную границу
                else
                    b = x; // Иначе обновляем конечную границу

                x = (a + b) / 2; // Вычисление нового значения x
            }

            return Math.Round(x, accuracy); // Возвращаем значение x с заданной точностью n
        }
        public double AntiCoordinateDescent(double interval1, double interval2, int accuracy)
        {
            double a = interval1, b = interval2;
            double x = (a + b) / 2; // Инициализация начального значения x
            double delta = 1 / Math.Pow(10, accuracy); // Вычисление дельты по точности n

            while (b - a > delta) // Условие остановки
            {
                if (-F(a) > -F(b))
                    a = x; // Если значение функции в точке a больше, обновляем начальную границу
                else
                    b = x; // Иначе обновляем конечную границу

                x = (a + b) / 2; // Вычисление нового значения x
            }

            return Math.Round(x, accuracy); // Возвращаем значение x с заданной точностью n
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double a, b, Xi, n;
            if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            if (a >= b)
            {
                throw new ArgumentException("Некорректные границы интервала");
            }
            this.chart1.Series[0].Points.Clear();
            double x = a;
            double y;
            while (x <= b)
            {
                y = F(x);
                this.chart1.Series[0].Points.AddXY(x, y);
                x += 0.1;
            }
            DescentMethodRoot(a, b, Xi);

            double resultMin = CoordinateDescentMin(a, b, (int)-Math.Log10(Xi));
            double resultMax = AntiCoordinateDescent(a, b, (int)-Math.Log10(Xi));
            XBox.Text = resultMin.ToString();
            YBox.Text = resultMax.ToString();
            if (resultMin == a || resultMin == b)
            {
                throw new ArgumentException("Точки минимум нет на данном интервале");
            }
            else
            {
                XBox.Text = resultMin.ToString();
            }
            if (resultMax == a || resultMax == b)
            {
                throw new ArgumentException("Точки максимум нет на данном интервале");
            }
            else
            {
                XBox.Text = resultMax.ToString();
            }
        }
  
        //2*x^2+y^2-x*y

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            equationBox.Clear();
            XBox.Clear();
            YBox.Clear();
            chart1.Series.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void minimumBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
