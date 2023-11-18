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

        double F(double x, double y, string formula)
        {
            org.matheval.Expression expression = new org.matheval.Expression(formula.ToLower());
            expression.Bind("x", x);
            expression.Bind("y", y);
            double value = expression.Eval<double>();
            return (double)value;
        }

        static double[] CoordinateDescentOptimization(Func<double, double, double> targetFunction, double initialX, double initialY, double tolerance, int maxIterations = 1000)
        {
            double x = initialX;
            double y = initialY;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                double oldX = x;
                double oldY = y;

                x = OptimizeCoordinate(targetFunction, x, y, "x");
                y = OptimizeCoordinate(targetFunction, y, x, "y");

                double changeX = Math.Abs(x - oldX);
                double changeY = Math.Abs(y - oldY);

                if (changeX < tolerance && changeY < tolerance)
                {
                    break;
                }
            }

            return new double[] { x, y };
        }

        static double OptimizeCoordinate(Func<double, double, double> targetFunction, double coordinateValue, double otherCoordinate, string coordinateName)
        {
            double alpha = 0.01; // шаг обучения
            double gradient = targetFunction(coordinateValue, otherCoordinate);

            return coordinateValue - alpha * gradient;
        }
        private void DisplayChart(Func<double, double, double> targetFunction)
        {
            chart1.Series.Clear();

            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
            series.ChartType = SeriesChartType.Line;

            for (double x = -10; x <= 10; x += 0.1)
            {
                double y = targetFunction(x, 0); // Фиксируем одну переменную, например, y = 0
                series.Points.AddXY(x, y);
            }

            chart1.Series.Add(series);
            chart1.Invalidate();
        }

        private void maximumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string formula = equationBox.Text;
            Func<double, double, double> targetFunction = (x, y) => F(x, y, formula);

            double initialX = Convert.ToDouble(textBox1.Text);
            double initialY = Convert.ToDouble(textBox2.Text);
            double tolerance = Convert.ToDouble(textBox3.Text);

           
            double[] result = CoordinateDescentOptimization(targetFunction, initialX, initialY, tolerance);
            XBox.Text = $"{result[0]}";
            YBox.Text = $"{result[1]}";
            DisplayChart(targetFunction);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void minimumBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
