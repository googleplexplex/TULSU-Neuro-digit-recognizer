using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace demoapp
{
    public partial class Form1 : Form, IView
    {
        private double[] _inputPixels = new double[784];
        public double[] InputPixels { get => _inputPixels; }

        public double[] NetOutput { set
            {
                label1.Text = value.ToList().IndexOf(value.Max()).ToString();
                textBox1.Text = "0 - " + Math.Round(value.ToList()[0], 5) + Environment.NewLine +
                    "1 - " + Math.Round(value.ToList()[1], 5) + Environment.NewLine +
                    "2 - " + Math.Round(value.ToList()[2], 5) + Environment.NewLine +
                    "3 - " + Math.Round(value.ToList()[3], 5) + Environment.NewLine +
                    "4 - " + Math.Round(value.ToList()[4], 5) + Environment.NewLine +
                    "5 - " + Math.Round(value.ToList()[5], 5) + Environment.NewLine +
                    "6 - " + Math.Round(value.ToList()[6], 5) + Environment.NewLine +
                    "7 - " + Math.Round(value.ToList()[7], 5) + Environment.NewLine +
                    "8 - " + Math.Round(value.ToList()[8], 5) + Environment.NewLine +
                    "9 - " + Math.Round(value.ToList()[9], 5);
            }
        }

        public event EventHandler<EventArgs> GotResult;

        public Form1()
        {
            InitializeComponent();

            button1_Click(null, null);
        }
        private void Form1_Load(object sender, EventArgs e) { }


        private void paint(int x, int y, SolidBrush p, Graphics fig, MouseEventArgs e)
        {
            if (x < 0 || y < 0 || x >= 28 || y >= 28)
                return;

            fig.FillRectangle(p, x*10, y*10, 10, 10);

            if (e.Button == MouseButtons.Left)
            {
                _inputPixels[y * 28 + x] = 1d;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _inputPixels[y * 28 + x] = 0d;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap myBitmap = new Bitmap(pictureBox1.Image);
            Graphics fig = Graphics.FromImage(myBitmap);

            SolidBrush p;
            if (e.Button == MouseButtons.Left)
                p = new SolidBrush(Color.Black);
            else if (e.Button == MouseButtons.Right)
                p = new SolidBrush(Color.White);
            else
                return;


            int x = (e.X - e.X % 10) / 10;
            int y = (e.Y - e.Y % 10) / 10;
            paint(x, y, p, fig, e);
            paint(x+1, y, p, fig, e);
            paint(x, y+1, p, fig, e);
            paint(x+1, y+1, p, fig, e);

            GotResult?.Invoke(this, EventArgs.Empty);

            pictureBox1.Image = myBitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics fig = Graphics.FromImage(myBitmap);
            SolidBrush p = new SolidBrush(Color.White);
            fig.FillRectangle(p, 0, 0, 280, 280);
            pictureBox1.Image = myBitmap;

            for (int i = 0; i < 784; i++)
            {
                _inputPixels[i] = 0d;
            }
        }
    }
}
