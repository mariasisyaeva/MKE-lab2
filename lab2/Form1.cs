using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace lab2
{
    public partial class Form1 : Form
    {
        Thread myThread;
        DataGridView dgv;
        double Nx = 5;
        double Ny = 10;
        double h = 1;
        float temp4 = 10;
        float temp2 = 20;

        public Form1()
        {
            InitializeComponent();
            dgv = dataGridView1;
            int n = (int)(Nx / h);
            int m = (int)(Ny / h);
            dgv.ColumnCount = n;
            dgv.RowCount = m;
        }

        private void calculation()
        {
            const double a = 10;
            double h = 1;
            double tau = 0.01;
            double t0 = 0;
            double tmax = 5;
            double R = a  * tau / h;
            int n = (int)(Nx / h);
            int m = (int)(Ny / h);
            double[,] result = new double[m, n];
            double[,] item = new double[m, n];

            float r = 255 / (temp4 > temp2 ? temp4 + 1: temp2 + 1);

            if (R < 0.5)
            {
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        result[i, j] = item[i, j] = 0;
                        //сторона 3
                        if (i == 0)
                            item[i, j] = result[i, j] = 0;
                        //сторона 1
                        if (i == m - 1)
                            item[i, j] = result[i, j] = 0;
                    }
                    //сторона 2
                    item[i, n - 1] = result[i, n - 1] = temp2;
                    //сторона 4
                    item[i, 0] = result[i, 0] = temp4;
                }

                // отрисовка исходных данных
                for (int i = 0; i < m ; i++)
                {
                    for (int j = 0; j < n ; j++)
                    {
                        dgv[j, i].Value = item[i, j];
                        dgv[j, i].Style.BackColor = Color.FromArgb(255, (int)(result[i, j] * r), 255 - (int)(result[i, j] * r), 0);
                    }
                }

               // вычисления
                for (double t = t0 + tau; t <= tmax; t += tau)
                {
                    for (int i = 1; i < m - 1; i++)
                    {
                        for (int j = 1; j < n - 1; j++)
                        {
                            result[i, j] = item[i, j] + R * (item[i + 1, j] + item[i - 1, j] + item[i, j + 1] + item[i, j - 1] - 4 * item[i, j]);
                            dgv[j, i].Value = String.Format("{0:0.00}", result[i, j]);
                            dgv[j, i].Style.BackColor = Color.FromArgb(255, (int)(result[i, j] * r), 255 - (int)(result[i, j] * r), 0);
                        }
                    }
                    UpdateData(item, result);
                    Thread.Sleep(100);
                    
                }
            }
        }

        private void UpdateData(double[,] A, double[,] B)
        {
            for (int i = 0; i < A.GetLength(0); i++)
                for (int j = 0; j < A.GetLength(1); j++)
                    A[i, j] = B[i, j];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(myThread != null)
                myThread.Abort();

            myThread = new Thread(new ThreadStart(calculation));
            myThread.Start(); // запускаем поток
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(myThread != null)
                myThread.Abort();
            myThread = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myThread != null)
                myThread.Abort();
            myThread = null;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            this.dgv.ClearSelection();
        }
    }
}
